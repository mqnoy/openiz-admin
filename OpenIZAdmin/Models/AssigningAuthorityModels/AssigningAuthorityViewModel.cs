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
 * Date: 2016-7-8
 */

using OpenIZ.Core.Model.AMI.DataTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenIZAdmin.Models.AssigningAuthorityModels
{
	public class AssigningAuthorityViewModel
	{
		public AssigningAuthorityViewModel()
		{
		}

		public AssigningAuthorityViewModel(AssigningAuthorityInfo assigningAuthority)
		{
			this.Id = assigningAuthority.Id;
			this.Name = assigningAuthority.AssigningAuthority.Name;
			this.Oid = assigningAuthority.AssigningAuthority.Oid;
			this.Url = assigningAuthority.AssigningAuthority.Url;
			this.DomainName = assigningAuthority.AssigningAuthority.DomainName;
			this.Description = assigningAuthority.AssigningAuthority.Description;
		}

		[Display(Name = "Description", ResourceType = typeof(Localization.Locale))]
		public string Description { get; set; }

		[Display(Name = "DomainName", ResourceType = typeof(Localization.Locale))]
		public string DomainName { get; set; }

		public Guid Id { get; set; }

		[Display(Name = "Name", ResourceType = typeof(Localization.Locale))]
		public string Name { get; set; }

		[Display(Name = "Oid", ResourceType = typeof(Localization.Locale))]
		public string Oid { get; set; }

		[Display(Name = "Url", ResourceType = typeof(Localization.Locale))]
		public string Url { get; set; }
	}
}