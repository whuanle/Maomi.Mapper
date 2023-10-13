namespace Maomi.Mapper
{
	/// <summary>
	/// 映射配置
	/// </summary>
	public abstract class IMapper
	{
		/// <summary>
		/// 允许被全局静态 <see cref="MaomiMapper"/> 使用
		/// </summary>
		public bool IsBindGlobal { get; protected set; }

		/// <summary>
		/// 配置绑定
		/// </summary>
		/// <param name="mapper"></param>
		public abstract void Bind(MaomiMapper mapper);
	}
}
