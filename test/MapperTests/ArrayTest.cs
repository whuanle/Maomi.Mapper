using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maomi.Mapper;

namespace MapperTests
{
	public class ArrayTest
	{
		public class TestA
		{
			public int[] Value { get; set; }
		}
		public class TestB
		{
			public int[] Value { get; set; }
		}

		[Fact]
		public void Array()
		{
			var mapper = new MaomiMapper();
			mapper.Bind<TestA, TestB>(option =>
			{
				option.IsObjectReference = true;
			}).BuildAndReverse(option =>
			{
				option.IsObjectReference = false;
			});

			var a = new TestA
			{
				Value = new[] { 1, 2, 3 }
			};
			var b = mapper.Map<TestA, TestB>(a);
		}

		[Fact]
		public void Array_Not_Ref()
		{
			var mapper = new MaomiMapper();
			mapper.Bind<TestA, TestB>(option =>
			{
				option.IsObjectReference = false;
			}).Build();

			var a = new TestA
			{
				Value = new[] { 1, 2, 3 }
			};
			var b = mapper.Map<TestA, TestB>(a);
		}

		[Fact]
		public void List()
		{
			var mapper = new MaomiMapper();
			mapper.Bind<TestC, TestD>(option =>
			{
				option.IsObjectReference = false;
			}).Build();

			var a = new TestA
			{
				Value = new[] { 1, 2, 3 }
			};
			var b = mapper.Map<TestA, TestB>(a);
		}

		public class TestC
		{
			public List<int> Value { get; set; }
		}
		public class TestD
		{
			public List<int> Value { get; set; }
		}


		[Fact]
		public void List_Ref()
		{
			var mapper = new MaomiMapper();
			mapper.Bind<TestC, TestD>(option =>
			{
				option.IsObjectReference = true;
			}).Build();

			var a = new TestA
			{
				Value = new[] { 1, 2, 3 }
			};
			var b = mapper.Map<TestA, TestB>(a);
		}

		public class TestE
		{
			public List<int> Value { get; set; }
		}
		public class TestF
		{
			public IEnumerable<int> Value { get; set; }
		}
		public class TestG
		{
			public HashSet<int> Value { get; set; }
		}


		[Fact]
		public void List_IEnumerable()
		{
			var mapper = new MaomiMapper();
			mapper.Bind<TestE, TestF>(option =>
			{
				option.IsObjectReference = false;
			}).Build();

			var a = new TestE
			{
				Value = new List<int> { 1, 2, 3 }
			};
			var b = mapper.Map<TestE, TestF>(a);
		}
	}
}
