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
using Thinktecture.AuthorizationServer.WebHost.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Api
{
    [ClaimsAuthorize(Constants.Actions.Configure, Constants.Resources.Server)]
    [ValidateHttpAntiForgeryToken]
    public class ClientRedirectsController : ApiController
    {
        readonly IAuthorizationServerAdministration _config;

        public ClientRedirectsController(IAuthorizationServerAdministration config)
        {
            _config = config;
        }

        public HttpResponseMessage Get(string id)
        {
            var query =
                from item in _config.Clients.All
                where item.ClientId == id
                from uri in item.RedirectUris
                select new
                {
                    uri.ID, uri.Description, uri.Uri
                };
            return Request.CreateResponse(HttpStatusCode.OK, query.ToArray());
        }

        public HttpResponseMessage Delete(int id)
        {
            var query =
                from r in _config.ClientRedirects.All
                where r.ID == id
                select r;
            var item = query.SingleOrDefault();
            if (item != null)
            {
                _config.ClientRedirects.Remove(item);
                _config.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Post(string id, ClientRedirectModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.GetErrors());
            }

            var client = _config.Clients.All.SingleOrDefault(x => x.ClientId == id);
            if (client == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var item = new ClientRedirectUri { Uri = model.Uri, Description = model.Description };
            client.RedirectUris.Add(item);
            _config.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }
    }
}
