using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Maomi.Mapper
{
	/// <summary>
	/// 创建映射
	/// </summary>
	public partial class MaomiMapper
	{
		// 静态实例
		private static readonly MaomiMapper Instance = new();

		/// <summary>
		/// <see cref="MaomiMapper.Bind{TSource, TTarget}(Action{MapOption}?)"/>
		/// </summary>
		private static readonly MethodInfo BindMethodInfo;

		static MaomiMapper()
		{
			var ms = typeof(MaomiMapper).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
			ASMethodInfo = ms.FirstOrDefault(x => x.IsGenericMethod && x.Name == "AS")!;
			MapMethodInfo = ms.FirstOrDefault(x => x.IsGenericMethod && x.Name == "Map")!;
			BindMethodInfo = ms.FirstOrDefault(x => x.IsGenericMethod && x.Name == "Bind")!;
		}
		
		public MaomiMapper(){}

		// 记录映射信息
		private readonly HashSet<MapperData> Maps = new();

		/// <summary>
		/// 全局静态实例
		/// </summary>
		public static MaomiMapper Static => Instance;

		/// <summary>
		/// 如果没有设置 A => B 类型的映射，则框架自动创建映射
		/// </summary>
		public bool IsAutoBuild { get; set; } = true;

		/// <summary>
		/// 绑定映射
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <param name="action">映射配置</param>
		/// <returns></returns>
		public MapperBuilder<TSource, TTarget> Bind<TSource, TTarget>(Action<MapOption>? action = null)
			where TSource : class
			where TTarget : class
		{
			MapOption option = new MapOption();
			if (action != null) action.Invoke(option);

			MapperData? data = Maps.FirstOrDefault(x => x.MapInfo.Source == typeof(TSource) && x.MapInfo.Target == typeof(TTarget));
			if (data == null)
			{
				var mapInfo = new MapInfo(typeof(TSource), typeof(TTarget), option);
				var builder = new MapperBuilder<TSource, TTarget>(this, mapInfo, option);

				data = new MapperData(builder, mapInfo);
				Maps.Add(data);
				return builder;
			}
			else
			{
				return (data.MapperBuilder as MapperBuilder<TSource, TTarget>)!;
			}


		}

		/// <summary>
		/// 绑定映射
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <param name="action">映射配置</param>
		/// <returns></returns>
		public static MapperBuilder<TSource, TTarget> BindTo<TSource, TTarget>(Action<MapOption>? action = null)
			where TSource : class
			where TTarget : class
			=> Instance.Bind<TSource, TTarget>(action);

		/// <summary>
		/// 绑定映射
		/// </summary>
		/// <param name="sourceType">源类型</param>
		/// <param name="targetType">目标类型</param>
		/// <param name="action">映射配置</param>
		/// <returns></returns>
		public void Bind(Type sourceType, Type targetType, Action<MapOption>? action = null)
		{
			var method = BindMethodInfo.MakeGenericMethod(sourceType, targetType);
			method.Invoke(this, new object?[] { action });
		}

		/// <summary>
		/// 绑定映射
		/// </summary>
		/// <param name="sourceType">源类型</param>
		/// <param name="targetType">被映射的类型</param>
		/// <param name="action">映射配置</param>
		public static void BindTo(Type sourceType, Type targetType, Action<MapOption>? action = null)
			=> Instance.Bind(sourceType, targetType, action);

		/// <summary>
		/// 映射
		/// </summary>
		/// <param name="source">源对象</param>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <returns></returns>
		public TTarget Map<TSource, TTarget>(TSource source)
			where TSource : class
			where TTarget : class, new()
		{
			TTarget target = new TTarget();
			Map(source, target);
			return target;
		}

		/// <summary>
		/// 获取映射值
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">被映射的类型</typeparam>
		/// <param name="source">源对象</param>
		/// <returns></returns>
		public static TTarget MapTo<TSource, TTarget>(TSource source)
			where TSource : class
			where TTarget : class, new()
		{
			TTarget target = new TTarget();
			MapTo(source, target);
			return target;
		}

		/// <summary>
		/// 获取映射值
		/// </summary>
		/// <param name="source">源对象</param>
		/// <param name="target">目标对象</param>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">被映射的类型</typeparam>
		/// <returns></returns>
		public static TTarget MapTo<TSource, TTarget>(TSource source, TTarget target)
			where TSource : class
			where TTarget : class
			=> Instance.Map<TSource, TTarget>(source, target);

		/// <summary>
		/// 获取映射值
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">被映射的类型</typeparam>
		/// <param name="source">源对象</param>
		/// <param name="target">被映射对象</param>
		/// <exception cref="InvalidCastException"></exception>
		public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
			where TSource : class
			where TTarget : class
		{
			var mapperData = Maps.FirstOrDefault(x => x.MapInfo.Source == typeof(TSource) && x.MapInfo.Target == typeof(TTarget));
			if (mapperData == null)
			{
				// 对未创建的服务
				if (IsAutoBuild)
				{
					// 运行时创建映射绑定
					Bind<TSource, TTarget>().Build();
					mapperData = Maps.FirstOrDefault(x => x.MapInfo.Source == typeof(TSource) && x.MapInfo.Target == typeof(TTarget));
				}
			}

			if (mapperData == null) throw new InvalidCastException($"未创建 {typeof(TSource).Name} 到的映射 {typeof(TTarget).Name}，该错误为框架内部错误，请提交 Issue！");
			// 如果开发者没有调用 Build()
			if (!mapperData.IsBuild) mapperData.Build();

			var mapInfo = mapperData.MapInfo;

			// 对 TTarget 的字段或属性逐个映射
			foreach (var item in mapInfo.MemberInfos)
			{
				// 已提前配置映射委托代码
				bool hasFunc = mapInfo.Binds.TryGetValue(item, out var @delegate);
				// 查找不到映射规则
				if (!hasFunc) continue;

				if (item is FieldInfo field)
				{
					// 如果不处理私有字段
					if (field.IsPrivate && !mapInfo.MapOption.IncludePrivateField) continue;

					// 忽略运行时生成的属性的私有字段
					if (item.Name.EndsWith(">k__BackingField")) continue;

					try
					{
						@delegate!.DynamicInvoke(source, target);
					}
					catch (Exception ex)
					{
						throw new InvalidCastException($"从 {typeof(TSource).Name} => [{field.FieldType.Name}]{typeof(TTarget).Name}.{field.Name} 出错，请检查异常信息： ", ex);
					}
				}
				else if (item is PropertyInfo property)
				{
					if (!property.CanWrite) continue;
					try
					{
						@delegate!.DynamicInvoke(source, target);
					}
					catch (Exception ex)
					{
						throw new InvalidCastException($"从 {typeof(TSource).Name}  =>   [{property.PropertyType.Name}]{typeof(TTarget).Name}.{property.Name} ，请检查异常信息：", ex);
					}
				}
			}
			return target;
		}

		/// <summary>
		/// 扫描程序集
		/// </summary>
		/// <param name="assemblies">需要扫描的所有程序集</param>
		/// <param name="mapFilter">过滤器</param>
		public void Scan(Assembly[] assemblies, Func<Type, Type, bool>? mapFilter = null)
		{
			foreach (var assembly in assemblies)
			{
				foreach (var type in assembly.GetTypes())
				{
					if (type.IsAssignableTo(typeof(IMapper)))
					{
						var mapper = Activator.CreateInstance(type) as IMapper;
						if (mapper != null)
						{
							mapper.Bind(Instance);
						}
						continue;
					}

					var mapAttribute = type.GetCustomAttribute<MapAttribute>();
					var optionAttribute = type.GetCustomAttribute<MapOptionAttribute>();
					if (mapAttribute == null) continue;

					foreach (var mapType in mapAttribute.Types)
					{
						if (mapFilter != null)
							if (!mapFilter.Invoke(type, mapType)) continue;
						this.Bind(type, mapType, optionAttribute?.MapOption);

						// 反向绑定
						if (mapAttribute.IsReverse)
						{
							if (mapFilter != null)
								if (!mapFilter.Invoke(mapType, type)) continue;
							this.Bind(mapType, type, optionAttribute?.MapOption);
						}
					}
				}
			}
		}

		// 不扫描的程序集
		private static readonly string[] CLRAssembly = new string[] { "System.", "Microsoft." };

		/// <summary>
		/// 扫描当前进程所有程序集。<br />
		/// 不扫描 "System.", "Microsoft." 开头的程序集
		/// </summary>
		/// <param name="assemblyFilter">程序集过滤器</param>
		/// <param name="mapFilter">映射过滤器</param>
		public void Scan(Func<Assembly, bool>? assemblyFilter = null, Func<Type, Type, bool>? mapFilter = null)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			// 不扫描 CLR 程序集
			assemblies = assemblies.Where(x => CLRAssembly.Any(c => x.GetName().Name?.StartsWith(c) == true)).ToArray();
			if (assemblyFilter != null)
			{
				List<Assembly> list = new List<Assembly>();
				foreach (var item in assemblies)
				{
					if (assemblyFilter.Invoke(item))
					{
						list.Add(item);
					}
				}
				assemblies = list.ToArray();
			}
			Scan(assemblies, mapFilter);
		}

	}
}
