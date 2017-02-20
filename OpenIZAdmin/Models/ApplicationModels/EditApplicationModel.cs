﻿using OpenIZ.Core.Model.AMI.Auth;
using OpenIZAdmin.Models.PolicyModels.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace OpenIZAdmin.Models.ApplicationModels
{
	public class EditApplicationModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditApplicationModel"/> class.
		/// </summary>
		public EditApplicationModel()
		{
			this.Policies = new List<string>();
			this.PoliciesList = new List<SelectListItem>();
		}

		public EditApplicationModel(SecurityApplicationInfo securityApplicationInfo) : this()
		{
			this.ApplicationName = securityApplicationInfo.Application.Name;
			this.ApplicationPolicies = securityApplicationInfo.Policies.Select(p => new PolicyViewModel(p)).OrderBy(p => p.Name).ToList();
			this.CreationTime = securityApplicationInfo.Application.CreationTime.DateTime;
			this.HasPolicies = this.ApplicationPolicies.Any();
			this.Id = securityApplicationInfo.Id.Value;
			this.Policies = this.ApplicationPolicies.Select(p => p.Id.ToString()).ToList();
			this.IsObsolete = securityApplicationInfo.Application.ObsoletionTime != null;
		}

		[Display(Name = "AddPolicies", ResourceType = typeof(Localization.Locale))]
		public List<string> AddPolicies { get; set; }

		[Display(Name = "ApplicationName", ResourceType = typeof(Localization.Locale))]
		public string ApplicationName { get; set; }

		public IEnumerable<PolicyViewModel> ApplicationPolicies { get; set; }

		[Display(Name = "CreationTime", ResourceType = typeof(Localization.Locale))]
		public DateTime CreationTime { get; set; }

		[Display(Name = "HasPolicies", ResourceType = typeof(Localization.Locale))]
		public bool HasPolicies { get; set; }

		[Required]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets whether the application is obsolete.
		/// </summary>
		public bool IsObsolete { get; set; }

		[Display(Name = "Policies", ResourceType = typeof(Localization.Locale))]
		public List<string> Policies { get; set; }

		public List<SelectListItem> PoliciesList { get; set; }
	}
}