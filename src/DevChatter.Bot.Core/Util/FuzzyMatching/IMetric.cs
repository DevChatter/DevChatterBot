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

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
	/**
	 * A metric, e.g., a <a href="http://en.wikipedia.org/wiki/String_metric">string metric</a>,
	 * that defines a <a href="http://en.wikipedia.org/wiki/Metric_space">metric space</a>.
	 *
	 * @param <E> type of elements in the metric space defined by this metric
	 */
	public interface IMetric<in T>
	{
		/**
		 * Returns the distance between the given elements.
		 */
		Int32 Distance(T x, T y);
	}
}
