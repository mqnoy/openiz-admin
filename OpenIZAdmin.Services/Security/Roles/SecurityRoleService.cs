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
 * User: nitya
 * Date: 2017-8-3
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIZ.Core.Model.AMI.Auth;
using OpenIZ.Messaging.AMI.Client;
using OpenIZAdmin.Services.Core;

namespace OpenIZAdmin.Services.Security.Roles
{
	/// <summary>
	/// Represents a security role service.
	/// </summary>
	/// <seealso cref="OpenIZAdmin.Services.Core.AmiServiceBase" />
	/// <seealso cref="OpenIZAdmin.Services.Security.Roles.ISecurityRoleService" />
	public class SecurityRoleService : AmiServiceBase, ISecurityRoleService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityRoleService"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		public SecurityRoleService(AmiServiceClient client) : base(client)
		{
		}

		/// <summary>
		/// Gets all roles.
		/// </summary>
		/// <returns>Returns a list of all roles in the system.</returns>
		public IEnumerable<SecurityRoleInfo> GetAllRoles()
		{
			return this.Client.GetRoles(r => r.ObsoletionTime == null).CollectionItem;
		}
	}
}