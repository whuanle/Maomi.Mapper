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

	/// <summary>
	/// 映射绑定
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MapAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public Type[] Types { get; private set; }

		/// <summary>
		/// 是否允许反向绑定
		/// </summary>
		public bool IsReverse { get; set; }

		/// <summary>
		/// 该类型可以转换为何种类型
		/// </summary>
		/// <param name="types"> => t1,t2</param>
		public MapAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}
