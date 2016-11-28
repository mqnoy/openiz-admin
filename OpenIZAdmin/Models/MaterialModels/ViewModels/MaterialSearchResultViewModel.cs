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
 * Date: 2016-7-23
 */

using OpenIZ.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenIZ.Core.Model.Entities;

namespace OpenIZAdmin.Models.MaterialModels.ViewModels
{
	/// <summary>
	/// Represents a material search result view model.
	/// </summary>
	public class MaterialSearchResultViewModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="MaterialSearchResultViewModel"/> class.
		/// </summary>
		public MaterialSearchResultViewModel()
		{

		}

		public MaterialSearchResultViewModel(Material material)
		{
			this.CreationTime = material.CreationTime.DateTime;
			this.Key = material.Key.Value;
			this.Name = string.Join(" ", material.Names.SelectMany(m => m.Component).Select(c => c.Value));
			this.VersionKey = material.VersionKey;
		}

		public string Name { get; set; }
        
		public DateTime CreationTime { get; set; }

		public Guid Key { get; set; }

		public Guid? VersionKey { get; set; }
	}
}