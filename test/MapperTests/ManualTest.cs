
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
        var c = typeof(Maomi.Mapper.MaomiMapper);
    }
}
