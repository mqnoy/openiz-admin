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
using OpenIZ.Core.Model.DataTypes;
using OpenIZ.Core.Model.Query;
using OpenIZAdmin.Attributes;
using OpenIZAdmin.Extensions;
using OpenIZAdmin.Localization;
using OpenIZAdmin.Models.ConceptModels;
using OpenIZAdmin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OpenIZAdmin.Models.LanguageModels;
using System.Diagnostics;

namespace OpenIZAdmin.Controllers
{
	/// <summary>
	/// Provides operations for managing concepts.
	/// </summary>
	[TokenAuthorize]
	public class ConceptController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConceptController"/> class.
		/// </summary>
		public ConceptController()
		{
		}

        /// <summary>
		/// Activates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="versionId">The version identifier.</param>
		/// <returns>ActionResult.</returns>
		public ActionResult Activate(Guid id, Guid? versionId)
        {
            try
            {
                var concept = ImsiClient.Get<Concept>(id, null) as Concept;

                if (concept == null)
                {
                    TempData["error"] = Locale.Concept + " " + Locale.NotFound;
                    return RedirectToAction("Index");
                }

                concept.CreationTime = DateTimeOffset.Now;
                concept.ObsoletedByKey = null;
                concept.ObsoletionTime = null;
                concept.VersionKey = null;

                var result = ImsiClient.Update(concept);

                TempData["success"] = Locale.Concept + " " + Locale.Activated + " " + Locale.Successfully;

                return RedirectToAction("ViewConcept", new { id = result.Key, versionId = result.VersionKey });
            }
            catch (Exception e)
            {
                ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
                Trace.TraceError($"Unable to activate concept: { e }");
            }

            TempData["error"] = Locale.UnableToActivate + " " + Locale.Material;

            return RedirectToAction("Edit", new { id = (Guid?) id, versionId });
        }

