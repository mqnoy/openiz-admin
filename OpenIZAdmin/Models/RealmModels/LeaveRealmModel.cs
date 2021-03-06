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
 * Date: 2016-7-13
 */

using OpenIZAdmin.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OpenIZAdmin.Models.RealmModels
{
	/// <summary>
	/// Represents a leave realm model.
	/// </summary>
	public class LeaveRealmModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LeaveRealmModel"/> class.
		/// </summary>
		public LeaveRealmModel()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LeaveRealmModel"/> class.
		/// </summary>
		public LeaveRealmModel(Realm realm)
		{
			this.CurrentRealm = new RealmViewModel(realm);
			this.Map(realm);
		}

		/// <summary>
		/// Gets or sets the current realm
		/// </summary>
		[Required]
		public RealmViewModel CurrentRealm { get; set; }
	}
}