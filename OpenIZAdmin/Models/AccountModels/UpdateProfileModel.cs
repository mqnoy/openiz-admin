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
 * Date: 2016-9-5
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenIZAdmin.Models.AccountModels
{
	/// <summary>
	/// Represents a model to allow a user to update their profile.
	/// </summary>
	public class UpdateProfileModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateProfileModel"/> class.
		/// </summary>
		public UpdateProfileModel()
		{
			this.FacilityList = new List<SelectListItem>();
			this.FamilyNameList = new List<SelectListItem>();
			this.FamilyNames = new List<string>();
			this.GivenNamesList = new List<SelectListItem>();
			this.GivenNames = new List<string>();
			this.LanguageList = new List<SelectListItem>();
			this.PhoneTypeList = new List<SelectListItem>();
		}

		/// <summary>
		/// Gets or sets the id of the facility of the user.
		/// </summary>
		[Display(Name = "Facility", ResourceType = typeof(Localization.Locale))]
		[Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Localization.Locale))]
		public string FacilityId { get; set; }

		/// <summary>
		/// Gets or sets the list of facilities.
		/// </summary>
		public List<SelectListItem> FacilityList { get; set; }

		public List<SelectListItem> FamilyNameList { get; set; }

		/// <summary>
		/// Gets or sets the family names of the user.
		/// </summary>
		[Display(Name = "FamilyNames", ResourceType = typeof(Localization.Locale))]
		public List<string> FamilyNames { get; set; }

		public List<SelectListItem> GivenNamesList { get; set; }

		/// <summary>
		/// Gets or sets the givens names of the user.
		/// </summary>
		[Display(Name = "Names", ResourceType = typeof(Localization.Locale))]
		public List<string> GivenNames { get; set; }

		/// <summary>
		/// Gets or sets the default language of the user.
		/// </summary>
		[Display(Name = "Language", ResourceType = typeof(Localization.Locale))]
		[Required(ErrorMessageResourceName = "LanguageRequired", ErrorMessageResourceType = typeof(Localization.Locale))]
		public Guid Language { get; set; }

		/// <summary>
		/// Gets or sets the list of languages.
		/// </summary>
		public List<SelectListItem> LanguageList { get; set; }

		/// <summary>
		/// Gets or sets the phone number of the user.
		/// </summary>
		[Display(Name = "Phone", ResourceType = typeof(Localization.Locale))]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the phone type of the user.
		/// </summary>
		[Display(Name = "PhoneType", ResourceType = typeof(Localization.Locale))]
		public string PhoneType { get; set; }

		/// <summary>
		/// Gets or sets the types of phones.
		/// </summary>
		public List<SelectListItem> PhoneTypeList { get; set; }
	}
}