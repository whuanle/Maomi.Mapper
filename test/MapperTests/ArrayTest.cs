using System;
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

		public class TestC
		{
			public List<int> Value { get; set; }
		}
		public class TestD
		{
			public List<int> Value { get; set; }
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
	}
}
