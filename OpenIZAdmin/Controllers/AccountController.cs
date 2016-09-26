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
 * Date: 2016-5-31
 */

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OpenIZ.Core.Model.Constants;
using OpenIZ.Core.Model.DataTypes;
using OpenIZ.Core.Model.Entities;
using OpenIZ.Messaging.AMI.Client;
using OpenIZ.Messaging.IMSI.Client;
using OpenIZAdmin.DAL;
using OpenIZAdmin.Localization;
using OpenIZAdmin.Models.AccountModels;
using OpenIZAdmin.Services.Http;
using OpenIZAdmin.Services.Http.Security;
using OpenIZAdmin.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OpenIZAdmin.Controllers
{
	/// <summary>
	/// Provides operations to manage an account.
	/// </summary>
	[Authorize]
	public class AccountController : Controller
	{
		/// <summary>
		/// The internal reference to the <see cref="AmiServiceClient"/> instance.
		/// </summary>
		private AmiServiceClient amiClient;

		/// <summary>
		/// The internal reference to the <see cref="ImsiServiceClient"/> instance.
		/// </summary>
		private ImsiServiceClient imsiClient;

		/// <summary>
		/// The internal reference to the <see cref="ApplicationUserManager"/> instance.
		/// </summary>
		private ApplicationSignInManager signInManager;

		/// <summary>
		/// The internal reference to the <see cref="ApplicationSignInManager"/> instance.
		/// </summary>
		private ApplicationUserManager userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class.
		/// </summary>
		public AccountController()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class
		/// with a specified user manager instance and sign in manager instance.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		/// <summary>
		/// Gets the sign in manager.
		/// </summary>
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				signInManager = value;
			}
		}

		/// <summary>
		/// Gets the user manager.
		/// </summary>
		public ApplicationUserManager UserManager
		{
			get
			{
				return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				userManager = value;
			}
		}

		/// <summary>
		/// Changes the password of a user.
		/// </summary>
		/// <param name="model">The model containing the users new password.</param>
		/// <returns>Returns an action result.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var userId = Guid.Parse(User.Identity.GetUserId());

				try
				{
					var user = this.amiClient.GetUser(userId.ToString());

					if (user != null && !user.Lockout.GetValueOrDefault(false))
					{
						user.Password = model.Password;
						user = this.amiClient.UpdateUser(userId, user);
					}

					TempData["success"] = Locale.PasswordChangedSuccessfully;

					return RedirectToAction("Manage");
				}
				catch (Exception e)
				{
#if DEBUG
					Trace.TraceError("Unable to change user's password", e.StackTrace);
#endif
					Trace.TraceError("Unable to change user's password", e.Message);
				}
			}

			TempData["error"] = Locale.UnableToChangePassword;

			return RedirectToAction("Manage");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (userManager != null)
				{
					userManager.Dispose();
					userManager = null;
				}

				if (signInManager != null)
				{
					signInManager.Dispose();
					signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Displays the login view.
		/// </summary>
		/// <param name="returnUrl">The return url for an unauthenticated user.</param>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (User.Identity.IsAuthenticated && RealmConfig.IsJoinedToRealm())
			{
				return RedirectToAction("Index", "Home");
			}
			else if (!RealmConfig.IsJoinedToRealm())
			{
				return RedirectToAction("JoinRealm", "Realm");
			}

			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		/// <summary>
		/// Logs in the user.
		/// </summary>
		/// <param name="model">The login model containing a username and password.</param>
		/// <param name="returnUrl">The return url to redirect to once the user is authenticated.</param>
		/// <returns>Returns a <see cref="System.Threading.Tasks.Task"/> with the result of the login action.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, shouldLockout: false);

			switch (result)
			{
				case SignInStatus.Success:
					Response.Cookies.Add(new HttpCookie("access_token", SignInManager.AccessToken));
					return RedirectToLocal(returnUrl);

				case SignInStatus.LockedOut:
					return View("Lockout");

				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", Locale.IncorrectUsernameOrPassword);
					return View(model);
			}
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult Manage()
		{
			ManageModel model = new ManageModel();

			var userId = Guid.Parse(User.Identity.GetUserId());

			UserEntity user = null;

			try
			{
				var bundle = this.imsiClient.Query<UserEntity>(u => u.SecurityUserKey.Value == userId);

				bundle.Reconstitute();

				user = bundle.Item.OfType<UserEntity>().Cast<UserEntity>().FirstOrDefault();
			}
			catch (Exception e)
			{
#if DEBUG
				Trace.TraceError("Unable to retrieve user entity", e.StackTrace);
#endif
				Trace.TraceError("Unable to retrieve user entity", e.Message);
			}

			if (user == null)
			{
				user = new UserEntity();
			}

			var nameComponents = user.Names.SelectMany(n => n.Component);

			List<SelectListItem> facilityList = new List<SelectListItem>();

			facilityList.Add(new SelectListItem
			{
				Text = "",
				Value = ""
			});

			var places = PlaceUtil.GetPlaces(this.imsiClient, 0, 200);

			facilityList.AddRange(places.Select(p => new SelectListItem { Text = string.Join(" ", p.Names.SelectMany(n => n.Component).Select(c => c.Value)), Value = p.Key.Value.ToString() }));

			model.UpdateProfileModel.FacilityList = facilityList.OrderBy(c => c.Text).ToList();

			model.UpdateProfileModel.FamilyNameList = nameComponents.Where(c => c.ComponentType.Key == NameComponentKeys.Family).Select(c => new SelectListItem { Text = c.Value, Value = c.Value, Selected = true }).ToList();
			model.UpdateProfileModel.GivenNamesList = nameComponents.Where(c => c.ComponentType.Key == NameComponentKeys.Given).Select(c => new SelectListItem { Text = c.Value, Value = c.Value, Selected = true }).ToList();

			model.UpdateProfileModel.Language = user.LanguageCommunication.Select(l => l.Key).FirstOrDefault().GetValueOrDefault(Guid.Empty);

			model.UpdateProfileModel.LanguageList = new List<SelectListItem>
			{
				new SelectListItem
				{
					Text = "",
					Value = ""
				},
				new SelectListItem
				{
					Text = "English",
					// TODO: add the GUID for english here
					Value = Guid.NewGuid().ToString()
				},
				new SelectListItem
				{
					Text = "Kiswahili",
					// TODO: add the GUID for Kiswahili here
					Value = Guid.NewGuid().ToString()
				}
			};

			return View(model);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var amiRestClient = new RestClientService(Constants.AMI);

			amiRestClient.Accept = "application/xml";
			amiRestClient.Credentials = new AmiCredentials(this.User, HttpContext.Request);

			this.amiClient = new AmiServiceClient(amiRestClient);

			var imsiRestClient = new RestClientService(Constants.IMSI);

			imsiRestClient.Accept = "application/xml";
			imsiRestClient.Credentials = new ImsCredentials(this.User, HttpContext.Request);

			this.imsiClient = new ImsiServiceClient(imsiRestClient);

			base.OnActionExecuting(filterContext);
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// Updates a user's profile.
		/// </summary>
		/// <param name="model">The model containing the user profile information to be updated.</param>
		/// <returns>Returns the index view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProfile(UpdateProfileModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userId = Guid.Parse(User.Identity.GetUserId());

					UserEntity userEntity = null;

					var bundle = this.imsiClient.Query<UserEntity>(u => u.SecurityUserKey.Value == userId);

					bundle.Reconstitute();

					userEntity = bundle.Item.OfType<UserEntity>().Cast<UserEntity>().FirstOrDefault();

					if (userEntity == null)
					{
						using (IUnitOfWork unitOfWork = new EntityUnitOfWork())
						{
							// delete the user from the database incase there is an unknown error which occurred previously
							// this will allow the user to login again to this system at some point
							unitOfWork.UserRepository.Delete(User.Identity.GetUserId());
							unitOfWork.Save();
						}

						// somehow the user is on a screen where they are able to edit a profile but do not exist as a user
						// therefore, as a security concern we will log them out
						HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

						TempData["error"] = Locale.SessionTimedOut;
						return RedirectToAction("Login", "Account");
					}

					userEntity.AsSecurityUser.PhoneNumber = model.PhoneNumber;

					EntityName name = new EntityName();

					name.NameUse = new Concept
					{
						Key = NameUseKeys.OfficialRecord
					};

					name.Component = new List<EntityNameComponent>();

					if (model.FamilyNames != null && model.FamilyNames.Count > 0)
					{
						name.Component.AddRange(model.FamilyNames.Select(n => new EntityNameComponent(NameComponentKeys.Family, n)));
					}

					if (model.GivenNames != null && model.GivenNames.Count > 0)
					{
						name.Component.AddRange(model.GivenNames.Select(n => new EntityNameComponent(NameComponentKeys.Given, n)));
					}

					userEntity.Names = new List<EntityName>();

					userEntity.Names.Add(name);

					userEntity.Relationships.Add(new EntityRelationship
					{
						RelationshipType = new Concept
						{
							Key = EntityRelationshipTypeKeys.DedicatedServiceDeliveryLocation
						},
						TargetEntity = new Place
						{
							Key = Guid.Parse(model.FacilityId)
						}
					});

					userEntity.SecurityUserKey = Guid.Parse(User.Identity.GetUserId());

					this.imsiClient.Update<UserEntity>(userEntity);

					TempData["success"] = Locale.ProfileUpdatedSuccessfully;

					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
#if DEBUG
					Trace.TraceError("Unable to update profile: {0}", e.StackTrace);
#endif
					Trace.TraceError("Unable to update profile: {0}", e.Message);
				}
			}

			TempData["error"] = Locale.UnableToUpdateProfile;

			return View(model);
		}
	}
}