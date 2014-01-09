namespace Thinktecture.AuthorizationServer.Interfaces
{
    public interface ICredentialAuthorizationResource
    {
        /// <summary>
        /// Uri of Authorization Resource Server
        /// </summary>
        string Address { get; set; }
        /// <summary>
        /// Authorization Realm
        /// </summary>
        string Realm { get; set; }
        /// <summary>
        /// Certificate Issuer Thumbprint
        /// </summary>
        string IssuerThumbprint { get; set; }
    }
}