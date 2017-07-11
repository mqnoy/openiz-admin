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
 * Date: 2017-7-9
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIZ.Core.Http;
using OpenIZ.Messaging.AMI.Client;

namespace OpenIZAdmin.Services.Core
{
	/// <summary>
	/// Represents a base service.
	/// </summary>
	public abstract class AmiServiceBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AmiServiceBase"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		protected AmiServiceBase(AmiServiceClient client)
		{
			this.Client = client;
		}

		/// <summary>
		/// Gets the client.
		/// </summary>
		/// <value>The client.</value>
		protected AmiServiceClient Client { get; }
	}
}