using Maomi.Mapper;
namespace MapperTests;

// 各种规则映射
public class RuleMap
{
    public class TestA
    {
        public string ValueA { get; set; }
    }
    public class TestB
    {
        public string ValueB { get; set; }
    }

    public class TestC
    {
        private string Value { get; set; }
        public string ValueC => this.Value;
    }

    [Fact]
    public void StringName()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestB>()
        .Map(a => a.ValueA, b => b.ValueB).Build();

        var a = new TestA()
        {
            ValueA = "A"
        };

        var b = mapper.Map<TestA, TestB>(a);
        Assert.Equal("A", b.ValueB);


        var mapper2 = new MaomiMapper();
        mapper2.Bind<TestA, TestB>()
        .Map(a => a.ValueA, b => "ValueB").Build();
        var b2 = mapper2.Map<TestA, TestB>(a);

        Assert.Equal("A", b2.ValueB);
    }

    [Fact]
    public void StringName_Private()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestC>()
        .Map(a => a.ValueA, b => "Value").Build();

        var a = new TestA()
        {
            ValueA = "A"
        };

        var c = mapper.Map<TestA, TestC>(a);
        Assert.Equal("A", c.ValueC);
    }


    public class TestD
    {
        public string Value;
    }

    [Fact]
    public void StringName_Field_To_Property()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestD>()
        .Map(a => a.ValueA, b => b.Value).Build();

        var a = new TestA()
        {
            ValueA = "A"
        };

        var d = mapper.Map<TestA, TestD>(a);
        Assert.Equal("A", d.Value);
    }

    [Fact]
    public void StringName_Property_To_Field()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestD, TestA>()
        .Map(a => a.Value, b => b.ValueA).Build();

        var d = new TestD()
        {
            Value = "A"
        };

        var a = mapper.Map<TestD, TestA>(d);
        Assert.Equal("A", a.ValueA);
    }

    [Fact]
    public void ValueRule()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestD>()
        .Map(a => a.ValueA + "B", b => b.Value).Build();

        var a = new TestA()
        {
            ValueA = "A"
        };

        var d = mapper.Map<TestA, TestD>(a);
        Assert.Equal("AB", d.Value);
    }
}
