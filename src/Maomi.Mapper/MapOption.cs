using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Maomi.Mapper
{
	/// <summary>
	/// 映射配置
	/// </summary>
	public class MapOption
	{
		/// <summary>
		/// 包括私有字段
		/// </summary>
		public bool IncludePrivateField { get; set; } = false;

		/// <summary>
		/// 自动映射，如果有字段/属性没有配置映射规则，则自动映射
		/// </summary>
		public bool AutoMap { get; set; } = true;

		/// <summary>
		/// 如果属性字段是对象且为相同类型，则保持引用。 <br />
		/// 如果设置为 false，则会创建新的对象，再对字段逐个处理。
		/// </summary>
		public bool IsObjectReference { get; set; } = true;

		/// <summary>
		/// 配置时间转换器。<br />
		///  如果 b.Value 是 DateTime，而 a.Value 不是 DateTime，则需要配置转换器，否则会报错。
		/// </summary>
		/// <value></value>
		public Func<object, DateTime>? ConvertDateTime { get; set; }
	}
}
