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
		/// <see cref="UInt64"/>、
		/// <see cref="Single"/>、
		/// <see cref="Double"/>、
		/// <see cref="Decimal"/>、
		/// <see cref="Char"/> 。<br />
		/// 不支持 <see cref="DateTime"/> 。<br />
		/// 注意，浮点型转非浮点型，会出乎意料之外。
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
			var c1 = TypeInfo.GetTypeCode(typeof(TSourceValue));
			var c2 = TypeInfo.GetTypeCode(typeof(TTargetValue));

			if (c1 == TypeCode.DateTime || c2 == TypeCode.DateTime)
			{
				throw new InvalidCastException(
					$"不支持该类型字段的转换： {typeof(TSourceValue).Name}  => {typeof(TTargetValue).Name}");
			}

			if (c1 == c2) return Unsafe.As<TSourceValue, TTargetValue>(ref sourceValue);

			switch (c2)
			{
				case TypeCode.Boolean:
					var v1 = Convert.ToBoolean(sourceValue);
					return Unsafe.As<Boolean, TTargetValue>(ref v1);
				case TypeCode.SByte:
					var v2 = Convert.ToSByte(sourceValue);
					return Unsafe.As<SByte, TTargetValue>(ref v2);
				case TypeCode.Byte:
					var v3 = Convert.ToByte(sourceValue);
					return Unsafe.As<Byte, TTargetValue>(ref v3);
				case TypeCode.Int16:
					var v4 = Convert.ToInt16(sourceValue);
					return Unsafe.As<Int16, TTargetValue>(ref v4);
				case TypeCode.UInt16:
					var v5 = Convert.ToUInt16(sourceValue);
					return Unsafe.As<UInt16, TTargetValue>(ref v5);
				case TypeCode.Int32:
					var v6 = Convert.ToInt32(sourceValue);
					return Unsafe.As<Int32, TTargetValue>(ref v6);
				case TypeCode.UInt32:
					var v7 = Convert.ToUInt32(sourceValue);
					return Unsafe.As<UInt32, TTargetValue>(ref v7);
				case TypeCode.Int64:
					var v8 = Convert.ToInt64(sourceValue);
					return Unsafe.As<Int64, TTargetValue>(ref v8);
				case TypeCode.UInt64:
					var v9 = Convert.ToUInt64(sourceValue);
					return Unsafe.As<UInt64, TTargetValue>(ref v9);
				case TypeCode.Single:
					var v10 = Convert.ToSingle(sourceValue);
					return Unsafe.As<Single, TTargetValue>(ref v10);
				case TypeCode.Double:
					var v11 = Convert.ToDouble(sourceValue);
					return Unsafe.As<Double, TTargetValue>(ref v11);
				case TypeCode.Decimal:
					var v12 = Convert.ToDecimal(sourceValue);
					return Unsafe.As<Decimal, TTargetValue>(ref v12);
				case TypeCode.Char:
					char v13 = Convert.ToChar(Convert.ToUInt16(sourceValue));
					return Unsafe.As<Char, TTargetValue>(ref v13);
			}

			throw new InvalidCastException(
				$"不支持该类型字段的转换： {typeof(TSourceValue).Name}  => {typeof(TTargetValue).Name}");
		}
	}
}
