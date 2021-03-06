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
 * Date: 2016-7-14
 */

using OpenIZAdmin.Models.Domain;
using System;
using System.Threading.Tasks;

namespace OpenIZAdmin.DAL
{
	/// <summary>
	/// Defines repositories for accessing data.
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		#region Repositories

		/// <summary>
		/// Gets the manual repository.
		/// </summary>
		/// <value>The manual repository.</value>
		IRepository<Manual> ManualRepository { get; }

		/// <summary>
		/// The repository for accessing realms.
		/// </summary>
		IRepository<Realm> RealmRepository { get; }

		/// <summary>
		/// The repository for accessing users.
		/// </summary>
		IRepository<ApplicationUser> UserRepository { get; }

		#endregion Repositories

		/// <summary>
		/// Save any pending changes to the database.
		/// </summary>
		void Save();

		/// <summary>
		/// Save any pending changes to the database.
		/// </summary>
		/// <returns>Returns a task.</returns>
		Task SaveAsync();
	}
}