using Maomi.Mapper;
using System.Runtime.CompilerServices;

namespace MapperTests;

public class Manual_ASType_Test
{
	public class TestValue
	{
		public bool ValueA { get; set; } = true;
		public sbyte ValueB { get; set; } = 1;
		public byte ValueC { get; set; } = 2;
		public short ValueD { get; set; } = 3;
		public ushort ValueE { get; set; } = 4;
		public int ValueF { get; set; } = 5;
		public uint ValueG { get; set; } = 6;
		public long ValueH { get; set; } = 7;
		public ulong ValueI { get; set; } = 8;
		public float ValueJ { get; set; } = 9;
		public double ValueK { get; set; } = 10;
		public decimal ValueL { get; set; } = 11;
		public char ValueM { get; set; } = (Char)12;
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
	public class TestA : TestBase<bool> { }
	public class TestB : TestBase<sbyte> { }
	public class TestC : TestBase<byte> { }
	public class TestD : TestBase<short> { }
	public class TestE : TestBase<ushort> { }
	public class TestF : TestBase<int> { }
	public class TestG : TestBase<uint> { }
	public class TestH : TestBase<long> { }
	public class TestI : TestBase<ulong> { }
	public class TestJ : TestBase<float> { }
	public class TestK : TestBase<double> { }
	public class TestL : TestBase<decimal> { }

	public class TestM : TestBase<char> { }

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
		mapper.Bind<TestValue, TestM>().Build();

		var v = new TestValue();
		var a = mapper.Map<TestValue, TestA>(v);
		var b = mapper.Map<TestValue, TestB>(v);
		var c = mapper.Map<TestValue, TestC>(v);
		var d = mapper.Map<TestValue, TestD>(v);
		var e = mapper.Map<TestValue, TestE>(v);
		var f = mapper.Map<TestValue, TestF>(v);
		var g = mapper.Map<TestValue, TestG>(v);
		var h = mapper.Map<TestValue, TestH>(v);
		var i = mapper.Map<TestValue, TestI>(v);
		var j = mapper.Map<TestValue, TestJ>(v);
		var k = mapper.Map<TestValue, TestK>(v);
		var l = mapper.Map<TestValue, TestL>(v);
		var m = mapper.Map<TestValue, TestM>(v);

		Assert.Equal(Convert.ToBoolean(v.ValueA), a.ValueA);
		Assert.Equal(Convert.ToBoolean(v.ValueB), a.ValueB);
		Assert.Equal(Convert.ToBoolean(v.ValueC), a.ValueC);
		Assert.Equal(Convert.ToBoolean(v.ValueD), a.ValueD);
		Assert.Equal(Convert.ToBoolean(v.ValueE), a.ValueE);
		Assert.Equal(Convert.ToBoolean(v.ValueF), a.ValueF);
		Assert.Equal(Convert.ToBoolean(v.ValueG), a.ValueG);
		Assert.Equal(Convert.ToBoolean(v.ValueH), a.ValueH);
		Assert.Equal(Convert.ToBoolean(v.ValueI), a.ValueI);
		Assert.Equal(Convert.ToBoolean(v.ValueJ), a.ValueJ);
		Assert.Equal(Convert.ToBoolean(v.ValueK), a.ValueK);
		Assert.Equal(Convert.ToBoolean(v.ValueL), a.ValueL);

		Assert.Equal(Convert.ToSByte(v.ValueA), b.ValueA);
		Assert.Equal(Convert.ToSByte(v.ValueB), b.ValueB);
		Assert.Equal(Convert.ToSByte(v.ValueC), b.ValueC);
		Assert.Equal(Convert.ToSByte(v.ValueD), b.ValueD);
		Assert.Equal(Convert.ToSByte(v.ValueE), b.ValueE);
		Assert.Equal(Convert.ToSByte(v.ValueF), b.ValueF);
		Assert.Equal(Convert.ToSByte(v.ValueG), b.ValueG);
		Assert.Equal(Convert.ToSByte(v.ValueH), b.ValueH);
		Assert.Equal(Convert.ToSByte(v.ValueI), b.ValueI);
		Assert.Equal(Convert.ToSByte(v.ValueJ), b.ValueJ);
		Assert.Equal(Convert.ToSByte(v.ValueK), b.ValueK);
		Assert.Equal(Convert.ToSByte(v.ValueL), b.ValueL);

