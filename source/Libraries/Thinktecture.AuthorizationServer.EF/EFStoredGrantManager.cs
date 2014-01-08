/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Linq;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.Models;

namespace Thinktecture.AuthorizationServer.EF
{
    public class EFStoredGrantManager : IStoredGrantManager
    {
        readonly AuthorizationServerContext _db;

        public EFStoredGrantManager(AuthorizationServerContext db)
        {
            _db = db;
        }

        public void Add(StoredGrant grant)
        {
            _db.StoredGrants.Add(grant);
            _db.SaveChanges();
        }

        public StoredGrant Get(string grantIdentifier)
        {
            return _db.StoredGrants.Find(grantIdentifier);
        }

        public void Delete(string grantIdentifier)
        {
            var item = _db.StoredGrants.Find(grantIdentifier);
            if (item != null)
            {
                _db.StoredGrants.Remove(item);
                _db.SaveChanges();
            }
        }

        public StoredGrant Find(string subject, Client client, Application application, IEnumerable<Scope> scopes, StoredGrantType type)
        {
            var grants = _db.StoredGrants.Where(h => h.Subject == subject &&
                                                             h.Client.ClientId == client.ClientId &&
                                                             h.Application.ID == application.ID &&
                                                             h.Type == type).ToList();

            var tempScopes = scopes.ToArray();
            return grants.FirstOrDefault(grant => grant.Scopes.ScopeEquals(tempScopes));
        }
    }
}
