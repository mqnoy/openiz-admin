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
 * User: yendtr
 * Date: 2016-7-23
 */

using Elmah;
using OpenIZ.Core.Model.Constants;
using OpenIZ.Core.Model.DataTypes;
using OpenIZAdmin.Attributes;
using OpenIZAdmin.Extensions;
using OpenIZAdmin.Localization;
using OpenIZAdmin.Models.ConceptModels;
using OpenIZAdmin.Models.ReferenceTermModels;
using OpenIZAdmin.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using OpenIZ.Core.Model.Query;
using OpenIZAdmin.Models.LanguageModels;

namespace OpenIZAdmin.Controllers
{
    /// <summary>
    /// Provides operations for managing concepts.
    /// </summary>
    [TokenAuthorize]
	public class ConceptController : MetadataController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConceptController"/> class.
		/// </summary>
		public ConceptController()
		{
		}      

        /// <summary>
        /// Displays the create view.
        /// </summary>
        /// <returns>Returns the create view.</returns>
        [HttpGet]
		public ActionResult Create()
        {            
            var model = new CreateConceptModel
			{                
				ConceptClassList = this.GetConceptClasses().ToSelectList().OrderBy(c => c.Text).ToList(),
				Language = Locale.EN,
				LanguageList = LanguageUtil.GetLanguageList().ToSelectList("DisplayName", "TwoLetterCountryCode").ToList()                
            };

			return View(model);
		}

		/// <summary>
		/// Creates a concept.
		/// </summary>
		/// <param name="model">The model containing the information to create a concept.</param>
		/// <returns>Returns the created concept.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateConceptModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var concept = this.ImsiClient.Create(model.ToConcept());

					TempData["success"] = Locale.Concept + " " + Locale.Created + " " + Locale.Successfully;