		Assert.Equal(Convert.ToByte(v.ValueA), c.ValueA);
		Assert.Equal(Convert.ToByte(v.ValueB), c.ValueB);
		Assert.Equal(Convert.ToByte(v.ValueC), c.ValueC);
		Assert.Equal(Convert.ToByte(v.ValueD), c.ValueD);
		Assert.Equal(Convert.ToByte(v.ValueE), c.ValueE);
		Assert.Equal(Convert.ToByte(v.ValueF), c.ValueF);
		Assert.Equal(Convert.ToByte(v.ValueG), c.ValueG);
		Assert.Equal(Convert.ToByte(v.ValueH), c.ValueH);
		Assert.Equal(Convert.ToByte(v.ValueI), c.ValueI);
		Assert.Equal(Convert.ToByte(v.ValueJ), c.ValueJ);
		Assert.Equal(Convert.ToByte(v.ValueK), c.ValueK);
		Assert.Equal(Convert.ToByte(v.ValueL), c.ValueL);

		Assert.Equal(Convert.ToInt16(v.ValueA), d.ValueA);
		Assert.Equal(Convert.ToInt16(v.ValueB), d.ValueB);
		Assert.Equal(Convert.ToInt16(v.ValueC), d.ValueC);
		Assert.Equal(Convert.ToInt16(v.ValueD), d.ValueD);
		Assert.Equal(Convert.ToInt16(v.ValueE), d.ValueE);
		Assert.Equal(Convert.ToInt16(v.ValueF), d.ValueF);
		Assert.Equal(Convert.ToInt16(v.ValueG), d.ValueG);
		Assert.Equal(Convert.ToInt16(v.ValueH), d.ValueH);
		Assert.Equal(Convert.ToInt16(v.ValueI), d.ValueI);
		Assert.Equal(Convert.ToInt16(v.ValueJ), d.ValueJ);
		Assert.Equal(Convert.ToInt16(v.ValueK), d.ValueK);
		Assert.Equal(Convert.ToInt16(v.ValueL), d.ValueL);


		Assert.Equal(Convert.ToUInt16(v.ValueA), e.ValueA);
		Assert.Equal(Convert.ToUInt16(v.ValueB), e.ValueB);
		Assert.Equal(Convert.ToUInt16(v.ValueC), e.ValueC);
		Assert.Equal(Convert.ToUInt16(v.ValueD), e.ValueD);
		Assert.Equal(Convert.ToUInt16(v.ValueE), e.ValueE);
		Assert.Equal(Convert.ToUInt16(v.ValueF), e.ValueF);
		Assert.Equal(Convert.ToUInt16(v.ValueG), e.ValueG);
		Assert.Equal(Convert.ToUInt16(v.ValueH), e.ValueH);
		Assert.Equal(Convert.ToUInt16(v.ValueI), e.ValueI);
		Assert.Equal(Convert.ToUInt16(v.ValueJ), e.ValueJ);
		Assert.Equal(Convert.ToUInt16(v.ValueK), e.ValueK);
		Assert.Equal(Convert.ToUInt16(v.ValueL), e.ValueL);


		Assert.Equal(Convert.ToInt32(v.ValueA), f.ValueA);
		Assert.Equal(Convert.ToInt32(v.ValueB), f.ValueB);
		Assert.Equal(Convert.ToInt32(v.ValueC), f.ValueC);
		Assert.Equal(Convert.ToInt32(v.ValueD), f.ValueD);
		Assert.Equal(Convert.ToInt32(v.ValueE), f.ValueE);
		Assert.Equal(Convert.ToInt32(v.ValueF), f.ValueF);
		Assert.Equal(Convert.ToInt32(v.ValueG), f.ValueG);
		Assert.Equal(Convert.ToInt32(v.ValueH), f.ValueH);
		Assert.Equal(Convert.ToInt32(v.ValueI), f.ValueI);
		Assert.Equal(Convert.ToInt32(v.ValueJ), f.ValueJ);
		Assert.Equal(Convert.ToInt32(v.ValueK), f.ValueK);
		Assert.Equal(Convert.ToInt32(v.ValueL), f.ValueL);

