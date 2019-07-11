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


/**
 * A <a href="http://en.wikipedia.org/wiki/BK-tree">BK-tree</a>.
 *
 * @param <E> type of elements in this tree
 */

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
	public interface IBkTree<TKey, out TValue>
	{
		/** Returns the metric for elements in this tree. */
		IMetric<TKey> Metric { get; }

		/** Returns the root node of this tree. */
		IBkTreeNode<TKey, TValue> Root { get; }
	}
}