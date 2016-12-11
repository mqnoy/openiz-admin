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
 * Date: 2016-8-1
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace OpenIZAdmin.Models.MaterialModels.ViewModels
{
	/// <summary>
	/// Represents a material view model.
	/// </summary>
	public class MaterialViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MaterialViewModel"/> class.
		/// </summary>
		public MaterialViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the form concept of the material.
		/// </summary>
		[Display(Name = "FormConcept", ResourceType = typeof(Localization.Locale))]
		public string FormConcept { get; set; }

		/// <summary>
		/// Gets or sets the key of the material.
		/// </summary>
		public Guid Key { get; set; }

		/// <summary>
		/// Gets or sets the name of the material.
		/// </summary>
		[Display(Name = "Name", ResourceType = typeof(Localization.Locale))]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the quantity concept of the material.
		/// </summary>
		[Display(Name = "QuantityConcept", ResourceType = typeof(Localization.Locale))]
		public string QuantityConcept { get; set; }

		/// <summary>
		/// Gets or sets the version key of the material.
		/// </summary>
		public Guid VersionKey { get; set; }
	}
}