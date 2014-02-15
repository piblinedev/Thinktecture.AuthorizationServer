using System.Collections.Generic;
using System.Linq;
using Thinktecture.AuthorizationServer.Models;

namespace Thinktecture.AuthorizationServer.Extensions
{
    public static class RedirectUriExtensions
    {
        public static ClientRedirectUri Get(this IEnumerable<ClientRedirectUri> uris, string uri)
        {
            return (from u in uris
                    where u.Uri.Equals(uri)
                    select u)
                   .SingleOrDefault();
        }
    }
}
