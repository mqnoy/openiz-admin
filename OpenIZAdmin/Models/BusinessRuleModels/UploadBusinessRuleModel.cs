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
 * User: khannan
 * Date: 2016-11-29
 */
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OpenIZAdmin.Models.BusinessRuleModels
{
	/// <summary>
	/// Represents a model to upload an applet.
	/// </summary>
	public class UploadBusinessRuleModel
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadBusinessRuleModel"/> class.
        /// </summary>
        public UploadBusinessRuleModel()
		{
		}

		/// <summary>
		/// Gets or sets the applet content.
		/// </summary>
		[Required]
		//[FileExtensions(Extensions = ".pak.gz, .pak, .gz", ErrorMessage = "Unsupported file format, the allow file types are .pak.gz, .gz, .pak")]
		public HttpPostedFileBase File { get; set; }
	}
}