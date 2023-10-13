using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Linq.Expressions;

namespace Maomi.Mapper
{
	// 类型转换
	public partial class MaomiMapper
	{
		/// <summary>
		/// <see cref="MaomiMapper.AS{TSourceValue, TTargetValue}(TSourceValue)"/>
		/// </summary>
		private static readonly MethodInfo ASMethodInfo;


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
		/// <see cref="Char"/> 。<br />
		/// 不支持 <see cref="DateTime"/> 。<br />
		/// 注意，大类型转小类型等情况，转换时可能会出现精确度丢失。
		/// </summary>
		/// <typeparam name="TSourceValue">源值类型</typeparam>
		/// <typeparam name="TTargetValue">转换后值类型</typeparam>
		/// <param name="sourceValue">源值</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException">不支持的类型</exception>
		public static TTargetValue AS<TSourceValue, TTargetValue>(TSourceValue sourceValue)
			where TSourceValue : struct
			where TTargetValue : struct
		{
			if (typeof(TSourceValue) == typeof(TTargetValue))
				return Unsafe.As<TSourceValue, TTargetValue>(ref sourceValue);

			var c1 = TypeInfo.GetTypeCode(typeof(TSourceValue));
			var c2 = TypeInfo.GetTypeCode(typeof(TTargetValue));

			if (c1 == TypeCode.DateTime || c2 == TypeCode.DateTime)
			{
				throw new InvalidCastException(
					$"不支持该类型字段的转换： {typeof(TSourceValue).Name}  => {typeof(TTargetValue).Name}");
			}

			switch (c1)
			{
				case TypeCode.Boolean:
					var v1 = Unsafe.As<TSourceValue, Boolean>(ref sourceValue);
					return Unsafe.As<Boolean, TTargetValue>(ref v1);
				case TypeCode.SByte:
					var v2 = Unsafe.As<TSourceValue, SByte>(ref sourceValue);
					return Unsafe.As<SByte, TTargetValue>(ref v2);
				case TypeCode.Byte:
					var v3 = Unsafe.As<TSourceValue, Byte>(ref sourceValue);
					return Unsafe.As<Byte, TTargetValue>(ref v3);
				case TypeCode.Int16:
					var v4 = Unsafe.As<TSourceValue, Int16>(ref sourceValue);
					return Unsafe.As<Int16, TTargetValue>(ref v4);
				case TypeCode.UInt16:
					var v5 = Unsafe.As<TSourceValue, UInt16>(ref sourceValue);
					return Unsafe.As<UInt16, TTargetValue>(ref v5);
				case TypeCode.Int32:
					var v6 = Unsafe.As<TSourceValue, Int32>(ref sourceValue);
					return Unsafe.As<Int32, TTargetValue>(ref v6);
				case TypeCode.UInt32:
					var v7 = Unsafe.As<TSourceValue, UInt32>(ref sourceValue);
					return Unsafe.As<UInt32, TTargetValue>(ref v7);
				case TypeCode.Int64:
					var v8 = Unsafe.As<TSourceValue, Int64>(ref sourceValue);
					return Unsafe.As<Int64, TTargetValue>(ref v8);
				case TypeCode.UInt64:
					var v9 = Unsafe.As<TSourceValue, UInt64>(ref sourceValue);
					return Unsafe.As<UInt64, TTargetValue>(ref v9);
				case TypeCode.Single:
					var v10 = Unsafe.As<TSourceValue, Single>(ref sourceValue);
					return Unsafe.As<Single, TTargetValue>(ref v10);
				case TypeCode.Double:
					var v11 = Unsafe.As<TSourceValue, Double>(ref sourceValue);
					return Unsafe.As<Double, TTargetValue>(ref v11);
				case TypeCode.Decimal:
					var v12 = Unsafe.As<TSourceValue, Decimal>(ref sourceValue);
					return Unsafe.As<Decimal, TTargetValue>(ref v12);
				case TypeCode.Char:
					var v13 = Unsafe.As<TSourceValue, Char>(ref sourceValue);
					return Unsafe.As<Char, TTargetValue>(ref v13);
			}

			throw new InvalidCastException(
				$"不支持该类型字段的转换： {typeof(TSourceValue).Name}  => {typeof(TTargetValue).Name}");
		}

		/// <summary>
		/// 类型转换
		/// 值类型映射，支持以下类型互转：<br />
		/// <see cref="String"/> 、
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
		/// <see cref="Char"/> 。<br />
		/// 不支持 <see cref="DateTime"/> 。<br />
		/// 注意，大类型转小类型等情况，转换时可能会出现精确度丢失。
		/// </summary>
		/// <param name="sourceValue"></param>
		/// <typeparam name="TSourceValue"></typeparam>
		/// <typeparam name="TTargetValue"></typeparam>
		/// <returns></returns>
		public static TTargetValue ASPrimitive<TSourceValue, TTargetValue>(TSourceValue sourceValue)
		{
			return ASHelper<TSourceValue, TTargetValue>.Convert(sourceValue);
		}

		private static class ASHelper<TSourceValue, TTargetValue>
		{
			private static readonly MethodInfo ASMethodInfo;
			private delegate TTargetValue ASConverter(TSourceValue sourceValue);
			private static readonly ASConverter ASConvert;

			static ASHelper()
			{
				var ms = typeof(ASHelper<,>).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
				ASMethodInfo = ms.FirstOrDefault(x => x.IsGenericMethod && x.Name == "AS")!;

				if (typeof(TTargetValue) == typeof(string))
				{
					ASConvert = (TSourceValue sourceValue) =>
					{
						if (sourceValue == null) return default!;
						return (TTargetValue)(object)sourceValue.ToString()!;
					};
				}
				else if (typeof(TTargetValue).IsValueType && typeof(TSourceValue).IsValueType)
				{
					ParameterExpression sourceParameter = Expression.Parameter(typeof(TSourceValue), "a");
					var asMethodInfo = ASMethodInfo.MakeGenericMethod(typeof(TSourceValue), typeof(TTargetValue));
					var call = Expression.Call(null, asMethodInfo, sourceParameter);
					ASConvert = Expression.Lambda<ASConverter>(call, sourceParameter).Compile();
				}
				else
				{
					throw new InvalidCastException($"不支持 {typeof(TSourceValue).Name} => {typeof(TTargetValue).Name}");
				}
			}

			/// <summary>
			/// 转换为对应类型
			/// </summary>
			/// <param name="sourceValue"></param>
			/// <returns></returns>
			public static TTargetValue Convert(TSourceValue sourceValue)
			{
				return ASConvert(sourceValue);
			}
		}
	}
}
