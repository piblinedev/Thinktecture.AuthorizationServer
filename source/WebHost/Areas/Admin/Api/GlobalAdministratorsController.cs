/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.WebHost.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Api
{
    [ClaimsAuthorize(Constants.Actions.Configure, Constants.Resources.Server)]
    [ValidateHttpAntiForgeryToken]
    public class GlobalAdministratorsController : ApiController
    {
        readonly IAuthorizationServerAdministration _config;

        public GlobalAdministratorsController(IAuthorizationServerAdministration config)
        {
            _config = config;
        }

        public HttpResponseMessage Get()
        {
            var config = _config.GlobalConfiguration;
            var admins = config.Administrators.ToArray();
            return Request.CreateResponse(HttpStatusCode.OK, admins);
        }

        public HttpResponseMessage Post([FromBody] string nameID)
        {
            if (String.IsNullOrEmpty(nameID))
            {
                ModelState.AddModelError("nameID", "Invalid nameID");
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.GetErrors());
            }

            if (_config.GlobalConfiguration.Administrators.Any(x => x.NameID == nameID))
            {
                ModelState.AddModelError("", "That user is already an administrator.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.GetErrors());
            }

            var item = new AuthorizationServer.Models.AuthorizationServerAdministrator { NameID = nameID };
            _config.GlobalConfiguration.Administrators.Add(item);
            _config.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public HttpResponseMessage Delete(int id)
        {
            var item =
                _config.GlobalConfiguration.Administrators.SingleOrDefault(x => x.ID == id);
            if (item != null)
            {
                _config.GlobalConfiguration.Administrators.Remove(item);
                _config.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