        /// <summary>
        /// Displays the create view.
        /// </summary>
        /// <returns>Returns the create view.</returns>
        [HttpGet]
		public ActionResult Create()
		{            
            var model = new CreateConceptModel()
            {
                LanguageList = LanguageUtil.GetSelectListItemLanguageList().ToList()                
            };

            var conceptClasses = ConceptUtil.GetConceptClasses(ImsiClient);
            model.ConceptClassList.AddRange(conceptClasses.ToSelectList().OrderBy(c => c.Text));
		    model.Language = Locale.EN;
            		    
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
					var concept = this.ImsiClient.Create<Concept>(model.ToConcept());

					TempData["success"] = Locale.Concept + " " + Locale.Created + " " + Locale.Successfully;

					return RedirectToAction("ViewConcept", new { id = concept.Key = concept.VersionKey });
                }
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
			}

			TempData["error"] = Locale.UnableToCreate + " " + Locale.Concept;
			
		    model.LanguageList = LanguageUtil.GetSelectListItemLanguageList().ToList();

            return View(model);
		}

		/// <summary>
		/// Deletes a concept.
		/// </summary>
		/// <param name="id">The id of the concept to delete.</param>
		/// <returns>Returns the index view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(Guid id)
		{
			try
			{
				var concept = this.ImsiClient.Get<Concept>(id, null) as Concept;

				if (concept == null)
				{
					TempData["error"] = Locale.Concept + " " + Locale.NotFound;
					return RedirectToAction("Index");
				}

				this.ImsiClient.Obsolete<Concept>(concept);

				TempData["success"] = Locale.Concept + " " + Locale.Deleted + " " + Locale.Successfully;

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
			}

			TempData["error"] = Locale.Concept + " " + Locale.NotFound;

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
		    var concept = ConceptUtil.GetConcept(ImsiClient, id, versionId);

            if (concept == null)
            {
                TempData["error"] = Locale.Concept + " " + Locale.NotFound;
                return RedirectToAction("Index");
            }                       

            var model = new EditConceptModel(concept)
            {
                LanguageList = LanguageUtil.GetSelectListItemLanguageList().ToList()
            };

            var conceptClasses = ConceptUtil.GetConceptClasses(ImsiClient).ToList();
            model.ConceptClassList.AddRange(conceptClasses.ToSelectList().OrderBy(c => c.Text));

		    if (concept.Class != null)
		    {
		        var selectedClass = conceptClasses.FirstOrDefault(c => c.Key == concept.Class.Key);
                if (selectedClass != null)
                {
                    model.ConceptClass = selectedClass.Key.ToString();
                }
            }            
		                  
            var referenceTerms = ConceptUtil.GetConceptReferenceTerms(ImsiClient, concept);

		    if (referenceTerms != null)
		    {
                model.ReferenceTerms.AddRange(referenceTerms.Select(r => new ReferenceTermModel
		        {
		            Mnemonic = r.Mnemonic,
		            Name = string.Join(" ", r.DisplayNames.Select(d => d.Name)),
		            Id = r.Key.Value
		        }));
		    }		   

		    return View(model);
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
			if (ModelState.IsValid)
			{
                var concept = ConceptUtil.GetConcept(ImsiClient, model.Id, null);

                if (concept == null)
                {
                    TempData["error"] = Locale.Concept + " " + Locale.NotFound;

                    return RedirectToAction("Index");
                }
			    
                if (!string.Equals(concept.Mnemonic, model.Mnemonic) && !ConceptUtil.CheckUniqueConceptMnemonic(ImsiClient, model.Mnemonic))
			    {
                    TempData["error"] = Locale.Mnemonic + " " + Locale.MustBeUnique;
			        return View(model);
			    }                                                					    

			    concept = ConceptUtil.ToEditConceptInfo(model, concept);										    

                var result = this.ImsiClient.Update<Concept>(concept);

				TempData["success"] = Locale.Concept + " " + Locale.Updated + " " + Locale.Successfully;

				return RedirectToAction("Edit", new { id = result.Key });
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
			return View();
		}

		/// <summary>
		/// Displays the search view.
		/// </summary>
		/// <returns>Returns the search view.</returns>
		[HttpGet]
		public ActionResult Search(string searchTerm)
		{
			var viewModels = new List<ConceptSearchResultViewModel>();


            //IEnumerable<MaterialViewModel> results = new List<MaterialViewModel>();

            //if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrWhiteSpace(searchTerm))
            //{
            //    var bundle = this.ImsiClient.Query<Material>(p => p.Names.Any(n => n.Component.Any(c => c.Value.Contains(searchTerm))) && p.ClassConceptKey == EntityClassKeys.Material);

            //    results = bundle.Item.OfType<Material>().Where(p => p.Names.Any(n => n.Component.Any(c => c.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))).LatestVersionOnly().Select(p => new MaterialViewModel(p)).OrderBy(p => p.Name).ToList();
            //}

            //if (CommonUtil.IsValidString(searchTerm))
            //{
            //    var conceptBundle = this.ImsiClient.Query<Concept>(c => c.Mnemonic.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            //    var results = conceptBundle.Item.OfType<Concept>().LatestVersionOnly().Select(p => new ConceptSearchResultViewModel(p)).OrderBy(p => p.Mnemonic).ToList();

            //    viewModels.AddRange(conceptBundle.Item.OfType<Concept>().Select(c => new ConceptSearchResultViewModel(c)));

            //    TempData["searchTerm"] = searchTerm;

            //    return PartialView("_ConceptSearchResultsPartial", viewModels.OrderBy(c => c.Mnemonic));
            //}


            if (CommonUtil.IsValidString(searchTerm))
            {
                var conceptBundle = this.ImsiClient.Query<Concept>(c => c.Mnemonic.Contains(searchTerm) && c.ObsoletionTime == null);

                viewModels.AddRange(conceptBundle.Item.OfType<Concept>().Select(c => new ConceptSearchResultViewModel(c)));

                TempData["searchTerm"] = searchTerm;

                return PartialView("_ConceptSearchResultsPartial", viewModels.OrderBy(c => c.Mnemonic));
            }
            else
			{
				TempData["error"] = Locale.InvalidSearch;
			}

			TempData["searchTerm"] = searchTerm;

			return PartialView("_ConceptSearchResultsPartial", viewModels);
		}

	    /// <summary>
	    /// Retrieves the Concept by identifier
	    /// </summary>
	    /// <param name="id">The identifier of the Concept</param>
	    /// <param name="versionKey">The version identifier (Guid) of the concept instance.</param>
	    /// <returns></returns>
	    [HttpGet]
		public ActionResult ViewConcept(Guid id, Guid? versionKey)
		{
			try
			{				
			    var concept = ConceptUtil.GetConcept(ImsiClient, id, versionKey);

				if (concept == null)
				{
					TempData["error"] = Locale.Concept + " " + Locale.NotFound;

					return RedirectToAction("Index");
				}
				
				var conceptViewModel = new ConceptViewModel(concept);

				for (var i = 0; i < concept.ReferenceTerms.Count(r => r.ReferenceTerm == null && r.RelationshipTypeKey.HasValue); i++)
				{
					concept.ReferenceTerms[i].ReferenceTerm = this.ImsiClient.Get<ReferenceTerm>(concept.ReferenceTerms[i].ReferenceTermKey.Value, null) as ReferenceTerm;
				}

				conceptViewModel.ReferenceTerms.AddRange(concept.ReferenceTerms.Select(r => new ReferenceTermModel
				{
					Mnemonic = r.ReferenceTerm?.Mnemonic,
					Name = string.Join(", ", r.ReferenceTerm.DisplayNames.Select(d => d.Name)),
					Id = r.Key.Value
				}));

				return View(conceptViewModel);
			}
			catch (Exception e)
			{
				ErrorLog.GetDefault(HttpContext.ApplicationInstance.Context).Log(new Error(e, HttpContext.ApplicationInstance.Context));
			}

			TempData["error"] = Locale.Concept + " " + Locale.NotFound;

			return RedirectToAction("Index");
		}
	}
}