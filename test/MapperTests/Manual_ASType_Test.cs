using Maomi.Mapper;

namespace MapperTests;

public class Manual_ASType_Test
{
    public class TestValue
    {
        public Boolean ValueA { get; set; } = true;
        public SByte ValueB { get; set; } = 1;
        public Byte ValueC { get; set; } = 2;
        public Int16 ValueD { get; set; } = 3;
        public UInt16 ValueE { get; set; } = 4;
        public Int32 ValueF { get; set; } = 5;
        public Int64 ValueG { get; set; } = 6;
        public UInt64 ValueH { get; set; } = 7;
        public Single ValueI { get; set; } = 8;
        public Double ValueJ { get; set; } = 9;
        public Decimal ValueK { get; set; } = 10;
        public Char ValueL { get; set; } = (Char)11;
    }

    public class TestBase<T>
    {
        public T ValueA { get; set; }
        public T ValueB { get; set; }
        public T ValueC { get; set; }
        public T ValueD { get; set; }
        public T ValueE { get; set; }
        public T ValueF { get; set; }
        public T ValueG { get; set; }
        public T ValueH { get; set; }
        public T ValueI { get; set; }
        public T ValueJ { get; set; }
        public T ValueK { get; set; }
        public T ValueL { get; set; }
    }
    public class TestA : TestBase<Boolean> { }
    public class TestB : TestBase<SByte> { }
    public class TestC : TestBase<Byte> { }
    public class TestD : TestBase<Int16> { }
    public class TestE : TestBase<UInt16> { }

    public class TestF : TestBase<Int32> { }
    public class TestG : TestBase<Int64> { }
    public class TestH : TestBase<UInt64> { }
    public class TestI : TestBase<Single> { }
    public class TestJ : TestBase<Double> { }
    public class TestK : TestBase<Decimal> { }
    public class TestL : TestBase<Char> { }

