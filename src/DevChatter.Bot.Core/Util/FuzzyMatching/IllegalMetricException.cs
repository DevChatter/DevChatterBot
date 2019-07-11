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
	 * Thrown if a {@link Metric} is not a true metric, e.g., if it defines a
	 * negative distance between any two elements.
	 */
	public class IllegalMetricException : Exception
	{
		public IllegalMetricException(String message)
			: base(message)
		{
		}
	}
}