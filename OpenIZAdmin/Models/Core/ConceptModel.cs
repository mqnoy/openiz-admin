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
 * User: Andrew
 * Date: 2017-4-12
 */

using OpenIZAdmin.Localization;
using OpenIZAdmin.Models.ReferenceTermModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenIZAdmin.Models.Core
{
	/// <summary>
	/// Represents a model of a concept.
	/// </summary>
	public abstract class ConceptModel
	{
		/// <summary>
		/// Gets or sets the concatenated string of Concept names
		/// </summary>
		public string ConceptNames { get; set; }

		/// <summary>
		/// Gets or sets the creation time of the concept.
		/// </summary>
		[Display(Name = "CreationTime", ResourceType = typeof(Locale))]
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets the key of the concept.
		/// </summary>
		[Required]
		public virtual Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the obsolete property of the Concept indicating object state
		/// </summary>
		public bool IsObsolete { get; set; }

		/// <summary>
		/// Gets or sets the readonly property of the Concept
		/// </summary>
		public bool IsSystemConcept { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		[Display(Name = "Language", ResourceType = typeof(Locale))]
		[StringLength(2, ErrorMessageResourceName = "LanguagCodeTooLong", ErrorMessageResourceType = typeof(Locale))]
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the mnemonic of the concept.
		/// </summary>
		[Display(Name = "Mnemonic", ResourceType = typeof(Locale))]
		[Required(ErrorMessageResourceName = "MnemonicRequired", ErrorMessageResourceType = typeof(Locale))]
		[StringLength(64, ErrorMessageResourceName = "MnemonicLength64", ErrorMessageResourceType = typeof(Locale))]
		[RegularExpression(Constants.RegExBasicString, ErrorMessageResourceName = "InvalidStringEntry", ErrorMessageResourceType = typeof(Locale))]
		public string Mnemonic { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[Display(Name = "Name", ResourceType = typeof(Locale))]
		[StringLength(256, ErrorMessageResourceName = "NameLength256", ErrorMessageResourceType = typeof(Locale))]
		[RegularExpression(Constants.RegExBasicString, ErrorMessageResourceName = "InvalidStringEntry", ErrorMessageResourceType = typeof(Locale))]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a list of names associated with the concept.
		/// </summary>
		[Display(Name = "Name", ResourceType = typeof(Locale))]
		public List<string> Names { get; set; }

		/// <summary>
		/// Gets or sets the list of reference terms associated with the concept.
		/// </summary>
		[Display(Name = "ReferenceTerms", ResourceType = typeof(Locale))]
		public List<ReferenceTermViewModel> ReferenceTerms { get; set; }

		/// <summary>
		/// Gets or sets the version key of the concept.
		/// </summary>
		public virtual Guid? VersionKey { get; set; }
	}
}