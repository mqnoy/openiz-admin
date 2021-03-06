﻿/*
 * Copyright 2016-2017 Mohawk College of Applied Arts and Technology
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: Nityan
 * Date: 2017-7-9
 */

namespace OpenIZAdmin.Core
{
	/// <summary>
	/// Represents a singleton.
	/// </summary>
	/// <typeparam name="T">The type of the instance</typeparam>
	public class Singleton<T>
	{
		/// <summary>
		/// Gets or sets the current.
		/// </summary>
		/// <value>The current.</value>
		public static T Current { get; set; }
	}
}