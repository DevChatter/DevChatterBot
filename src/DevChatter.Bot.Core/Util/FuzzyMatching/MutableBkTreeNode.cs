using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
	public sealed class MutableBkTreeNode<TKey, TValue> : IBkTreeNode<TKey, TValue>
	{
		public TKey Key { get; }
		
		public TValue Value { get; }

		internal IDictionary<Int32, MutableBkTreeNode<TKey, TValue>> ChildrenByDistance { get; } = new Dictionary<Int32, MutableBkTreeNode<TKey, TValue>>();

		internal MutableBkTreeNode(TKey element, TValue value)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			Key = element;
			Value = value;
		}

		public IBkTreeNode<TKey, TValue> GetChildNode(Int32 distance) =>
			ChildrenByDistance.ContainsKey(distance)
				? ChildrenByDistance[distance]
				: null;

		private Boolean Equals(MutableBkTreeNode<TKey, TValue> other) =>
			Equals(Key, other.Key)
			&& Equals(ChildrenByDistance, other.ChildrenByDistance);

		public override Boolean Equals(Object obj) =>
			ReferenceEquals(this, obj) || obj is MutableBkTreeNode<TKey, TValue> other && Equals(other);

		public override Int32 GetHashCode()
		{
			var result = Key.GetHashCode();
			result = 31 * result + ChildrenByDistance.GetHashCode();
			return result;
		}

		public override String ToString()
		{
			var sb = new StringBuilder("MutableNode{");
			sb.Append("element=").Append(Key);
			sb.Append(", childrenByDistance=").Append(ChildrenByDistance);
			sb.Append('}');
			return sb.ToString();
		}
	}
}