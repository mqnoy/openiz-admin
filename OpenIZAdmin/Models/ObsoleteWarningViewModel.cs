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
 * User: khannan
 * Date: 2017-3-28
 */

namespace OpenIZAdmin.Models
{
	/// <summary>
	/// Represents an obsolete warning view model.
	/// </summary>
	public class ObsoleteWarningViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObsoleteWarningViewModel"/> class.
		/// </summary>
		public ObsoleteWarningViewModel()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObsoleteWarningViewModel"/> class.
		/// </summary>
		/// <param name="isObsolete">if set to <c>true</c> [is obsolete].</param>
		/// <param name="warningMessage">The warning message.</param>
		public ObsoleteWarningViewModel(bool isObsolete, string warningMessage)
		{
			this.IsObsolete = isObsolete;
			this.WarningMessage = warningMessage;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is obsolete.
		/// </summary>
		/// <value><c>true</c> if this instance is obsolete; otherwise, <c>false</c>.</value>
		public bool IsObsolete { get; set; }

		/// <summary>
		/// Gets or sets the warning message.
		/// </summary>
		/// <value>The warning message.</value>
		public string WarningMessage { get; set; }
	}
}