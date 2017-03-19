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
 * Date: 2016-7-23
 */

using OpenIZ.Core.Model.Constants;
using OpenIZ.Core.Model.Entities;
using OpenIZAdmin.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace OpenIZAdmin.Models.PlaceModels
{
	/// <summary>
	/// Represents a place view model.
	/// </summary>
	public class PlaceViewModel : EntityViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PlaceViewModel"/> class.
		/// </summary>
		public PlaceViewModel()
		{
			this.RelatedPlaces = new List<RelatedPlaceModel>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlaceViewModel"/> class.
		/// </summary>
		/// <param name="place">The place.</param>
		public PlaceViewModel(Place place) : base(place)
		{
			this.RelatedPlaces = new List<RelatedPlaceModel>();

			var childPlaces = place.Relationships.Where(r => r.RelationshipTypeKey == EntityRelationshipTypeKeys.Child)
						.Select(r => r.TargetEntity)
						.OfType<Place>()
						.Select(p => new RelatedPlaceModel(p));

			this.RelatedPlaces.AddRange(childPlaces);

			var parentPlaces = place.Relationships.Where(r => r.RelationshipTypeKey == EntityRelationshipTypeKeys.Parent)
									.Select(r => r.TargetEntity)
									.OfType<Place>()
									.Select(p => new RelatedPlaceModel(p));

			this.RelatedPlaces.AddRange(parentPlaces);
		}

		/// <summary>
		/// Gets or sets a list of places related to the place.
		/// </summary>
		public List<RelatedPlaceModel> RelatedPlaces { get; set; }
	}
}