﻿/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
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

using System.ComponentModel.DataAnnotations;

namespace OpenIZAdmin.Models.RealmModels
{
	public class JoinRealmModel
	{
		public JoinRealmModel()
		{
		}

		[Required]
		[StringLength(255)]
		public string Address { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Application Id")]
		public string ApplicationId { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Application Secret")]
		public string ApplicationSecret { get; set; }

		[StringLength(255)]
		public string Description { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Username")]
		public string Username { get; set; }
	}
}