		Assert.Equal(Convert.ToUInt32(v.ValueA), g.ValueA);
		Assert.Equal(Convert.ToUInt32(v.ValueB), g.ValueB);
		Assert.Equal(Convert.ToUInt32(v.ValueC), g.ValueC);
		Assert.Equal(Convert.ToUInt32(v.ValueD), g.ValueD);
		Assert.Equal(Convert.ToUInt32(v.ValueE), g.ValueE);
		Assert.Equal(Convert.ToUInt32(v.ValueF), g.ValueF);
		Assert.Equal(Convert.ToUInt32(v.ValueG), g.ValueG);
		Assert.Equal(Convert.ToUInt32(v.ValueH), g.ValueH);
		Assert.Equal(Convert.ToUInt32(v.ValueI), g.ValueI);
		Assert.Equal(Convert.ToUInt32(v.ValueJ), g.ValueJ);
		Assert.Equal(Convert.ToUInt32(v.ValueK), g.ValueK);
		Assert.Equal(Convert.ToUInt32(v.ValueL), g.ValueL);

		Assert.Equal(Convert.ToInt64(v.ValueA), h.ValueA);
		Assert.Equal(Convert.ToInt64(v.ValueB), h.ValueB);
		Assert.Equal(Convert.ToInt64(v.ValueC), h.ValueC);
		Assert.Equal(Convert.ToInt64(v.ValueD), h.ValueD);
		Assert.Equal(Convert.ToInt64(v.ValueE), h.ValueE);
		Assert.Equal(Convert.ToInt64(v.ValueF), h.ValueF);
		Assert.Equal(Convert.ToInt64(v.ValueG), h.ValueG);
		Assert.Equal(Convert.ToInt64(v.ValueH), h.ValueH);
		Assert.Equal(Convert.ToInt64(v.ValueI), h.ValueI);
		Assert.Equal(Convert.ToInt64(v.ValueJ), h.ValueJ);
		Assert.Equal(Convert.ToInt64(v.ValueK), h.ValueK);
		Assert.Equal(Convert.ToInt64(v.ValueL), h.ValueL);

		Assert.Equal(Convert.ToUInt64(v.ValueA), i.ValueA);
		Assert.Equal(Convert.ToUInt64(v.ValueB), i.ValueB);
		Assert.Equal(Convert.ToUInt64(v.ValueC), i.ValueC);
		Assert.Equal(Convert.ToUInt64(v.ValueD), i.ValueD);
		Assert.Equal(Convert.ToUInt64(v.ValueE), i.ValueE);
		Assert.Equal(Convert.ToUInt64(v.ValueF), i.ValueF);
		Assert.Equal(Convert.ToUInt64(v.ValueG), i.ValueG);
		Assert.Equal(Convert.ToUInt64(v.ValueH), i.ValueH);
		Assert.Equal(Convert.ToUInt64(v.ValueI), i.ValueI);
		Assert.Equal(Convert.ToUInt64(v.ValueJ), i.ValueJ);
		Assert.Equal(Convert.ToUInt64(v.ValueK), i.ValueK);
		Assert.Equal(Convert.ToUInt64(v.ValueL), i.ValueL);


