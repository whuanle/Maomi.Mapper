using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Maomi.Mapper
{
	public abstract class MapperBuilder
	{
		public abstract Mapper Build();
	}

	/// <summary>
	/// 映射构建器，<see cref="TSource"/> => <see cref="TTarget"/>
	/// </summary>
	/// <typeparam name="TSource">源类型</typeparam>
	/// <typeparam name="TTarget">被映射的类型</typeparam>
	public sealed class MapperBuilder<TSource, TTarget> : MapperBuilder
		where TSource : class
		where TTarget : class
	{
		private readonly Mapper _mapper;
		private readonly MapInfo _mapInfo;
		private readonly MapOption _mapOption;

		internal MapperBuilder(Mapper mapper, MapInfo mapInfo, MapOption mapOption)
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
			MemberInfo p = GetMember(field);
			Mapper.SetDefaultValue<TTarget>(p);
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
			MemberInfo p = GetMember(field);

			// 生成 (a , b) =>
			// {
			//		b.Value = b.Value
			// }

			_mapInfo.Binds[p] = Mapper.BuildAssign<TSource, TTarget>(p, valueFunc);
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
				throw new NotSupportedException($"{body.ToString()} 不是有效的字段或属性");
			}

			var p = _mapInfo.MemberInfos.FirstOrDefault(x => x.Name == name);
			if (p == null)
			{
				if (name == "-") throw new NotSupportedException($"未找到指定字段或属性：{body.ToString()}");
				else throw new NotSupportedException($"{name} 不是有效的字段或属性");
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
		/// <see cref="Single"/>、
		/// <see cref="Double"/>、
		/// <see cref="Decimal"/>、
		/// <see cref="Char"/> <br />
		/// 注意，转换时可能会出现精确度丢失
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="t1"></param>
		/// <returns></returns>
		public static T2 AS<T1, T2>(T1 t1)
			where T1 : struct
			where T2 : struct
			=> Mapper.AS<T1, T2>(t1);

		// 允许映射到私有属性字段
		public override Mapper Build()
		{
			foreach (var item in _mapInfo.MemberInfos)
			{
				bool hasDelegate = _mapInfo.Binds.TryGetValue(item, out _);
				if (hasDelegate) continue;

				// 不自动处理未配置映射的字段或属性
				if (!_mapOption.AutoMap) continue;

				if (item is FieldInfo field)
				{
					// 忽略属性的私有字段
					if (item.Name.EndsWith(">k__BackingField")) continue;

					// 如果不处理私有字段
					if (!_mapOption.IncludePrivateField && field.IsPrivate) continue;

					Delegate assignDel = Mapper.MapField<TSource, TTarget>(field, _mapOption.ObjectReference);
					_mapInfo.Binds.Add(item, assignDel);
				}
				else if (item is PropertyInfo property)
				{
					if (!property.CanWrite) continue;

					Delegate assignDel = Mapper.MapProperty<TSource, TTarget>(property, _mapOption.ObjectReference);
					_mapInfo.Binds.Add(item, assignDel);
				}
			}

			return _mapper;
		}
	}
}
