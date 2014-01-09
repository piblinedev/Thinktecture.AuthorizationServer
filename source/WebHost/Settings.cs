/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Configuration;

namespace Thinktecture.AuthorizationServer.WebHost
{
    public class Settings
    {
        public static bool EnableAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:EnableAdmin"].Equals("true");
            }
        }
        public static bool EnableInitialConfiguration
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:EnableInitialConfiguration"].Equals("true");
            }
        }
        public static bool EnableSelfService
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:EnableSelfService"].Equals("true");
            }
        }

        public static string CredentialResourceAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:CredentialResourceAddress"];
            }
        }
        public static string CredentialResourceRealm
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:CredentialResourceRealm"];
            }
        }
        public static string CredentialResourceThumbprint
        {
            get
            {
                return ConfigurationManager.AppSettings["authz:CredentialResourceThumbprint"];
            }
        }
    }
}