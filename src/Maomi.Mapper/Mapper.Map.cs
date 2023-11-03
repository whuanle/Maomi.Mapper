using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maomi.Mapper
{
	public partial class MaomiMapper
	{
		/// <summary>
		/// <see cref="MaomiMapper.Map{TSource, TTarget}(TSource, TTarget)"/>
		/// </summary>
		private static readonly MethodInfo MapMethodInfo;

		/// <summary>
		/// 映射字段或属性，构造：<br />
		/// (a , b) => <br />
		/// {
		///  ... ...
		/// }
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <param name="sourceParameter">源对象</param>
		/// <param name="targetParameter">目标对象</param>
		/// <param name="targetField">目标属性 b.Value</param>
		/// <param name="mapOption">映射配置</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		internal Expression MapFieldOrProperty<TSource, TTarget>(ParameterExpression sourceParameter, ParameterExpression targetParameter, MemberInfo targetField, MapOption mapOption)
		{
			// b.Value
			MemberExpression targetMember;
			Type targetFieldType;
			{
				if (targetField is FieldInfo fieldInfo)
				{
					targetFieldType = fieldInfo.FieldType;
					targetMember = Expression.Field(targetParameter, fieldInfo);
				}
				else if (targetField is PropertyInfo propertyInfo)
				{
					targetFieldType = propertyInfo.PropertyType;
					targetMember = Expression.Property(targetParameter, propertyInfo);
				}
				else
				{
					throw new InvalidCastException(
						$"框架处理出错，请提交 Issue！ {typeof(TTarget).Name}.{targetField.Name} 既不是字段也不是属性");
				}
			}

			var sourceField = typeof(TSource).GetMember(targetField.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

			// 在 TSource 中搜索不到对应字段时，b.Value 使用默认值
			if (sourceField == null)
			{
				// (a , b) =>
				// {
				//		b.Value = default;
				// }

				// b.Value = default;
				BinaryExpression assign = Expression.Assign(targetMember, Expression.Default(targetFieldType));
				// sourceParameter 实际上是多余的，完全用不到
				return assign;
			}
			MemberExpression sourceMember;
			Type sourceFieldType;
			{
				if (sourceField is FieldInfo fieldInfo)
				{
					sourceFieldType = fieldInfo.FieldType;
					sourceMember = Expression.Field(sourceParameter, fieldInfo);
				}
				else if (sourceField is PropertyInfo propertyInfo)
				{
					sourceFieldType = propertyInfo.PropertyType;
					sourceMember = Expression.Property(sourceParameter, propertyInfo);
				}
				else
				{
					throw new InvalidCastException(
						$"框架处理出错，请提交 Issue！ {typeof(TSource).Name}.{sourceField.Name} 既不是字段也不是属性");
				}
			}

			// 如果搜索到 a 也存在相同的字段

			var sourceFieldTypeCode = TypeInfo.GetTypeCode(sourceFieldType);
			var targetFieldTypeCode = TypeInfo.GetTypeCode(targetFieldType);

			// 如果两个都是对象，则走嵌套映射
			if (sourceFieldTypeCode == TypeCode.Object && targetFieldTypeCode == TypeCode.Object)
			{
				// 两个都是数组
				if (targetFieldType.IsArray)
				{
					if (sourceFieldType == targetFieldType)
					{
						BinaryExpression assign = Expression.Assign(targetMember, sourceMember);
						return assign;
					}
				}
				// 相同类型，直接赋值
				else if ((sourceFieldType == targetFieldType) && mapOption.IsObjectReference)
				{
					BinaryExpression assign = Expression.Assign(targetMember, sourceMember);
					return assign;
				}
				// 目标字段为集合类型
				else if (targetFieldType.IsAssignableTo(typeof(IEnumerable)))
				{
					// 两个都是集合类型
					if (sourceFieldType.IsAssignableTo(typeof(IEnumerable)))
					{
						// b.Value = 转换集合(a.Value);
						var isType = sourceFieldType.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>))!.GenericTypeArguments[0]!;
						var convertDel = ConvertCollectionHelper.GetExpression(isType, targetFieldType);
						var convertMethodInfo = typeof(ConvertCollectionHelper<,>).MakeGenericType(isType, targetFieldType).GetMethod("ConvertCollection");
						if (convertMethodInfo != null)
						{
							var call = Expression.Call(null, convertMethodInfo, sourceMember);
							BinaryExpression assign = Expression.Assign(targetMember, call);
							return assign;
						}
					}
				}
				// 嵌套赋值
				else
				{
					// (a , b) =>
					// {
					//      b.Value = new();
					//		Mapper.Map<Field1,Field2>(a.Value , b.Value);
					// }
					// b.Value = new(); 
					var newExpression = Expression.New(targetFieldType);
					BinaryExpression assign = Expression.Assign(targetMember, newExpression);

					// Mapper.Map<Field1,Field2>(a.Value , b.Value);
					var mapMethodInfo = MapMethodInfo.MakeGenericMethod(sourceFieldType, targetFieldType);
					var call = Expression.Call(Expression.Constant(this), mapMethodInfo, sourceMember, targetMember);
					var black = Expression.Block(assign, call);
					return black;
				}
			}
			// 两个都是相同的类型，直接赋值
			else if (sourceFieldTypeCode == targetFieldTypeCode)
			{
				// (a,b) =>
				// {
				//		b.Value == a.Value
				// }
				BinaryExpression assign = Expression.Assign(targetMember, sourceMember);
				return assign;
			}
			else if (targetFieldTypeCode == TypeCode.DateTime)
			{
				if (mapOption.ConvertDateTime != null)
				{
					var call = Expression.Call(Expression.Constant(mapOption.ConvertDateTime.Target), mapOption.ConvertDateTime.Method, sourceMember);
					BinaryExpression assign = Expression.Assign(targetMember, call);
					return assign;
				}
			}
			else if (targetFieldTypeCode == TypeCode.String)
			{
				var toStringMethodInfo = sourceFieldType.GetMethod("ToString", Type.EmptyTypes)!;
				var call = Expression.Call(sourceMember, toStringMethodInfo);
				BinaryExpression assign = Expression.Assign(targetMember, call);
				return assign;
			}
			// 如果两者属于不同的类型，则自动转换类型，但是只支持 struct 类型，不包括 DateTime 类型
			else
			{
				// (a , b) =>
				// {
				//		b. Value = Mapper.AS<TSource,TTarget>(a.Value);
				// }

				// var value = Mapper.AS<TSource,TTarget>(a.Value);
				var asMethodInfo = ASMethodInfo.MakeGenericMethod(sourceFieldType, targetFieldType);
				var call = Expression.Call(null, asMethodInfo, sourceMember);

				// b.Value = value;
				BinaryExpression assign = Expression.Assign(targetMember, call);
				return assign;
			}
			throw new InvalidCastException(
	$"自动创建规则出错： $({targetFieldType.Name}){typeof(TTarget).Name}.{targetField.Name} = $({sourceFieldType.Name}){typeof(TSource).Name}.{sourceField.Name}");
		}

		#region 改版之后，这两个用不到了

		/// <summary>
		/// 封装委托，将委托返回的值赋予 b.Value
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="sourceParameter"></param>
		/// <param name="targetParameter"></param>
		/// <param name="memberInfo">b.Value 成员</param>
		/// <param name="delegate"> a=> a.Value... 委托</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		internal static Expression BuildAssign<TSource, TTarget>(ParameterExpression sourceParameter, ParameterExpression targetParameter, MemberInfo memberInfo, Delegate @delegate)
		{
			// b.Value
			MemberExpression targetMember;
			if (memberInfo is FieldInfo field)
			{
				targetMember = Expression.Field(targetParameter, field);
			}
			else if (memberInfo is PropertyInfo property)
			{
				targetMember = Expression.Property(targetParameter, property);
			}
			else
			{
				throw new InvalidCastException($"{memberInfo.DeclaringType?.Name}.{memberInfo.Name} 不是字段或属性");
			}

			// 调用用户自定义委托
			var instance = Expression.Constant(@delegate.Target);
			MethodCallExpression delegateCall = Expression.Call(instance, @delegate.Method, sourceParameter);
			// b.Value = @delegate.DynamicInvoke(a);
			BinaryExpression assign = Expression.Assign(targetMember, delegateCall);
			return assign;
		}


		/// <summary>
		/// 设置默认值
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="memberInfo"></param>
		/// <returns></returns>
		internal static Expression SetDefaultValue<TTarget>(MemberInfo memberInfo)
		{
			BinaryExpression assign;
			// TTarget b;
			ParameterExpression sourceParameter = Expression.Parameter(typeof(object), "a");
			ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "b");

			// b.Value
			MemberExpression targetMember;
			if (memberInfo is FieldInfo field)
			{
				targetMember = Expression.Field(targetParameter, field);
				assign = Expression.Assign(targetMember, Expression.Default(field.FieldType));
			}
			else if (memberInfo is PropertyInfo property)
			{
				targetMember = Expression.Property(targetParameter, property);
				assign = Expression.Assign(targetMember, Expression.Default(property.PropertyType));
			}
			else
			{
				throw new InvalidCastException($"{memberInfo.DeclaringType?.Name}.{memberInfo.Name} 不是字段或属性");
			}
			// sourceParameter 实际上是多余的，完全用不到
			return assign;
		}

		#endregion
	}
}
