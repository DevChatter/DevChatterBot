/*
 * Copyright 2013 Georgia Tech Applied Research Corporation
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
	/**
	 * A mutable {@linkplain BkTree BK-tree}.
	 *
	 * <p>Mutating operations are <em>not</em> thread-safe.
	 *
	 * <p>Whereas the {@linkplain #add(Object) mutating methods} are iterative and
	 * can thus handle very large trees, the {@link #equals(Object)},
	 * {@link #hashCode()} and {@link #toString()} methods on this class and its
	 * {@link BkTree.Node} implementation are each recursive and as such may not
	 * complete normally when called on very deep trees.
	 *
	 * @param <E> type of elements in this tree
	 */
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	public sealed class MutableBkTree<TKey, TValue> : IBkTree<TKey, TValue>
	{
		public IMetric<TKey> Metric { get; }
		
		public MutableBkTreeNode<TKey, TValue> Root { get; private set; }
		IBkTreeNode<TKey, TValue> IBkTree<TKey, TValue>.Root => Root;

		public MutableBkTree(IMetric<TKey> metric)
		{
			Metric = metric ?? throw new ArgumentNullException(nameof(metric));
		}

		/**
		 * Adds the given element to this tree, if it's not already present.
		 *
		 * @param element element
		 */
		public void Add(TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			if (Root == null)
			{
				Root = new MutableBkTreeNode<TKey, TValue>(key, value);
			}
			else
			{
				var node = Root;
				while (!node.Key.Equals(key))
				{
					var distance = Distance(node.Key, key);

					var parent = node;
					if (!parent.ChildrenByDistance.ContainsKey(distance))
					{
						node = new MutableBkTreeNode<TKey, TValue>(key, value);
						parent.ChildrenByDistance.Add(distance, node);
						break;
					}

					node = parent.ChildrenByDistance[distance];
				}
			}
		}

		private Int32 Distance(TKey x, TKey y)
		{
			var distance = Metric.Distance(x, y);
			if (distance < 0)
				throw new IllegalMetricException($"negative distance ({distance}) defined between elements `{x}` and `{y}`");

			return distance;
		}

		/**
		 * Adds all of the given elements to this tree.
		 *
		 * @param elements elements
		 */
		public void AddAll(IEnumerable<(TKey, TValue)> elements)
		{
			if (elements == null)
				throw new ArgumentNullException(nameof(elements));

			foreach (var (key, value) in elements)
			{
				Add(key, value);
			}
		}

		/**
		 * Adds all of the given elements to this tree.
		 *
		 * @param elements elements
		 */
		public void AddAll(params (TKey, TValue)[] elements)
		{
			if (elements == null)
				throw new ArgumentNullException(nameof(elements));

			AddAll(elements.AsEnumerable());
		}

		private Boolean Equals(MutableBkTree<TKey, TValue> other) =>
			Equals(Metric, other.Metric)
			&& Equals(Root, other.Root);

		public override Boolean Equals(Object obj) => 
			ReferenceEquals(this, obj) || obj is MutableBkTree<TKey, TValue> other && Equals(other);

		public override Int32 GetHashCode()
		{
			unchecked
			{
				return ((Metric != null ? Metric.GetHashCode() : 0) * 397) ^ (Root != null ? Root.GetHashCode() : 0);
			}
		}

		public override String ToString()
		{
			var sb = new StringBuilder("MutableBkTree{");
			sb.Append("metric=").Append(Metric);
			sb.Append(", root=").Append(Root);
			sb.Append('}');
			return sb.ToString();
		}
	}
}