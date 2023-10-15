using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Maomi.Mapper;

namespace MaomiMapperBen
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
	public class TestB
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

	public class TestC : TestBase<int> { }

	public class TestD
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

	[SimpleJob(RuntimeMoniker.Net70)]
	[MemoryDiagnoser]
	[MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
	public class TestMapper
	{
		private AutoMapper.IMapper _autoMapper;
		private MaomiMapper _maomi;
		[GlobalSetup]
		public async Task Setup()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<TestValue, TestB>();
				cfg.CreateMap<TestValue, TestC>();
				cfg.CreateMap<TestValue, TestD>();
			});
			_autoMapper = config.CreateMapper();
			_ = _autoMapper.Map<TestValue, TestB>(new TestValue());
			_ = _autoMapper.Map<TestValue, TestC>(new TestValue());
			_ = _autoMapper.Map<TestValue, TestD>(new TestValue());


			_maomi = new MaomiMapper();
			_maomi
				.Bind<TestValue, TestB>()
				.Bind<TestValue, TestC>().Build()
				.Bind<TestValue, TestD>().Build();
		}

		[Benchmark]
		public void ASAutoMapper()
		{
			_ = _autoMapper.Map<TestValue, TestB>(new TestValue());
			_ = _autoMapper.Map<TestValue, TestC>(new TestValue());
		}

		[Benchmark]
		public void ASMaomiMapper()
		{
			_ = _maomi.Map<TestValue, TestB>(new TestValue());
			_ = _maomi.Map<TestValue, TestC>(new TestValue());
		}


		[Benchmark]
		public void _AutoMapper()
		{
			_ = _autoMapper.Map<TestValue, TestD>(new TestValue());
		}

		[Benchmark]
		public void _MaomiMapper()
		{
			_ = _maomi.Map<TestValue, TestD>(new TestValue());
		}
	}


	internal class Program
	{
		static void Main()
		{
			var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
			Console.Read();
		}
	}
}