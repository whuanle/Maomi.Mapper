using Maomi.Mapper;

namespace MapperTests;

public class Manual_AS_DateTime_String_Test
{
	public class TestA
	{
		public string Value { get; set; }
	}
	public class TestB
	{
		public DateTime Value { get; set; }
	}

	[Fact]
	public void AS_Datetime()
	{
		var mapper = new MaomiMapper();
		mapper.Bind<TestA, TestB>(option =>
		{
			option.ConvertDateTime = value =>
			{
				if (value is string str)
					return DateTime.Parse(str);
				throw new Exception("未能转换为时间");
			};
		}).Build();
		var date = DateTime.Now;
		var a = mapper.Map<TestA, TestB>(new TestA()
		{
			Value = date.ToString()
		});

		Assert.Equal(date.ToString("yyyy/MM/dd HH:mm:ss"), a.Value.ToString("yyyy/MM/dd HH:mm:ss"));
	}

	[Fact]
	public void AS_Datetime_Ex()
	{
		var mapper = new MaomiMapper();

		var ex = Assert.Throws<InvalidCastException>(() =>
		{
			mapper.Bind<TestA, TestB>().Build();
		});
		Assert.Equal("自动创建规则出错： $(DateTime)TestB.Value = $(String)TestA.Value",ex.Message);

	}


	[Fact]
	public void DateTime_AS_String()
	{
		var mapper = new MaomiMapper();
		mapper.Bind<TestB, TestA>().Build();
		var date = DateTime.Now;
		var a = mapper.Map<TestB, TestA>(new TestB()
		{
			Value = date
		});

		Assert.Equal(date.ToString("yyyy/MM/dd HH:mm:ss"), a.Value);
	}
}
