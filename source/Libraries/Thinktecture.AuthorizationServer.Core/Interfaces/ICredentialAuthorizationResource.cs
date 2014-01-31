namespace Thinktecture.AuthorizationServer.Interfaces
{
    /// <summary>
    /// Interface to specify the Credential Authorization Resource server
    /// </summary>
    /// <remarks>
    /// Added to give better IoC resolution for AutoFac and elimitnate the need for an Autofac.config file
    /// </remarks>
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