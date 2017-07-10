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
using System.Web;
using MARC.HI.EHRS.SVC.Auditing.Data;
using OpenIZ.Core.Http;
using OpenIZ.Core.Model.Entities;
using OpenIZ.Core.Model.Roles;

namespace OpenIZAdmin.Audit
{
	/// <summary>
	/// Represents an entity audit helper.
	/// </summary>
	/// <seealso cref="OpenIZAdmin.Audit.AuditHelperBase" />
	public class EntityAuditHelper : HttpContextAuditHelperBase
	{
		/// <summary>
		/// The create entity audit code.
		/// </summary>
		public static readonly AuditCode CreateEntityAuditCode = new AuditCode("EntityCreated", "OpenIZAdminOperations") { DisplayName = "Create" };

		/// <summary>
		/// The delete entity audit code.
		/// </summary>
		public static readonly AuditCode DeleteEntityAuditCode = new AuditCode("EntityDeleted", "OpenIZAdminOperations") { DisplayName = "Delete" };

		/// <summary>
		/// The query entity audit code.
		/// </summary>
		public static readonly AuditCode QueryEntityAuditCode = new AuditCode("EntityQueried", "OpenIZAdminOperations") { DisplayName = "Query" };

		/// <summary>
		/// The update entity audit code.
		/// </summary>
		public static readonly AuditCode UpdateEntityAuditCode = new AuditCode("EntityUpdated", "OpenIZAdminOperations") { DisplayName = "Update" };

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityAuditHelper"/> class.
		/// </summary>
		/// <param name="credentials">The credentials.</param>
		/// <param name="context">The context.</param>
		public EntityAuditHelper(Credentials credentials, HttpContext context) : base(credentials, context)
		{
		}

		/// <summary>
		/// Audits the creation of an entity.
		/// </summary>
		/// <param name="outcomeIndicator">The outcome indicator.</param>
		/// <param name="entity">The entity.</param>
		public void AuditCreateEntity(OutcomeIndicator outcomeIndicator, Entity entity)
		{
			var audit = base.CreateBaseAudit(ActionType.Create, CreateEntityAuditCode, EventIdentifierType.ApplicationActivity, outcomeIndicator);

			if (entity != null)
			{
				var auditableObjectType = AuditableObjectType.Other;

				if (entity is Organization)
				{
					auditableObjectType = AuditableObjectType.Organization;
				}
				else if (entity is Provider || entity is Person)
				{
					auditableObjectType = AuditableObjectType.Person;
				}

				base.AddObjectInfo(audit, AuditableObjectIdType.Custom, AuditableObjectLifecycle.Creation, AuditableObjectRole.Resource, auditableObjectType, "Key", "Key", true, new
				{
					Key = entity.Key.ToString(),
					entity.CreationTime,
					entity.CreatedByKey,
					entity.ClassConceptKey
				});
			}

			this.SendAudit(audit);
		}

		/// <summary>
		/// Audits the deletion of an entity.
		/// </summary>
		/// <param name="outcomeIndicator">The outcome indicator.</param>
		/// <param name="entity">The entity.</param>
		public void AuditDeleteEntity(OutcomeIndicator outcomeIndicator, Entity entity)
		{
			var audit = base.CreateBaseAudit(ActionType.Delete, DeleteEntityAuditCode, EventIdentifierType.ApplicationActivity, outcomeIndicator);

			if (entity != null)
			{
				var auditableObjectType = AuditableObjectType.Other;

				if (entity is Organization)
				{
					auditableObjectType = AuditableObjectType.Organization;
				}
				else if (entity is Provider || entity is Person)
				{
					auditableObjectType = AuditableObjectType.Person;
				}

				base.AddObjectInfo(audit, AuditableObjectIdType.Custom, AuditableObjectLifecycle.LogicalDeletion, AuditableObjectRole.Resource, auditableObjectType, "Key", "Key", true, new
				{
					Key = entity.Key.ToString(),
					entity.CreationTime,
					entity.CreatedByKey,
					entity.ClassConceptKey,
					entity.ObsoletedByKey,
					entity.ObsoletionTime
				});
			}

			this.SendAudit(audit);
		}

		/// <summary>
		/// Audits the query of entities.
		/// </summary>
		/// <param name="outcomeIndicator">The outcome indicator.</param>
		/// <param name="entities">The entities.</param>
		public void AuditQueryEntity(OutcomeIndicator outcomeIndicator, IEnumerable<Entity> entities)
		{
			var audit = base.CreateBaseAudit(ActionType.Read, QueryEntityAuditCode, EventIdentifierType.ApplicationActivity, outcomeIndicator);

			if (entities?.Any() == true)
			{
				base.AddObjectInfoEx(audit, AuditableObjectIdType.Custom, AuditableObjectLifecycle.Disclosure, AuditableObjectRole.Resource, AuditableObjectType.Other, "Key", "Key", true, entities.Select(e => new
				{
					Key = e.Key.ToString(),
					e.CreationTime,
					e.CreatedByKey,
					e.ClassConceptKey
				}));
			}

			this.SendAudit(audit);
		}

		/// <summary>
		/// Audits the update of an entity.
		/// </summary>
		/// <param name="outcomeIndicator">The outcome indicator.</param>
		/// <param name="entity">The entity.</param>
		public void AuditUpdateEntity(OutcomeIndicator outcomeIndicator, Entity entity)
		{
			var audit = base.CreateBaseAudit(ActionType.Update, UpdateEntityAuditCode, EventIdentifierType.ApplicationActivity, outcomeIndicator);

			if (entity != null)
			{
				var auditableObjectType = AuditableObjectType.Other;

				if (entity is Organization)
				{
					auditableObjectType = AuditableObjectType.Organization;
				}
				else if (entity is Provider || entity is Person)
				{
					auditableObjectType = AuditableObjectType.Person;
				}

				base.AddObjectInfo(audit, AuditableObjectIdType.Custom, AuditableObjectLifecycle.Creation, AuditableObjectRole.Resource, auditableObjectType, "Key", "Key", true, new
				{
					Key = entity.Key.ToString(),
					entity.CreationTime,
					entity.CreatedByKey,
					entity.ClassConceptKey,
					entity.ObsoletedByKey,
					entity.ObsoletionTime
				});
			}

			this.SendAudit(audit);
		}
	}
}