    // 类型转换
    [Fact]
    public void TypeAS()
    {
        var mapper = new MaomiMapper();
        mapper.Bind<TestValue, TestA>().Build();
        mapper.Bind<TestValue, TestB>().Build();
        mapper.Bind<TestValue, TestC>().Build();
        mapper.Bind<TestValue, TestD>().Build();
        mapper.Bind<TestValue, TestE>().Build();
        mapper.Bind<TestValue, TestF>().Build();
        mapper.Bind<TestValue, TestG>().Build();
        mapper.Bind<TestValue, TestH>().Build();
        mapper.Bind<TestValue, TestI>().Build();
        mapper.Bind<TestValue, TestJ>().Build();
        mapper.Bind<TestValue, TestK>().Build();
        mapper.Bind<TestValue, TestL>().Build();

        var a = mapper.Map<TestValue, TestA>(new TestValue());
        var b = mapper.Map<TestValue, TestB>(new TestValue());
        var c = mapper.Map<TestValue, TestC>(new TestValue());
        var d = mapper.Map<TestValue, TestD>(new TestValue());
        var e = mapper.Map<TestValue, TestE>(new TestValue());
        var f = mapper.Map<TestValue, TestF>(new TestValue());
        var g = mapper.Map<TestValue, TestG>(new TestValue());
        var h = mapper.Map<TestValue, TestH>(new TestValue());
        var i = mapper.Map<TestValue, TestI>(new TestValue());
        var j = mapper.Map<TestValue, TestJ>(new TestValue());
        var k = mapper.Map<TestValue, TestK>(new TestValue());
        var l = mapper.Map<TestValue, TestL>(new TestValue());

        Assert.Equal(true, a.ValueA);
        Assert.Equal(true, a.ValueB);
        Assert.Equal(true, a.ValueC);
        Assert.Equal(true, a.ValueD);
        Assert.Equal(true, a.ValueE);
        Assert.Equal(true, a.ValueF);
        Assert.Equal(true, a.ValueG);
        Assert.Equal(true, a.ValueH);
        Assert.Equal(true, a.ValueI);
        Assert.Equal(true, a.ValueJ);
        Assert.Equal(true, a.ValueK);
        Assert.Equal(true, a.ValueL);

        Assert.Equal(1, b.ValueA);
        Assert.Equal(1, b.ValueB);
        Assert.Equal(2, b.ValueC);
        Assert.Equal(3, b.ValueD);
        Assert.Equal(4, b.ValueE);
        Assert.Equal(5, b.ValueF);
        Assert.Equal(6, b.ValueG);
        Assert.Equal(6, b.ValueH);
        Assert.Equal(8, b.ValueI);
        Assert.Equal(9, b.ValueJ);
        Assert.Equal(10, b.ValueK);
        Assert.Equal(11, b.ValueL);

        Assert.Equal(1, c.ValueA);
        Assert.Equal(1, c.ValueB);
        Assert.Equal(2, c.ValueC);
        Assert.Equal(3, c.ValueD);
        Assert.Equal(4, c.ValueE);
        Assert.Equal(5, c.ValueF);
        Assert.Equal(6, c.ValueG);
        Assert.Equal(6, c.ValueH);
        Assert.Equal(8, c.ValueI);
        Assert.Equal(9, c.ValueJ);
        Assert.Equal(10, c.ValueK);
        Assert.Equal(11, c.ValueL);

        Assert.Equal(1, d.ValueA);
        Assert.Equal(1, d.ValueB);
        Assert.Equal(2, d.ValueC);
        Assert.Equal(3, d.ValueD);
        Assert.Equal(4, d.ValueE);
        Assert.Equal(5, d.ValueF);
        Assert.Equal(6, d.ValueG);
        Assert.Equal(6, d.ValueH);
        Assert.Equal(8, d.ValueI);
        Assert.Equal(9, d.ValueJ);
        Assert.Equal(10, d.ValueK);
        Assert.Equal(11, d.ValueL);


        Assert.Equal(1, e.ValueA);
        Assert.Equal(1, e.ValueB);
        Assert.Equal(2, e.ValueC);
        Assert.Equal(3, e.ValueD);
        Assert.Equal(4, e.ValueE);
        Assert.Equal(5, e.ValueF);
        Assert.Equal(6, e.ValueG);
        Assert.Equal(6, e.ValueH);
        Assert.Equal(8, e.ValueI);
        Assert.Equal(9, e.ValueJ);
        Assert.Equal(10, e.ValueK);
        Assert.Equal(11, e.ValueL);


        Assert.Equal(1, f.ValueA);
        Assert.Equal(1, f.ValueB);
        Assert.Equal(2, f.ValueC);
        Assert.Equal(3, f.ValueD);
        Assert.Equal(4, f.ValueE);
        Assert.Equal(5, f.ValueF);
        Assert.Equal(6, f.ValueG);
        Assert.Equal(6, f.ValueH);
        Assert.Equal(8, f.ValueI);
        Assert.Equal(9, f.ValueJ);
        Assert.Equal(10, f.ValueK);
        Assert.Equal(11, f.ValueL);


        Assert.Equal(1, g.ValueA);
        Assert.Equal(1, g.ValueB);
        Assert.Equal(2, g.ValueC);
        Assert.Equal(3, g.ValueD);
        Assert.Equal(4, g.ValueE);
        Assert.Equal(5, g.ValueF);
        Assert.Equal(6, g.ValueG);
        Assert.Equal(6, g.ValueH);
        Assert.Equal(8, g.ValueI);
        Assert.Equal(9, g.ValueJ);
        Assert.Equal(10, g.ValueK);
        Assert.Equal(11, g.ValueL);

        Assert.Equal((ulong)1, h.ValueA);
        Assert.Equal((ulong)1, h.ValueB);
        Assert.Equal((ulong)2, h.ValueC);
        Assert.Equal((ulong)3, h.ValueD);
        Assert.Equal((ulong)4, h.ValueE);
        Assert.Equal((ulong)5, h.ValueF);
        Assert.Equal((ulong)6, h.ValueG);
        Assert.Equal((ulong)6, h.ValueH);
        Assert.Equal((ulong)8, h.ValueI);
        Assert.Equal((ulong)9, h.ValueJ);
        Assert.Equal((ulong)10, h.ValueK);
        Assert.Equal((ulong)11, h.ValueL);


        Assert.Equal(1, i.ValueA);
        Assert.Equal(1, i.ValueB);
        Assert.Equal(2, i.ValueC);
        Assert.Equal(3, i.ValueD);
        Assert.Equal(4, i.ValueE);
        Assert.Equal(5, i.ValueF);
        Assert.Equal(6, i.ValueG);
        Assert.Equal(6, i.ValueH);
        Assert.Equal(8, i.ValueI);
        Assert.Equal(9, i.ValueJ);
        Assert.Equal(10, i.ValueK);
        Assert.Equal(11, i.ValueL);


        Assert.Equal(1, j.ValueA);
        Assert.Equal(1, j.ValueB);
        Assert.Equal(2, j.ValueC);
        Assert.Equal(3, j.ValueD);
        Assert.Equal(4, j.ValueE);
        Assert.Equal(5, j.ValueF);
        Assert.Equal(6, j.ValueG);
        Assert.Equal(6, j.ValueH);
        Assert.Equal(8, j.ValueI);
        Assert.Equal(9, j.ValueJ);
        Assert.Equal(10, j.ValueK);
        Assert.Equal(11, j.ValueL);


        Assert.Equal(1, k.ValueA);
        Assert.Equal(1, k.ValueB);
        Assert.Equal(2, k.ValueC);
        Assert.Equal(3, k.ValueD);
        Assert.Equal(4, k.ValueE);
        Assert.Equal(5, k.ValueF);
        Assert.Equal(6, k.ValueG);
        Assert.Equal(6, k.ValueH);
        Assert.Equal(8, k.ValueI);
        Assert.Equal(9, k.ValueJ);
        Assert.Equal(10, k.ValueK);
        Assert.Equal(11, k.ValueL);


        Assert.Equal(1, l.ValueA);
        Assert.Equal(1, l.ValueB);
        Assert.Equal(2, l.ValueC);
        Assert.Equal(3, l.ValueD);
        Assert.Equal(4, l.ValueE);
        Assert.Equal(5, l.ValueF);
        Assert.Equal(6, l.ValueG);
        Assert.Equal(6, l.ValueH);
        Assert.Equal(8, l.ValueI);
        Assert.Equal(9, l.ValueJ);
        Assert.Equal(10, l.ValueK);
        Assert.Equal(11, l.ValueL);
    }

