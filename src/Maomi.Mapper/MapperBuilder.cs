using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Maomi.Mapper
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MapperBuilder
	{
		/// <summary>
		/// 已被构建
		/// </summary>
		public bool IsBuild { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract MaomiMapper Build();
	}

	/// <summary>
	/// 映射构建器
	/// </summary>
	/// <typeparam name="TSource">源类型</typeparam>
	/// <typeparam name="TTarget">被映射的类型</typeparam>
	public sealed class MapperBuilder<TSource, TTarget> : MapperBuilder
		where TSource : class
		where TTarget : class
	{
		private readonly MaomiMapper _mapper;
		private readonly MapInfo _mapInfo;
		private readonly MapOption _mapOption;

		internal MapperBuilder(MaomiMapper mapper, MapInfo mapInfo, MapOption mapOption)
		{
			_mapper = mapper;
			_mapInfo = mapInfo;
			_mapOption = mapOption;
		}

		/// <summary>
		/// 忽略映射字段，该字段将会被忽略
		/// </summary>
		/// <typeparam name="TField">字段或属性</typeparam>
		/// <param name="field">要被忽略的字段或属性，也可以使用名称，示例：<br />
		/// b => b.Value <br />
		/// b => "Value" <br />
		/// b => "_value" -- 访问不到的私有字段
		/// </param>
		/// <returns></returns>
		public MapperBuilder<TSource, TTarget> Ignore<TField>(Expression<Func<TTarget, TField>> field)
		{
			IsBuild = false;

			MemberInfo p = GetMember(field);
			_mapInfo.Binds[p] = new FieldOption
			{
				IsIgnore = true
			};
			return this;
		}

		/// <summary>
		/// 字段或属性映射
		/// </summary>
		/// <typeparam name="TValue">取值</typeparam>
		/// <typeparam name="TField">字段或属性</typeparam>
		/// <param name="valueFunc">取值表达式，示例：source => source.Value + 1 </param>
		/// <param name="field">要被映射的字段或属性，也可以使用名称，示例：<br />
		/// b => b.Value <br />
		/// b => "Value" <br />
		/// b => "_value" -- 访问不到的私有字段
		/// </param>
		/// <returns></returns>
		public MapperBuilder<TSource, TTarget> Map<TValue, TField>(Func<TSource, TValue> valueFunc,
			Expression<Func<TTarget, TField>> field)
		{
			IsBuild = false;

			MemberInfo p = GetMember(field);
			_mapInfo.Binds[p] = new FieldOption
			{
				Delegate = valueFunc
			};
			return this;
		}

		private MemberInfo GetMember<TField>(Expression<Func<TTarget, TField>> field)
		{
			var body = field.Body;

			string name = "";
			if (body is MemberExpression memberExpression)
			{
				MemberInfo member = memberExpression.Member;
				name = member.Name;
			}
			else if (body is ParameterExpression parameterExpression)
			{
				name = parameterExpression.Name ?? "-";
			}
			else if (body is ConstantExpression constantExpression)
			{
				name = constantExpression.Value?.ToString() ?? "-";
			}
			else
			{
				throw new KeyNotFoundException($"{typeof(TTarget).Name} 中不存在名为 {body.ToString()} 的字段或属性，请检查表达式！");
			}

			var p = _mapInfo.MemberInfos.FirstOrDefault(x => x.Name == name);
			if (p == null)
			{
				throw new KeyNotFoundException($"{typeof(TTarget).Name} 中不存在名为 {body.ToString()} 的字段或属性，请检查表达式！");
			}

			return p;
		}


		/// <summary>
		/// 值类型映射，支持以下类型互转：<br />
		/// <see cref="Boolean"/>、
		/// <see cref="SByte"/>、
		/// <see cref="Byte"/>、
		/// <see cref="Int16"/>、
		/// <see cref="UInt16"/>、
		/// <see cref="Int32"/>、
		/// <see cref="UInt32"/>、
		/// <see cref="Int64"/>、
		/// <see cref="UInt64"/>、
		/// <see cref="Single"/>、
		/// <see cref="Double"/>、
		/// <see cref="Decimal"/>、
		/// <see cref="Char"/> 。<br />
		/// 不支持 <see cref="DateTime"/> 。<br />
		/// 注意，大类型转小类型等情况，转换时可能会出现精确度丢失。
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="t1"></param>
		/// <returns></returns>
		public static T2 AS<T1, T2>(T1 t1)
			where T1 : struct
			where T2 : struct
			=> MaomiMapper.AS<T1, T2>(t1);

		/// <summary>
		/// 构建当前映射，并反向映射
		/// </summary>
		/// <param name="action">映射配置</param>
		/// <returns></returns>
		public MapperBuilder<TTarget, TSource> BuildAndReverse(Action<MapOption>? action)
		{
			var mapper = Build();
			return mapper.Bind<TTarget, TSource>(action);
		}

		/// <summary>
		/// 预先构建，处理没被手动配置的字段
		/// </summary>
		/// <returns></returns>
		public override MaomiMapper Build()
		{
			IsBuild = true;
			List<Expression> exList = new List<Expression>();

			// TSource a;
			// TTarget b;
			ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "_a");
			ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "_b");

			foreach (var item in _mapInfo.MemberInfos)
			{
				if (_mapInfo.Binds.TryGetValue(item, out var option))
				{
					if (!option.IsIgnore && option.Delegate != null)
						exList.Add(MaomiMapper.BuildAssign<TSource, TTarget>(sourceParameter, targetParameter, item, option.Delegate));
					continue;
				}

				// 不自动处理未配置映射的字段或属性
				if (!_mapOption.AutoMap) continue;

				if (item is FieldInfo field)
				{
					// 忽略属性的私有字段
					if (item.Name.EndsWith(">k__BackingField")) continue;

					// 如果不处理私有字段
					if (!_mapOption.IncludePrivate && field.IsPrivate) continue;
					Expression assignDel = _mapper.MapFieldOrProperty<TSource, TTarget>(sourceParameter, targetParameter, field, _mapOption);
					exList.Add(assignDel);
				}
				else if (item is PropertyInfo property)
				{
					if (!property.CanWrite) continue;
					Expression assignDel = _mapper.MapFieldOrProperty<TSource, TTarget>(sourceParameter, targetParameter, property, _mapOption);
					exList.Add(assignDel);
				}
			}

			var block = Expression.Block(exList);
			var del = Expression.Lambda(block, sourceParameter, targetParameter).Compile();
			_mapInfo.Delegate = del;
			return _mapper;
		}
	}
}
