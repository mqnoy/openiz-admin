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
 * Date: 2017-4-13
 */

using OpenIZ.Core.Model.DataTypes;
using OpenIZAdmin.Models.Core;
using System;

namespace OpenIZAdmin.Models.ReferenceTermNameModels
{
	/// <summary>
	/// Represents a reference term view model.
	/// </summary>
	public class ReferenceTermNameViewModel : ReferenceTermNameModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceTermNameViewModel"/> class.
		/// </summary>
		public ReferenceTermNameViewModel(ReferenceTermName referenceTermName)
		{
			Id = referenceTermName.Key ?? Guid.Empty;
			Language = referenceTermName.Language;
			Name = referenceTermName.Name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceTermNameViewModel"/> class.
		/// </summary>
		public ReferenceTermNameViewModel(Guid? id, string langCode, string name, ReferenceTerm referenceTerm)
		{
			Id = id;
			Language = langCode;
			Name = name;
			ReferenceTermId = referenceTerm.Key;
		}
	}
}