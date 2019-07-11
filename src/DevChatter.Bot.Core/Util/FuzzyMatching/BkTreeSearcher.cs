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
using System.Linq;
using System.Text;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
	/**
	 * Searches a {@link BkTree}.
	 *
	 * @param <E> type of elements in the searched tree
	 */
	public sealed class BkTreeSearcher<TKey, TValue>
	{
		public IBkTree<TKey, TValue> Tree { get; }

		/**
		 * Constructs a searcher that orders matches in increasing order of
		 * distance from the query.
		 *
		 * @param tree tree to search
		 */
		public BkTreeSearcher(IBkTree<TKey, TValue> tree)
		{
			Tree = tree ?? throw new ArgumentNullException(nameof(tree));
		}

		/**
		 * Searches the tree for elements whose distance from the given query
		 * is less than or equal to the given maximum distance.
		 *
		 * @param query query against which to match tree elements
		 * @param maxDistance non-negative maximum distance of matching elements from query
		 * @return matching elements in no particular order
		 */
		public ISet<SearchMatch<TKey, TValue>> Search(TKey query, Int32 maxDistance)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));
			if (maxDistance < 0) throw new ArgumentException("maxDistance must be non-negative");

			var metric = Tree.Metric;

			ISet<SearchMatch<TKey, TValue>> matches = new HashSet<SearchMatch<TKey, TValue>>();

			var queue = new Queue<IBkTreeNode<TKey, TValue>>();
			queue.Enqueue(Tree.Root);

			while (queue.Count() != 0)
			{
				var node = queue.Dequeue();
				var element = node.Key;

				var distance = metric.Distance(element, query);
				if (distance < 0)
					throw new IllegalMetricException($"negative distance ({distance}) defined between element `{element}` and query `{query}`");

				if (distance <= maxDistance)
					matches.Add(new SearchMatch<TKey, TValue>(element, node.Value, distance));

				var minSearchDistance = Math.Max(distance - maxDistance, 0);
				var maxSearchDistance = distance + maxDistance;

				for (var searchDistance = minSearchDistance; searchDistance <= maxSearchDistance; ++searchDistance)
				{
					var childNode = node.GetChildNode(searchDistance);
					if (childNode != null)
					{
						queue.Enqueue(childNode);
					}
				}
			}

			return matches;
		}
	}

	/**
	 * An element matching a query.
	 *
	 * @param <E> type of matching element
	 */
	public sealed class SearchMatch<TKey, TValue>
	{
		public TKey MatchKey { get; }
		public TValue MatchValue { get; }
		public Int32 Distance { get; }

		/**
		 * @param match matching element
		 * @param distance distance of the matching element from the search query
		 */
		public SearchMatch(TKey matchKey, TValue matchValue, Int32 distance)
		{
			if (matchKey == null) throw new ArgumentNullException(nameof(matchKey));
			if (distance < 0) throw new ArgumentException("distance must be non-negative");

			MatchKey = matchKey;
			MatchValue = matchValue;
			Distance = distance;
		}

		private Boolean Equals(SearchMatch<TKey, TValue> other)
		{
			return EqualityComparer<TKey>.Default.Equals(MatchKey, other.MatchKey) && Distance == other.Distance;
		}

		public override Boolean Equals(Object obj)
		{
			return ReferenceEquals(this, obj) || obj is SearchMatch<TKey, TValue> other && Equals(other);
		}

		public override Int32 GetHashCode()
		{
			unchecked
			{
				return (EqualityComparer<TKey>.Default.GetHashCode(MatchKey) * 397) ^ Distance;
			}
		}

		public override String ToString()
		{
			var sb = new StringBuilder("Match{");
			sb.Append("match=").Append(MatchKey);
			sb.Append(", matchValue=").Append(MatchValue);
			sb.Append(", distance=").Append(Distance);
			sb.Append('}');
			return sb.ToString();
		}
	}
}