
// 程序集扫描
using Maomi.Mapper;

namespace MapperTests;

public class ScanTest
{
	// 特性
	// 特性 + 配置
	// 自定义 IMapper 接口


	public class TestValueA
	{
		public string ValueA { get; set; } = "A";

		public string ValueB { get; set; } = "B";

		public string ValueC { get; set; } = "C";
	}

	[Map(typeof(TestValueA), IsReverse = true)]
	public class TestValueB
	{
		public string ValueA { get; set; }

		public string ValueB { get; set; }

		public string ValueC { get; set; }
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MyMapOptionAttribute : MapOptionAttribute
	{
		public override Action<MapOption> MapOption => _option;
		private Action<MapOption> _option;
		public MyMapOptionAttribute()
		{
			_option = option =>
			{
				option.IsObjectReference = false;
			};
		}
	}

	[MyMapOption]
	[Map(typeof(TestB), IsReverse = true)]
	public class TestA
	{
		public string ValueA { get; set; } = "A";

		public string ValueB { get; set; } = "B";

		public string ValueC { get; set; } = "C";
		public TestValueA Value { get; set; }
	}

	public class TestB
	{
		public string ValueA { get; set; }

		public string ValueB { get; set; }

		public string ValueC { get; set; }
		public TestValueA Value { get; set; }
	}


	public class MyMapper : IMapper
	{
		public override void Bind(MaomiMapper mapper)
		{
			mapper.Bind<TestA, TestC>(option => option.IsObjectReference = false).Build();
			mapper.Bind<TestA, TestD>(option => option.IsObjectReference = false).Build();
		}
	}

	public class TestC
	{
		public string ValueA { get; set; }

		public string ValueB { get; set; }

		public string ValueC { get; set; }
		public TestValueB Value { get; set; }
	}


	public class TestD
	{
		public string ValueA { get; set; }

		public string ValueB { get; set; }

		public string ValueC { get; set; }
		public TestValueA Value { get; set; }
	}

	// 嵌套对象引用
	[Fact]
	public void Object_Map_Ref()
	{
		var mapper = new MaomiMapper();
		mapper.Scan();

		var a = new TestB()
		{
			Value = new TestValueA()
		};
		var b = mapper.Map<TestB, TestD>(a);

		Assert.True(object.ReferenceEquals(a.Value, b.Value));
		Assert.True(a.Value == b.Value);

		Assert.Equal("A", b.Value.ValueA);
		Assert.Equal("B", b.Value.ValueB);
		Assert.Equal("C", b.Value.ValueC);
	}

	// 嵌套对象嵌套映射
	[Fact]
	public void Object_Map_NotRef()
	{
		var mapper = new MaomiMapper();
		mapper.Scan();

		var a = new TestA()
		{
			Value = new TestValueA()
		};
		var b = mapper.Map<TestA, TestB>(a);

		Assert.False(object.ReferenceEquals(a.Value, b.Value));
		Assert.False(a.Value == b.Value);
		Assert.Equal("A", b.ValueA);
		Assert.Equal("B", b.ValueB);
		Assert.Equal("C", b.ValueC);

		Assert.Equal("A", b.Value.ValueA);
		Assert.Equal("B", b.Value.ValueB);
		Assert.Equal("C", b.Value.ValueC);
	}


	// 嵌套对象
	[Fact]
	public void Object_Map_AnyType()
	{
		var mapper = new MaomiMapper();
		mapper.Scan();

		var a = new TestA()
		{
			Value = new TestValueA()
		};
		var b = mapper.Map<TestA, TestC>(a);

		Assert.Equal("A", b.ValueA);
		Assert.Equal("B", b.ValueB);
		Assert.Equal("C", b.ValueC);

		Assert.Equal("A", b.Value.ValueA);
		Assert.Equal("B", b.Value.ValueB);
		Assert.Equal("C", b.Value.ValueC);
	}
}