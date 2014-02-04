/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.Models;
using Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Models;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Api
{
    [ClaimsAuthorize(Constants.Actions.Configure, Constants.Resources.Server)]
    [ValidateHttpAntiForgeryToken]
    public class ApplicationScopesController : ApiController
    {
        readonly IAuthorizationServerAdministration _config;

        public ApplicationScopesController(IAuthorizationServerAdministration config)
        {
            _config = config;
        }

        public HttpResponseMessage Get(int id)
        {
            var app = _config.Applications.All.SingleOrDefault(x => x.ID == id);
            if (app == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var data =
                from s in app.Scopes
                select new
                {
                    s.ID,s.Name,s.DisplayName,s.Description,s.Emphasize,clientCount=s.AllowedClients.Count
                };
            return Request.CreateResponse(HttpStatusCode.OK, data.ToArray());
        }

        public HttpResponseMessage Post(int id, ScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.GetErrors());
            }

            var app = _config.Applications.All.SingleOrDefault(x => x.ID == id);
            if (app == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (app.Scopes.Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("", "That Scope name is already in use.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.GetErrors());
            }

            var scope = new Scope
                {
                    Name = model.Name,
                    DisplayName = model.DisplayName,
                    Description = model.Description,
                    Emphasize = model.Emphasize
                };

            app.Scopes.Add(scope);
            _config.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new {
                    scope.ID,
                    scope.Name,
                    scope.DisplayName,
                    scope.Description,
                    scope.Emphasize
                });
        }
    }
}
