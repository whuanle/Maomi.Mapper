using System;

namespace Maomi.Mapper
{
	/// <summary>
	/// 记录映射绑定数据
	/// </summary>
	internal class MapperData : IEquatable<MapperData>
	{
		public MapperData(MapperBuilder mapperBuilder, MapInfo mapInfo)
		{
			MapperBuilder = mapperBuilder;
			MapInfo = mapInfo;
		}

		/// <summary>
		/// 是否已经构建整个类型的映射
		/// </summary>
		public bool IsBuild { get; private set; } = false;

		/// <summary>
		/// 映射构建器
		/// </summary>
		public MapperBuilder MapperBuilder { get; private set; }

		/// <summary>
		/// 映射信息
		/// </summary>
		public MapInfo MapInfo { get; private set; }

		/// <summary>
		/// 构建整个类型的映射
		/// </summary>
		public void Build()
		{
			if (IsBuild == true) return;
			IsBuild = true;
			MapperBuilder.Build();
		}

		public bool Equals(MapperData? other)
		{
			if (other == null) return false;
			return other.MapInfo.Equals(other.MapInfo);
		}

		public override int GetHashCode()
		{
			return MapInfo.GetHashCode();
		}

		public override bool Equals(object? obj)
		{
			if (obj is MapperData info)
			{
				return this.MapInfo.Equals(info.MapInfo);
			}
			return false;
		}
	}
}
