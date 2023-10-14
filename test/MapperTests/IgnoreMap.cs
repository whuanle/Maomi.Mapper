using Maomi.Mapper;

namespace MapperTests;

public class IgnoreMap
{
    public class TestA
    {
        public string ValueA { get; set; } = "A";

        public string ValueB { get; set; } = "B";

        public string ValueC { get; set; } = "C";
    }
    public class TestB
    {
        public string ValueA { get; set; }

        public string ValueB { get; set; }

        public string ValueC { get; set; }
    }

    /// <summary>
    /// 忽略映射
    /// </summary>
    [Fact]
    public void IgnoreMapField()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestA, TestB>()
        .Ignore(b => b.ValueA).Build();

        var b = mapper.Map<TestA, TestB>(new TestA());

        Assert.Equal(null, b.ValueA);
        Assert.Equal("B", b.ValueB);
        Assert.Equal("C", b.ValueC);
    }
}
