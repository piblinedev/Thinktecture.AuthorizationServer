namespace Thinktecture.AuthorizationServer.Interfaces
{
    /// <summary>
    /// Added to convert the AutoFAC config file into proper IoC
    /// </summary>
    public interface ICredentialAuthorizationResource
    {
        string Address { get; set; }
        string IssuerThumbprint { get; set; }
        string Realm { get; set; }
    }
}