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
                    return new DateTime(str);
                throw new Exception("未能转换为时间");
            };
        }).Build();
        var date = DateTime.Now;
        var a = mapper.Map<TestA, TestB>(new TestA()
        {
            Value = date.ToString()
        });

        Assert.Equal(date, a.Value);
    }

    [Fact]
    public void AS_Datetime_Ex()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestB>().Build();
        var date = DateTime.Now;
        Assert.Throws<InvalidCastException>(() =>
        {
            mapper.Map<TestA, TestB>(new TestA()
            {
                Value = date.ToString()
            });
        });
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

        Assert.Equal(date.ToString(), a.Value);
    }
}