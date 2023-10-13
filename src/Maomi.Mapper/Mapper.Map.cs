using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Maomi.Mapper
{
	public partial class MaomiMapper
	{
		/// <summary>
		/// <see cref="MaomiMapper.Map{TSource, TTarget}(TSource, TTarget)"/>
		/// </summary>
		private static readonly MethodInfo MapMethodInfo;

		/// <summary>
		/// 映射字段，构造：<br />
		/// (a , b) => <br />
		/// {
		///  ... ...
		/// }
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <param name="targetField">b.Value 字段</param>
		/// <param name="mapOption">映射配置</param>
		/// <returns></returns>
		internal static Delegate MapField<TSource, TTarget>(FieldInfo targetField, MapOption mapOption)
		{
			// TSource a;
			// TTarget b;
			ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "a");
			ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "b");

			// b.Value
			MemberExpression targetMember = Expression.Field(targetParameter, targetField);
			var sourceField = typeof(TSource).GetField(targetField.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// 在 TSource 中搜索不到对应字段时，b.Value 使用默认值
			if (sourceField == null)
			{
				// (a , b) =>
				// {
				//		b.Value = default;
				// }

				// b.Value = default;
				BinaryExpression assign = Expression.Assign(targetMember, Expression.Default(targetField.FieldType));
				// sourceParameter 实际上是多余的，完全用不到
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}

			// 如果搜索到 a 也存在相同的字段

			var sourceFieldTypeCode = TypeInfo.GetTypeCode(sourceField.FieldType);
			var targetFieldTypeCode = TypeInfo.GetTypeCode(targetField.FieldType);
			MemberExpression sourceMember = Expression.Field(sourceParameter, sourceField);

			// 如果两个都是对象，则走嵌套映射
			if (sourceFieldTypeCode == TypeCode.Object && targetFieldTypeCode == TypeCode.Object)
			{
				// 相同类型，直接赋值
				if ((sourceField.FieldType == targetField.FieldType) && mapOption.IsObjectReference)
				{
					BinaryExpression assign = Expression.Assign(targetMember, sourceMember);
					return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
				}
				// 嵌套赋值
				else
				{
					// (a , b) =>
					// {
					//		Mapper.Map<Field1,Field2>(a.Value , b.Value);
					// }
					var asMethodInfo = MapMethodInfo.MakeGenericMethod(sourceField.FieldType, targetField.FieldType);
					// Mapper.Map<Field1,Field2>(a.Value , b.Value);
					var call = Expression.Call(Expression.Constant(Instance), asMethodInfo, sourceMember, targetMember);
					Delegate assignDelegate = Expression.Lambda(call, sourceParameter, targetParameter).Compile();
					return assignDelegate;
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
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}
			else if (targetFieldTypeCode == TypeCode.DateTime)
			{
				if (mapOption.ConvertDateTime != null)
				{
					var call = Expression.Call(Expression.Constant(mapOption.ConvertDateTime.Target), mapOption.ConvertDateTime.Method, sourceMember);
					BinaryExpression assign = Expression.Assign(targetMember, call);
					return Expression.Lambda(call, sourceParameter, targetParameter).Compile();
				}
				throw new InvalidCastException(
					$"自动创建规则出错： $({targetField.FieldType.Name}){typeof(TTarget).Name}.{targetField.Name} = $({sourceField.FieldType.Name}{typeof(TSource).Name}.{sourceField.Name}");
			}
			else if (sourceFieldTypeCode == TypeCode.DateTime)
			{
				throw new InvalidCastException(
					$"自动创建规则出错： $({targetField.FieldType.Name}){typeof(TTarget).Name}.{targetField.Name} = $({sourceField.FieldType.Name}{typeof(TSource).Name}.{sourceField.Name}");
			}
			// 如果两者属于不同的类型，则自动转换类型，但是只支持 struct 类型，不包括 DateTime 类型
			else
			{
				// (a , b) =>
				// {
				//		b. Value = Mapper.AS<TSource,TTarget>(a.Value);
				// }

				// var value = Mapper.AS<TSource,TTarget>(a.Value);
				var asMethodInfo = ASMethodInfo.MakeGenericMethod(sourceField.FieldType, targetField.FieldType);
				var call = Expression.Call(Expression.Constant(Instance), asMethodInfo, sourceMember);

				// b.Value = value;
				BinaryExpression assign = Expression.Assign(targetMember, call);
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}
		}

		/// <summary>
		/// 映射属性，构造：<br />
		/// (a , b) => <br />
		/// {
		///  ... ...
		/// }
		/// </summary>
		/// <typeparam name="TSource">源类型</typeparam>
		/// <typeparam name="TTarget">目标类型</typeparam>
		/// <param name="targetField">目标属性 b.Value</param>
		/// <param name="mapOption">映射配置</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		internal static Delegate MapProperty<TSource, TTarget>(PropertyInfo targetField, MapOption mapOption)
		{
			// TSource a;
			// TTarget b;
			ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "a");
			ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "b");

			// b.Value
			MemberExpression targetMember = Expression.Property(targetParameter, targetField);
			var sourceField = typeof(TSource).GetProperty(targetField.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// 在 TSource 中搜索不到对应字段时，b.Value 使用默认值
			if (sourceField == null)
			{
				// (a , b) =>
				// {
				//		b.Value = default;
				// }

				// b.Value = default;
				BinaryExpression assign = Expression.Assign(targetMember, Expression.Default(targetField.PropertyType));
				// sourceParameter 实际上是多余的，完全用不到
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}

			// 如果搜索到 a 也存在相同的字段

			var sourceFieldTypeCode = TypeInfo.GetTypeCode(sourceField.PropertyType);
			var targetFieldTypeCode = TypeInfo.GetTypeCode(targetField.PropertyType);
			MemberExpression sourceMember = Expression.Property(sourceParameter, sourceField);

			// 如果两个都是对象，则走嵌套映射
			if (sourceFieldTypeCode == TypeCode.Object && targetFieldTypeCode == TypeCode.Object)
			{
				// 相同类型，直接赋值
				if ((sourceField.PropertyType == targetField.PropertyType) && mapOption.IsObjectReference)
				{
					BinaryExpression assign = Expression.Assign(targetMember, sourceMember);
					return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
				}
				// 嵌套赋值
				else
				{
					// (a , b) =>
					// {
					//		Mapper.Map<Field1,Field2>(a.Value , b.Value);
					// }
					var asMethodInfo = MapMethodInfo.MakeGenericMethod(sourceField.PropertyType, targetField.PropertyType);
					// Mapper.Map<Field1,Field2>(a.Value , b.Value);
					var call = Expression.Call(Expression.Constant(Instance), asMethodInfo, sourceMember, targetMember);
					Delegate assignDelegate = Expression.Lambda(call, sourceParameter, targetParameter).Compile();
					return assignDelegate;
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
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}
			else if (targetFieldTypeCode == TypeCode.DateTime)
			{
				if (mapOption.ConvertDateTime != null)
				{
					var call = Expression.Call(Expression.Constant(mapOption.ConvertDateTime.Target), mapOption.ConvertDateTime.Method, sourceMember);
					BinaryExpression assign = Expression.Assign(targetMember, call);
					return Expression.Lambda(call, sourceParameter, targetParameter).Compile();
				}
				throw new InvalidCastException(
					$"自动创建规则出错： $({targetField.PropertyType.Name}){typeof(TTarget).Name}.{targetField.Name} = $({sourceField.PropertyType.Name}{typeof(TSource).Name}.{sourceField.Name}");
			}
			else if (sourceFieldTypeCode == TypeCode.DateTime)
			{
				throw new InvalidCastException(
					$"自动创建规则出错： $({targetField.PropertyType.Name}){typeof(TTarget).Name}.{targetField.Name} = $({sourceField.PropertyType.Name}{typeof(TSource).Name}.{sourceField.Name}");
			}
			// 如果两者属于不同的类型，则自动转换类型，但是只支持 struct 类型，不包括 DateTime 类型
			else
			{
				// (a , b) =>
				// {
				//		b. Value = Mapper.AS<TSource,TTarget>(a.Value);
				// }

				// var value = Mapper.AS<TSource,TTarget>(a.Value);
				var asMethodInfo = ASMethodInfo.MakeGenericMethod(sourceField.PropertyType, targetField.PropertyType);
				var call = Expression.Call(Expression.Constant(Instance), asMethodInfo, sourceMember);

				// b.Value = value;
				BinaryExpression assign = Expression.Assign(targetMember, call);
				return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
			}
		}

		/// <summary>
		/// 封装委托，将委托返回的值赋予 b.Value
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="memberInfo">b.Value 成员</param>
		/// <param name="delegate"> a=> a.Value... 委托</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		internal static Delegate BuildAssign<TSource, TTarget>(MemberInfo memberInfo, Delegate @delegate)
		{
			// TSource a;
			// TTarget b;
			ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "a");
			ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "b");

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
			return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
		}
		/// <summary>
		/// 设置默认值
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="memberInfo"></param>
		/// <returns></returns>
		internal static Delegate SetDefaultValue<TTarget>(MemberInfo memberInfo)
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
			return Expression.Lambda(assign, sourceParameter, targetParameter).Compile();
		}
	}
}
