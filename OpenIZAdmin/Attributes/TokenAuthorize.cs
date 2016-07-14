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
 * Date: 2016-7-10
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenIZAdmin.Attributes
{
	/// <summary>
	/// Validates against whether a user accessing a resource has the correct permissions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class TokenAuthorize : AuthorizeAttribute
	{
		public TokenAuthorize()
		{

		}

		/// <summary>
		/// Determines whether the current <see cref="System.Security.Principal.IPrincipal"/> is authorized to access the requested resources.
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		/// <returns>Returns true if the current user is authorized to access the request resources.</returns>
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool isAuthorized = false;

			var accessToken = httpContext.Request.Cookies["access_token"]?.Value;

			if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrWhiteSpace(accessToken))
			{
				try
				{
					JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

					// is the token expired?
					if (securityToken.ValidTo <= DateTime.UtcNow)
					{
						isAuthorized = false;
					}
					else
					{
						isAuthorized = true;
					}
				}
				catch (Exception e)
				{
					isAuthorized = false;
#if DEBUG
					Trace.TraceError(e.StackTrace);
#endif
					Trace.TraceError(e.Message);
				}
			}

			return base.AuthorizeCore(httpContext) && isAuthorized;
		}
	}
}