using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Maomi.Mapper
{
	internal class FieldOption
	{
		public bool IsIgnore { get; init; }
		public Delegate? Delegate { get; init; }
	}

	/// <summary>
	/// 映射信息
	/// </summary>
	internal class MapInfo : IEquatable<MapInfo>
	{
		public MapInfo(Type source, Type target, MapOption mapOption)
		{
			Source = source ?? throw new ArgumentNullException(nameof(source));
			Target = target ?? throw new ArgumentNullException(nameof(target));
			MapOption = mapOption;

			_memberInfos = target
				.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(x => (x is FieldInfo) || (x is PropertyInfo)).ToArray();
		}

		/// <summary>
		/// 源对象，a =>
		/// </summary>
		public Type Source { get; private set; }

		/// <summary>
		/// 目标对象， => b
		/// </summary>
		public Type Target { get; private set; }

		/// <summary>
		/// 映射配置
		/// </summary>
		public MapOption MapOption { get; private set; }

		private readonly MemberInfo[] _memberInfos;

		/// <summary>
		/// <see cref="Target"/> 的字段和属性列表
		/// </summary>
		public MemberInfo[] MemberInfos => _memberInfos;

		private readonly Dictionary<MemberInfo, FieldOption> _binds = new();

		/// <summary>
		/// 字段或属性绑定的委托
		/// </summary>
		public Dictionary<MemberInfo, FieldOption> Binds => _binds;


		/// <summary>
		///  (a,b)=>{}
		/// </summary>
		/// <value></value>
		public Delegate? Delegate { get; set; }

		public bool Equals(MapInfo? other)
		{
			if (other == null) return false;
			return other.Source == Source && other.Target == Target;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Source, Target);
		}

		public override bool Equals(object? obj)
		{
			if (obj is MapInfo info)
			{
				return info.Source == Source && info.Target == Target;
			}
			return false;
		}
	}




}
