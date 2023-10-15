using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maomi.Mapper
{
	internal class ConvertCollectionHelper
	{
		internal record CollectionType
		{
			public Type TSource { get; init; }
			public Type Collection { get; init; }
		}

		protected static readonly Dictionary<CollectionType, Delegate> Cache = new();

		/// <summary>
		/// 转换集合类型
		/// </summary>
		/// <param name="tSource">泛型类型</param>
		/// <param name="targetCollection">目标集合</param>
		/// <returns></returns>
		internal static Delegate? GetExpression(Type tSource, Type targetCollection)
		{
			var collectionType = new CollectionType
			{
				TSource = tSource,
				Collection = targetCollection
			};
			if (Cache.TryGetValue(collectionType, out var @delegate)) return @delegate;

			_ = Activator.CreateInstance(typeof(ConvertCollectionHelper<,>).MakeGenericType(tSource, targetCollection));

			Cache.TryGetValue(collectionType, out @delegate);
			return @delegate;
		}
	}

	/// <summary>
	/// 将一个集合类型转换为另一种集合类型
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TDestinationCollection"></typeparam>
	internal class ConvertCollectionHelper<TSource, TDestinationCollection>: ConvertCollectionHelper
		where TDestinationCollection : IEnumerable
	{
		internal delegate TDestinationCollection Converter(IEnumerable<TSource> sourceCollection);
		private static readonly Action<IEnumerable<TSource>, ICollection<TSource>> CommonConvertCollectionDelegate = (sourceCollection, collection) =>
		{
			foreach (var item in sourceCollection)
			{
				collection.Add(item);
			}
		};

		private static readonly Converter? ConvertCollectionDelegate;
		private static readonly MethodInfo AddMethodInfo;
		public static readonly MethodInfo ConvertCollectionMethodInfo;

		public static Converter? Delegate => ConvertCollectionDelegate;

		static ConvertCollectionHelper()
		{
			var targetType = typeof(TDestinationCollection);
			AddMethodInfo = targetType.GetMethod("Add")!;
			ConvertCollectionMethodInfo = typeof(ConvertCollectionHelper<TSource, TDestinationCollection>).GetMethod("ConvertCollection")!;

			// 集合接口 , List<T> 万能适应各种泛型集合接口
			if (targetType.IsInterface)
			{
				if (targetType.IsAssignableTo(typeof(IEnumerable)))
				{
					ConvertCollectionDelegate = (sourceCollection) =>
					{
						var v = new List<TSource>();
						v.AddRange(sourceCollection);
						return Unsafe.As<List<TSource>, TDestinationCollection>(ref v);
					};
					var collectionType = new CollectionType
					{
						TSource = typeof(TSource),
						Collection = typeof(TDestinationCollection)
					};
					Cache[collectionType] = ConvertCollectionDelegate;
				}
			}

			// 是可以实例化的类型，且可以添加元素
			else if (targetType.IsClass && !targetType.IsAbstract && targetType.IsAssignableTo(typeof(ICollection<TSource>)))
			{
				// (a) =>
				// {
				//  b = new List<x>();
				//  {
				//      foreach (var item in a) b.Add(item);
				//  }
				//  return b;
				// }

				// var list = new();
				// ... ...
				// return b;

				var varB = Expression.Variable(targetType, "vb");
				var deInstance = Expression.New(targetType);
				var assign = Expression.Assign(varB, deInstance);

				var parameterA = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(typeof(TSource)), "a");
				// (a,b) => {foreach (var item in a) b.Add(item);}
				Expression call = Expression.Call(Expression.Constant(CommonConvertCollectionDelegate.Target), CommonConvertCollectionDelegate.Method, parameterA, varB);

				var returnLabel = Expression.Label(targetType);
				var rst = Expression.Label(returnLabel, varB);
				var block = Expression.Block(new[] { varB }, assign, call, rst);

				ConvertCollectionDelegate = Expression.Lambda<Converter>(block, parameterA).Compile();

				var collectionType = new CollectionType
				{
					TSource = typeof(TSource),
					Collection = typeof(TDestinationCollection)
				};
				Cache[collectionType] = ConvertCollectionDelegate;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceCollection"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public static TDestinationCollection ConvertCollection(IEnumerable<TSource> sourceCollection)
		{
			if (ConvertCollectionDelegate != null) return ConvertCollectionDelegate.Invoke(sourceCollection);

			throw new InvalidCastException($"无法将 {typeof(TSource).Name} 转换为 {typeof(TDestinationCollection).Name}");
		}
	}

}
