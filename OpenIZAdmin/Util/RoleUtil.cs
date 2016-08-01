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
 * Date: 2016-7-30
 */

using OpenIZ.Core.Model.AMI.Auth;
using OpenIZ.Messaging.AMI.Client;
using OpenIZAdmin.Models.RoleModels;
using OpenIZAdmin.Models.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenIZAdmin.Util
{
	internal static class RoleUtil
	{
		internal static IEnumerable<RoleViewModel> GetAllRoles(AmiServiceClient client)
		{
			IEnumerable<RoleViewModel> viewModels = new List<RoleViewModel>();

			try
			{
				// HACK
				var roles = client.GetRoles(r => r.Name != null);

				viewModels = roles.CollectionItem.Select(r => RoleUtil.ToRoleViewModel(r));
			}
			catch (Exception e)
			{
#if DEBUG
				Trace.TraceError("Unable to retrieve roles: {0}", e.StackTrace);
#endif
				Trace.TraceError("Unable to retrieve roles: {0}", e.Message);
			}

			return viewModels;
		}

		internal static RoleViewModel ToRoleViewModel(SecurityRoleInfo roleInfo)
		{
			RoleViewModel viewModel = new RoleViewModel();

			viewModel.Description = roleInfo.Role.Description;
			viewModel.Id = roleInfo.Id.Value;
			viewModel.Name = roleInfo.Name;

			return viewModel;
		}

		internal static SecurityRoleInfo ToSecurityRoleInfo(CreateRoleModel model)
		{
			SecurityRoleInfo roleInfo = new SecurityRoleInfo();

			roleInfo.Name = model.Name;

			return roleInfo;
		}
	}
}