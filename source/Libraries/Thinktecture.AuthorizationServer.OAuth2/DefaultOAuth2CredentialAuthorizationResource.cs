using Thinktecture.AuthorizationServer.Interfaces;

namespace Thinktecture.AuthorizationServer.OAuth2
{
    public class DefaultOAuth2CredentialAuthorizationResource : ICredentialAuthorizationResource
    {
        public string Address { get; set; }
        public string Realm { get; set; }
        public string IssuerThumbprint { get; set; }
    }
}