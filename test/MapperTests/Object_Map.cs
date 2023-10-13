using Maomi.Mapper;
namespace MapperTests;

public class Object_Map
{
    public class TestValueA
    {
        public string ValueA { get; set; } = "A";

        public string ValueB { get; set; } = "B";

        public string ValueC { get; set; } = "C";
    }

    public class TestValueB
    {
        public string ValueA { get; set; }

        public string ValueB { get; set; }

        public string ValueC { get; set; }
    }


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

    public class TestC
    {
        public string ValueA { get; set; }

        public string ValueB { get; set; }

        public string ValueC { get; set; }
        public TestValueB Value { get; set; }
    }

    // 嵌套对象引用
    [Fact]
    public void Object_Map_Ref()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestB>().Build();

        var a = new TestA()
        {
            Value = new TestValueA()
        };
        var b = mapper.Map<TestA, TestB>(a);

        Assert.Equal(a.Value, b.Value);
        Assert.True(a.Value == b.Value);
    }
    // 嵌套对象嵌套映射
    public void Object_Map_NotRef()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestB>(option =>
        {
            option.IsObjectReference = false;
        }).Build();

        var a = new TestA()
        {
            Value = new TestValueA()
        };
        var b = mapper.Map<TestA, TestB>(a);

        Assert.NotEqual(a.Value, b.Value);
        Assert.False(a.Value == b.Value);
        Assert.Equal("A", b.ValueA);
        Assert.Equal("B", b.ValueB);
        Assert.Equal("C", b.ValueC);
    }


    // 嵌套对象
    [Fact]
    public void Object_Map_AnyType()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestC>().Build();

        var a = new TestA()
        {
            Value = new TestValueA()
        };
        var b = mapper.Map<TestA, TestC>(a);

        Assert.Equal("A", b.ValueA);
        Assert.Equal("B", b.ValueB);
        Assert.Equal("C", b.ValueC);
    }
}
