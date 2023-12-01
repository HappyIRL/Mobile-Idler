using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Utils
{
	public interface IReadOnlyTwoDimensionalArray<T> : IEnumerable<T>
	{
		int Rows { get; }
		int Columns { get; }

		//indexer that allows an object to be accessed using square brackets, providing a way to retrieve or set values based on specified indices.
		T this[int row, int col] { get; }
	}

	public class ReadOnlyTwoDimensionalArray<T> : IReadOnlyTwoDimensionalArray<T>
	{
		private readonly T[,] array;

		public int Rows => array.GetLength(0);
		public int Columns => array.GetLength(1);

		public ReadOnlyTwoDimensionalArray(T[,] array)
		{
			this.array = array ?? throw new ArgumentNullException(nameof(array));
		}

		public T this[int row, int col] => array[row, col];

		public IEnumerator<T> GetEnumerator()
		{
			foreach (var element in array)
			{
				yield return element;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}


