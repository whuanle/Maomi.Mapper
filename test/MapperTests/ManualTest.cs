using Maomi.Mapper;
using System.Reflection;

namespace MapperTests;

public class ManualTest
{
    public class TestA
    {
        public string ValueA { get; set; }
        public string ValueB;
    }
    public class TestB
    {
        public string ValueA { get; set; }
        public string ValueB;
    }

    // 字段数量和类型都一致
    [Fact]
    public void SameCount_SameType()
    {
        var build = MaomiMapper.BindTo<TestA, TestB>();
        build.Build();
        var b = MaomiMapper.MapTo<TestA, TestB>(new TestA
        {
            ValueA = "A",
            ValueB = "B"
        });
        Assert.Equal("A", b.ValueA);
        Assert.Equal("B", b.ValueB);

        var mapper = new MaomiMapper();
        build = mapper.Bind<TestA, TestB>();
        build.Build();
        b = mapper.Map<TestA, TestB>(new TestA
        {
            ValueA = "A",
            ValueB = "B"
        });
        Assert.Equal("A", b.ValueA);
        Assert.Equal("B", b.ValueB);
    }


    public class TestC
    {
        public string ValueA { get; set; }
        public string ValueB;
        private string ValueC { get; set; } = "C";
        private string ValueD = "D";
    }
    public class TestD
    {
        public string ValueA { get; set; }
        public string ValueB;
        private string ValueC { get; set; }
        private string ValueD;
    }

    public class TestDD
    {
        public string ValueA { get; set; }
        public string ValueB;
        public string ValueC { get; set; }
        public string ValueD;
    }


    // 字段数量和类型都一致
    [Fact]
    public void SameCount_SameType_Private()
    {
        var mapper = new MaomiMapper();
        var build = mapper.Bind<TestC, TestD>(
            option =>
            {
                option.IncludePrivate = true;
            }).Map(a => "111", b => "ValueC");
        build.Build();
        mapper.Bind<TestC, TestDD>().Build();

        var b = mapper.Map<TestC, TestD>(new TestC
        {
            ValueA = "A",
            ValueB = "B"
        });
        var c = mapper.Map<TestC, TestDD>(new TestC
        {
            ValueA = "A",
            ValueB = "B"
        });

        Assert.Equal("A", b.ValueA);
        Assert.Equal("B", b.ValueB);
        Assert.Equal("C", typeof(TestD).GetProperty("ValueC", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(b));
        Assert.Equal("D", typeof(TestD).GetField("ValueD", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(b));

        Assert.Equal("A", c.ValueA);
        Assert.Equal("B", c.ValueB);
        Assert.Equal("C", c.ValueC);
        Assert.Equal("D", c.ValueD);
    }

    // 不自动绑定的时候
    [Fact]
    public void NotBind()
    {
        var mapper = new MaomiMapper();
        mapper.IsAutoBuild = false;
        Assert.Throws<InvalidCastException>(() =>
        {
            mapper.Map<TestC, TestD>(new TestC
            {
                ValueA = "A",
                ValueB = "B"
            });
        });
    }
}
