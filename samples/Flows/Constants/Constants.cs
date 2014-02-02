
namespace Thinktecture.Samples
{
    public class Constants
    {
        //
        // change the below constants to match your local system
        //
        //http://localhost:9913/
        public const string WebHostName = "localhost:2601";
        public const string WebHostv1Path = "/v1/";
        public const string WebHostv2Path = "/rs2/api/";
        
        public const string WebHostv1BaseAddress = "http://" + WebHostName + WebHostv1Path;
        public const string WebHostv2BaseAddress = "http://" + WebHostName + WebHostv2Path;


        public const string Application = "users";
        public const string Audience = "users";

        public static class Clients
        {
            public const string CodeClient = "codeclient";
            public const string CodeClientSecret = "secret";
            public const string CodeClientRedirectUrl = "https://localhost:44303/callback";

            public const string ResourceOwnerClient = "resource";
            public const string ResourceOwnerClientSecret = "secret";

            public const string ImplicitClient = "implicitclient";
            public const string JavaScriptImplicitClientRedirectUrl = "https://localhost:44300/callback.cshtml";

            public const string Client = "client";
            public const string ClientSecret = "secret";
        }

        public static class AS
        {
            public const string OAuth2TokenEndpoint = "https://identity.pibline.com/authsrv/" + Application + "/oauth/token"; //" + Application + "
            public const string OAuth2AuthorizeEndpoint = "https://identity.pibline.com/authsrv/" + Application + "/oauth/authorize";

            public const string IssuerName = "http://identity.pibline.com/issuer";
            public const string SigningKey = "bDPeNV7OPRe9N55XA3PoV+V2brnOO0qemBr3BNTB478=";
        }
    }
}