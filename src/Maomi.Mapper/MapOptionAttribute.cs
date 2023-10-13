using System;

namespace Maomi.Mapper
{
    /// <summary>
    /// 映射配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public abstract class MapOptionAttribute : Attribute
	{
		/// <summary>
		/// 映射配置
		/// </summary>
		public abstract Action<MapOption> MapOption { get; }
	}
}