    // 自动创建映射
    [Fact]
    public void TypeAS_AutoBind()
    {
        var mapper = new MaomiMapper();

        var a = mapper.Map<TestValue, TestA>(new TestValue());
        var b = mapper.Map<TestValue, TestB>(new TestValue());
        var c = mapper.Map<TestValue, TestC>(new TestValue());
        var d = mapper.Map<TestValue, TestD>(new TestValue());
        var e = mapper.Map<TestValue, TestE>(new TestValue());
        var f = mapper.Map<TestValue, TestF>(new TestValue());
        var g = mapper.Map<TestValue, TestG>(new TestValue());
        var h = mapper.Map<TestValue, TestH>(new TestValue());
        var i = mapper.Map<TestValue, TestI>(new TestValue());
        var j = mapper.Map<TestValue, TestJ>(new TestValue());
        var k = mapper.Map<TestValue, TestK>(new TestValue());
        var l = mapper.Map<TestValue, TestL>(new TestValue());

        Assert.Equal(true, a.ValueA);
        Assert.Equal(true, a.ValueB);
        Assert.Equal(true, a.ValueC);
        Assert.Equal(true, a.ValueD);
        Assert.Equal(true, a.ValueE);
        Assert.Equal(true, a.ValueF);
        Assert.Equal(true, a.ValueG);
        Assert.Equal(true, a.ValueH);
        Assert.Equal(true, a.ValueI);
        Assert.Equal(true, a.ValueJ);
        Assert.Equal(true, a.ValueK);
        Assert.Equal(true, a.ValueL);

        Assert.Equal(1, b.ValueA);
        Assert.Equal(1, b.ValueB);
        Assert.Equal(2, b.ValueC);
        Assert.Equal(3, b.ValueD);
        Assert.Equal(4, b.ValueE);
        Assert.Equal(5, b.ValueF);
        Assert.Equal(6, b.ValueG);
        Assert.Equal(6, b.ValueH);
        Assert.Equal(8, b.ValueI);
        Assert.Equal(9, b.ValueJ);
        Assert.Equal(10, b.ValueK);
        Assert.Equal(11, b.ValueL);

        Assert.Equal(1, c.ValueA);
        Assert.Equal(1, c.ValueB);
        Assert.Equal(2, c.ValueC);
        Assert.Equal(3, c.ValueD);
        Assert.Equal(4, c.ValueE);
        Assert.Equal(5, c.ValueF);
        Assert.Equal(6, c.ValueG);
        Assert.Equal(6, c.ValueH);
        Assert.Equal(8, c.ValueI);
        Assert.Equal(9, c.ValueJ);
        Assert.Equal(10, c.ValueK);
        Assert.Equal(11, c.ValueL);

        Assert.Equal(1, d.ValueA);
        Assert.Equal(1, d.ValueB);
        Assert.Equal(2, d.ValueC);
        Assert.Equal(3, d.ValueD);
        Assert.Equal(4, d.ValueE);
        Assert.Equal(5, d.ValueF);
        Assert.Equal(6, d.ValueG);
        Assert.Equal(6, d.ValueH);
        Assert.Equal(8, d.ValueI);
        Assert.Equal(9, d.ValueJ);
        Assert.Equal(10, d.ValueK);
        Assert.Equal(11, d.ValueL);


        Assert.Equal(1, e.ValueA);
        Assert.Equal(1, e.ValueB);
        Assert.Equal(2, e.ValueC);
        Assert.Equal(3, e.ValueD);
        Assert.Equal(4, e.ValueE);
        Assert.Equal(5, e.ValueF);
        Assert.Equal(6, e.ValueG);
        Assert.Equal(6, e.ValueH);
        Assert.Equal(8, e.ValueI);
        Assert.Equal(9, e.ValueJ);
        Assert.Equal(10, e.ValueK);
        Assert.Equal(11, e.ValueL);


        Assert.Equal(1, f.ValueA);
        Assert.Equal(1, f.ValueB);
        Assert.Equal(2, f.ValueC);
        Assert.Equal(3, f.ValueD);
        Assert.Equal(4, f.ValueE);
        Assert.Equal(5, f.ValueF);
        Assert.Equal(6, f.ValueG);
        Assert.Equal(6, f.ValueH);
        Assert.Equal(8, f.ValueI);
        Assert.Equal(9, f.ValueJ);
        Assert.Equal(10, f.ValueK);
        Assert.Equal(11, f.ValueL);


        Assert.Equal(1, g.ValueA);
        Assert.Equal(1, g.ValueB);
        Assert.Equal(2, g.ValueC);
        Assert.Equal(3, g.ValueD);
        Assert.Equal(4, g.ValueE);
        Assert.Equal(5, g.ValueF);
        Assert.Equal(6, g.ValueG);
        Assert.Equal(6, g.ValueH);
        Assert.Equal(8, g.ValueI);
        Assert.Equal(9, g.ValueJ);
        Assert.Equal(10, g.ValueK);
        Assert.Equal(11, g.ValueL);

        Assert.Equal((ulong)1, h.ValueA);
        Assert.Equal((ulong)1, h.ValueB);
        Assert.Equal((ulong)2, h.ValueC);
        Assert.Equal((ulong)3, h.ValueD);
        Assert.Equal((ulong)4, h.ValueE);
        Assert.Equal((ulong)5, h.ValueF);
        Assert.Equal((ulong)6, h.ValueG);
        Assert.Equal((ulong)6, h.ValueH);
        Assert.Equal((ulong)8, h.ValueI);
        Assert.Equal((ulong)9, h.ValueJ);
        Assert.Equal((ulong)10, h.ValueK);
        Assert.Equal((ulong)11, h.ValueL);


        Assert.Equal(1, i.ValueA);
        Assert.Equal(1, i.ValueB);
        Assert.Equal(2, i.ValueC);
        Assert.Equal(3, i.ValueD);
        Assert.Equal(4, i.ValueE);
        Assert.Equal(5, i.ValueF);
        Assert.Equal(6, i.ValueG);
        Assert.Equal(6, i.ValueH);
        Assert.Equal(8, i.ValueI);
        Assert.Equal(9, i.ValueJ);
        Assert.Equal(10, i.ValueK);
        Assert.Equal(11, i.ValueL);


        Assert.Equal(1, j.ValueA);
        Assert.Equal(1, j.ValueB);
        Assert.Equal(2, j.ValueC);
        Assert.Equal(3, j.ValueD);
        Assert.Equal(4, j.ValueE);
        Assert.Equal(5, j.ValueF);
        Assert.Equal(6, j.ValueG);
        Assert.Equal(6, j.ValueH);
        Assert.Equal(8, j.ValueI);
        Assert.Equal(9, j.ValueJ);
        Assert.Equal(10, j.ValueK);
        Assert.Equal(11, j.ValueL);


        Assert.Equal(1, k.ValueA);
        Assert.Equal(1, k.ValueB);
        Assert.Equal(2, k.ValueC);
        Assert.Equal(3, k.ValueD);
        Assert.Equal(4, k.ValueE);
        Assert.Equal(5, k.ValueF);
        Assert.Equal(6, k.ValueG);
        Assert.Equal(6, k.ValueH);
        Assert.Equal(8, k.ValueI);
        Assert.Equal(9, k.ValueJ);
        Assert.Equal(10, k.ValueK);
        Assert.Equal(11, k.ValueL);


        Assert.Equal(1, l.ValueA);
        Assert.Equal(1, l.ValueB);
        Assert.Equal(2, l.ValueC);
        Assert.Equal(3, l.ValueD);
        Assert.Equal(4, l.ValueE);
        Assert.Equal(5, l.ValueF);
        Assert.Equal(6, l.ValueG);
        Assert.Equal(6, l.ValueH);
        Assert.Equal(8, l.ValueI);
        Assert.Equal(9, l.ValueJ);
        Assert.Equal(10, l.ValueK);
        Assert.Equal(11, l.ValueL);
    }

    public class TestM : TestBase<String> { }

    // 任意类型转 string
    public void AnyType_AS_String()
    {
        var mapper = new MaomiMapper();
        var a = mapper.Map<TestValue, TestM>(new TestValue());

        Assert.Equal("1", a.ValueA);
        Assert.Equal("1", a.ValueB);
        Assert.Equal("2", a.ValueC);
        Assert.Equal("3", a.ValueD);
        Assert.Equal("4", a.ValueE);
        Assert.Equal("5", a.ValueF);
        Assert.Equal("6", a.ValueG);
        Assert.Equal("7", a.ValueH);
        Assert.Equal("8", a.ValueI);
        Assert.Equal("9", a.ValueJ);
        Assert.Equal("10", a.ValueK);
        Assert.Equal("11", a.ValueL);
    }
}