		Assert.Equal(Convert.ToSingle(v.ValueA), j.ValueA);
		Assert.Equal(Convert.ToSingle(v.ValueB), j.ValueB);
		Assert.Equal(Convert.ToSingle(v.ValueC), j.ValueC);
		Assert.Equal(Convert.ToSingle(v.ValueD), j.ValueD);
		Assert.Equal(Convert.ToSingle(v.ValueE), j.ValueE);
		Assert.Equal(Convert.ToSingle(v.ValueF), j.ValueF);
		Assert.Equal(Convert.ToSingle(v.ValueG), j.ValueG);
		Assert.Equal(Convert.ToSingle(v.ValueH), j.ValueH);
		Assert.Equal(Convert.ToSingle(v.ValueI), j.ValueI);
		Assert.Equal(Convert.ToSingle(v.ValueJ), j.ValueJ);
		Assert.Equal(Convert.ToSingle(v.ValueK), j.ValueK);
		Assert.Equal(Convert.ToSingle(v.ValueL), j.ValueL);


		Assert.Equal(Convert.ToDouble(v.ValueA), k.ValueA);
		Assert.Equal(Convert.ToDouble(v.ValueB), k.ValueB);
		Assert.Equal(Convert.ToDouble(v.ValueC), k.ValueC);
		Assert.Equal(Convert.ToDouble(v.ValueD), k.ValueD);
		Assert.Equal(Convert.ToDouble(v.ValueE), k.ValueE);
		Assert.Equal(Convert.ToDouble(v.ValueF), k.ValueF);
		Assert.Equal(Convert.ToDouble(v.ValueG), k.ValueG);
		Assert.Equal(Convert.ToDouble(v.ValueH), k.ValueH);
		Assert.Equal(Convert.ToDouble(v.ValueI), k.ValueI);
		Assert.Equal(Convert.ToDouble(v.ValueJ), k.ValueJ);
		Assert.Equal(Convert.ToDouble(v.ValueK), k.ValueK);
		Assert.Equal(Convert.ToDouble(v.ValueL), k.ValueL);


		Assert.Equal(Convert.ToDecimal(v.ValueA), l.ValueA);
		Assert.Equal(Convert.ToDecimal(v.ValueB), l.ValueB);
		Assert.Equal(Convert.ToDecimal(v.ValueC), l.ValueC);
		Assert.Equal(Convert.ToDecimal(v.ValueD), l.ValueD);
		Assert.Equal(Convert.ToDecimal(v.ValueE), l.ValueE);
		Assert.Equal(Convert.ToDecimal(v.ValueF), l.ValueF);
		Assert.Equal(Convert.ToDecimal(v.ValueG), l.ValueG);
		Assert.Equal(Convert.ToDecimal(v.ValueH), l.ValueH);
		Assert.Equal(Convert.ToDecimal(v.ValueI), l.ValueI);
		Assert.Equal(Convert.ToDecimal(v.ValueJ), l.ValueJ);
		Assert.Equal(Convert.ToDecimal(v.ValueK), l.ValueK);
		Assert.Equal(Convert.ToDecimal(v.ValueL), l.ValueL);


		Assert.Equal(Convert.ToChar(Convert.ToUInt16(v.ValueA)), m.ValueA);
		Assert.Equal(Convert.ToChar(v.ValueB), m.ValueB);
		Assert.Equal(Convert.ToChar(v.ValueC), m.ValueC);
		Assert.Equal(Convert.ToChar(v.ValueD), m.ValueD);
		Assert.Equal(Convert.ToChar(v.ValueE), m.ValueE);
		Assert.Equal(Convert.ToChar(v.ValueF), m.ValueF);
		Assert.Equal(Convert.ToChar(v.ValueG), m.ValueG);
		Assert.Equal(Convert.ToChar(v.ValueH), m.ValueH);
		Assert.Equal(Convert.ToChar(v.ValueI), m.ValueI);
		Assert.Equal(Convert.ToChar(Convert.ToUInt16(v.ValueJ)), m.ValueJ);
		Assert.Equal(Convert.ToChar(Convert.ToUInt16(v.ValueK)), m.ValueK);
		Assert.Equal(Convert.ToChar(Convert.ToUInt16(v.ValueL)), m.ValueL);
	}

	public class TestN : TestBase<String> { }

	// 任意类型转 string
	[Fact]
	public void AnyType_AS_String()
	{
		var mapper = new MaomiMapper();
		var a = mapper.Map<TestValue, TestN>(new TestValue());

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
