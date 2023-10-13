using Maomi.Mapper;

namespace Demo.Console
{

	public class A
	{
		public string V { get; set; }
	}
	public class B
	{
		public string V { get; set; }
		public string V1 { get; set; }
		private string V2 { get; set; }
		public string V3 { get; set; }
	}

	// 自动映射到 D，以及反向绑定 D => C
	[Map(typeof(D), IsReverse = true)]
	public class C
	{
		public string V { get; set; }
	}

	public class D
	{
		public string V { get; set; }
	}

	/// <summary>
	/// 自定义映射配置
	/// </summary>
	public class MyMapOptionAttribute : MapOptionAttribute
	{
		public override Action<MapOption> MapOption => option =>
		{
			option.AutoMap = true;
			option.IsObjectReference = false;
		};
	}

	// 自动映射
	[Map(typeof(F), IsReverse = true)]
	// 映射时，使用自定义配置
	[MyMapOption]
	public class E
	{
		public string V { get; set; }
	}

	public class F
	{
		public string V { get; set; }
	}

	public class MyMapper : IMapper
	{
		public override void Bind(Mapper mapper)
		{
			// 手动绑定映射
		}
	}

	public class Test
	{

		public static void Main()
		{
			Mapper.BindTo<A, B>(option =>
			{
				option.AutoMap = true;
				option.IsObjectReference = false;
			})
				// b.V = a.V + "a"
				.Map(a => 1, b => b.V)
				// 忽略 V1
				.Ignore(x => x.V1)
				// b.V2 = a.V
				.Map(a => a.V, b => "V2")
				// b.V3 = "666";
				.Map(a => "666", b => "V3")
				.Build();

			// 扫描程序集
			Mapper.Static.Scan();

			var b = Mapper.MapTo<A, B>(new A()
			{
				V = "1"
			});
		}
	}
}
