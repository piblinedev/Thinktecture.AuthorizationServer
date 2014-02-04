/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Web.Mvc;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.Models;
using Thinktecture.AuthorizationServer.WebHost.Areas.InitialConfiguration.Models;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.InitialConfiguration.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IAuthorizationServerAdministration _authorizationServerAdministration;

        public HomeController(IAuthorizationServerAdministration authorizationServerAdministration)
        {
            _authorizationServerAdministration = authorizationServerAdministration;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_authorizationServerAdministration.GlobalConfiguration != null)
            {
                filterContext.Result = new RedirectResult("~");
            }
        }

        public ActionResult Index()
        {
            if (_authorizationServerAdministration.GlobalConfiguration != null)
            {
                return Redirect("~/");
            }

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(InitialConfigurationModel model)
        {
            if (_authorizationServerAdministration.GlobalConfiguration != null)
            {
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var global = new GlobalConfiguration
                    {
                        AuthorizationServerName = model.Name,
                        Issuer = model.Issuer,
                        Administrators = new List<AuthorizationServerAdministrator>
                            {
                                new AuthorizationServerAdministrator {NameID = model.Admin}
                            }
                    };
                _authorizationServerAdministration.GlobalConfiguration = global;
                _authorizationServerAdministration.SaveChanges();

                if (model.Test == "test")
                {
                    TestData.Populate();
                }

                return View("Success");
            }

            return View("Index");
        }
    }
}