					return RedirectToAction("ViewConcept", new { id = concept.Key, versionId = concept.VersionKey });
				}
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to create concept: {e}");
			}

			TempData["error"] = Locale.UnableToCreate + " " + Locale.Concept;

			Guid conceptClass;
			model.LanguageList = LanguageUtil.GetLanguageList().ToSelectList("DisplayName", "TwoLetterCountryCode").ToList();
			model.ConceptClassList = Guid.TryParse(model.ConceptClass, out conceptClass) ? this.GetConceptClasses().ToSelectList("Name", "Key", c => c.Key == conceptClass).OrderBy(c => c.Text).ToList() : this.GetConceptClasses().ToSelectList("Name", "Key").OrderBy(c => c.Text).ToList();

			return View(model);
		}

		/// <summary>
		/// Deletes a concept.
		/// </summary>
		/// <param name="id">The id of the concept to delete.</param>
		/// <param name="versionId">The version identifier of the Concept instance.</param>
		/// <param name="refTermId">The Reference Term identifier</param>
		/// <returns>Returns the index view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteReferenceTerm(Guid id, Guid? versionId, Guid? refTermId)
		{
			try
			{
				var concept = this.ImsiClient.Get<Concept>(id, versionId) as Concept;

				if (concept == null)
				{
					TempData["error"] = Locale.Concept + " " + Locale.NotFound;
					return RedirectToAction("Index");
				}

				concept.ReferenceTerms.RemoveAll(c => c.ReferenceTermKey == refTermId);

				var result = this.ImsiClient.Update(concept);

				TempData["success"] = Locale.Concept + " " + Locale.ReferenceTerm + " " + Locale.Deleted + " " + Locale.Successfully;

				return RedirectToAction("Edit", new { id = result.Key, versionId = result.VersionKey });
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to delete reference term from concept: {e}");
			}

			TempData["error"] = Locale.UnableToDelete + " " + Locale.ReferenceTerm;

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edits the specified concept.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="versionId">The version identifier(Guid) of the Concept instance.</param>
		/// <returns>ActionResult.</returns>
		[HttpGet]
		public ActionResult Edit(Guid id, Guid? versionId)
		{
			try
			{
				var concept = this.GetConcept(id, versionId);

				if (concept == null)
				{
					TempData["error"] = Locale.Concept + " " + Locale.NotFound;
					return RedirectToAction("Index");
				}

				var model = new EditConceptModel(concept)
				{
					LanguageList = LanguageUtil.GetLanguageList().ToSelectList("DisplayName", "TwoLetterCountryCode").ToList()
				};			    

				var conceptClasses = this.GetConceptClasses();
				model.ConceptClassList.AddRange(conceptClasses.ToSelectList().OrderBy(c => c.Text));

				if (concept.Class?.Key != null)
				{
					var selectedClass = conceptClasses.FirstOrDefault(c => c.Key == concept.Class.Key);
					model.ConceptClass = selectedClass?.Key.ToString();
				}

				model.ReferenceTerms = this.GetConceptReferenceTerms(id, versionId).Select(r => new ReferenceTermViewModel(r, concept)).ToList();                

                return View(model);
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to retrieve concept: {e}");
				this.TempData["error"] = Locale.Concept + " " + Locale.NotFound;
			}

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Updates a Concept from the <see cref="EditConceptModel"/> instance.
		/// </summary>
		/// <param name="model">The EditConceptModel instance</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditConceptModel model)
		{
			try
			{
                var concept = this.GetConcept(model.Id, model.VersionKey);

                if (concept == null)
                {
                    TempData["error"] = Locale.Concept + " " + Locale.NotFound;

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
			    {			        
			        if (!string.Equals(concept.Mnemonic, model.Mnemonic) && !DoesConceptExist(model.Mnemonic))
			        {
			            TempData["error"] = Locale.Mnemonic + " " + Locale.MustBeUnique;
			            return View(model);
			        }

			        concept = model.ToEditConceptModel(ImsiClient, concept);

			        var result = this.ImsiClient.Update<Concept>(concept);

			        TempData["success"] = Locale.Concept + " " + Locale.Updated + " " + Locale.Successfully;

			        return RedirectToAction("ViewConcept", new {id = result.Key, versionId = result.VersionKey});
			    }			    

                var conceptClasses = this.GetConceptClasses();
                model.ConceptClassList.AddRange(conceptClasses.ToSelectList().OrderBy(c => c.Text));

                if (concept.Class?.Key != null)
                {
                    var selectedClass = conceptClasses.FirstOrDefault(c => c.Key == concept.Class.Key);
                    model.ConceptClass = selectedClass?.Key.ToString();
                }

                model.ReferenceTerms = this.GetConceptReferenceTerms(concept.Key.Value, concept.VersionKey).Select(r => new ReferenceTermViewModel(r, concept)).ToList();
                model.Languages = concept.ConceptNames.Select(k => new LanguageViewModel(k.Language, k.Name, concept)).ToList();

            }
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to update concept: {e}");
			}

			TempData["error"] = Locale.UnableToUpdate + " " + Locale.Concept;                        

            return View(model);
		}

		/// <summary>
		/// Displays the index view.
		/// </summary>
		/// <returns>Returns the index view.</returns>
		public ActionResult Index()
		{
			TempData["searchType"] = "Concept";
            TempData["searchTerm"] = "*";
            return View();
		}

		/// <summary>
		/// Displays the search view.
		/// </summary>
		/// <returns>Returns the search view.</returns>
		[HttpGet]
		[ValidateInput(false)]
		public ActionResult Search(string searchTerm)
		{            
            var results = new List<ConceptViewModel>();

            try
			{
				if (this.IsValidId(searchTerm))
				{					
					var bundle = searchTerm == "*" ? this.ImsiClient.Query<Concept>(c => c.ObsoletionTime == null) : this.ImsiClient.Query<Concept>(c => c.Mnemonic.Contains(searchTerm) && c.ObsoletionTime == null);

                    results = bundle.Item.OfType<Concept>().LatestVersionOnly().Select(p => new ConceptViewModel(p)).ToList();

                    TempData["searchTerm"] = searchTerm;

					return PartialView("_ConceptSearchResultsPartial", results.OrderBy(c => c.Mnemonic));
				}
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to load concepts: {e}");
			}

			TempData["error"] = Locale.InvalidSearch;
			TempData["searchTerm"] = searchTerm;

			return PartialView("_ConceptSearchResultsPartial", results);
		}

        /// <summary>
		/// Searches for a user.
		/// </summary>
		/// <param name="searchTerm">The search term.</param>
		/// <returns>Returns a list of users which match the search term.</returns>
		[HttpGet]
        public ActionResult SearchAjax(string searchTerm)
        {
            var viewModels = new List<ReferenceTermViewModel>();

            var query = new List<KeyValuePair<string, object>>();

            query.AddRange(QueryExpressionBuilder.BuildQuery<ReferenceTerm>(c => c.Mnemonic.Contains(searchTerm)));
            query.AddRange(QueryExpressionBuilder.BuildQuery<ReferenceTerm>(c => c.ObsoletionTime == null));

            var bundle = this.ImsiClient.Query<ReferenceTerm>(QueryExpressionParser.BuildLinqExpression<ReferenceTerm>(new NameValueCollection(query.ToArray())));

            viewModels.AddRange(bundle.Item.OfType<ReferenceTerm>().Select(c => new ReferenceTermViewModel(c)));

            return Json(viewModels.OrderBy(c => c.Mnemonic).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieves the Concept by identifier
        /// </summary>
        /// <param name="id">The identifier of the Concept</param>
        /// <param name="versionId">The version identifier (Guid) of the concept instance.</param>
        /// <returns>Returns the concept view.</returns>
        [HttpGet]
		public ActionResult ViewConcept(Guid id, Guid? versionId)
		{
			try
			{
				var concept = this.GetConcept(id, versionId);

				if (concept == null)
				{
					TempData["error"] = Locale.Concept + " " + Locale.NotFound;

					return RedirectToAction("Index");
				}

				var model = new ConceptViewModel(concept)
				{
					ReferenceTerms = this.GetConceptReferenceTerms(id, versionId).Select(r => new ReferenceTermViewModel(r)).ToList()
				};

				return View(model);
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
				Trace.TraceError($"Unable to load concept: {e}");
			}

			TempData["error"] = Locale.Concept + " " + Locale.NotFound;

			return RedirectToAction("Index");
		}

        [HttpGet]
        public JsonResult HasReferenceTerm(EditConceptModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.AddReferenceTerm) && !string.Equals(model.AddReferenceTerm, "null")) return Json(true, JsonRequestBehavior.AllowGet); // indicates its valid
                        
            return Json(false, JsonRequestBehavior.AllowGet);             
        }

        [HttpGet]
        public JsonResult HasRelationshipType(EditConceptModel model)
        {            
            if (!string.IsNullOrWhiteSpace(model.RelationshipType) && !string.Equals(model.RelationshipType, "null")) return Json(true, JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}