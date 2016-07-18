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
 * Date: 2016-7-8
 */

using OpenIZAdmin.Models.AppletModels.ViewModels;
using OpenIZAdmin.Models.CertificateModels.ViewModels;
using OpenIZAdmin.Models.DeviceModels.ViewModels;
using OpenIZAdmin.Models.RoleModels.ViewModels;
using OpenIZAdmin.Models.UserAdministration.ViewModels;
using OpenIZAdmin.Models.UserModels.ViewModels;
using System.Collections.Generic;

namespace OpenIZAdmin.Models
{
	public class DashboardViewModel
	{
		public DashboardViewModel()
		{
		}

		public IEnumerable<AppletViewModel> Applets { get; set; }

		public IEnumerable<CertificateSigningRequestViewModel> CertificateRequests { get; set; }

		public IEnumerable<DeviceViewModel> Devices { get; set; }

		public IEnumerable<RoleViewModel> Roles { get; set; }

		public IEnumerable<UserRoleViewModel> UserRoles { get; set; }

		public IEnumerable<UserViewModel> Users { get; set; }
	}
}