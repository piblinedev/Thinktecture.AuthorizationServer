﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.AuthorizationServer.Models
{
    public class Client : IValidatableObject
    {
        [Key]
        [Required]
        public virtual string ClientId { get; set; }

        [Required]
        public virtual string ClientSecret { get; private set; }

        public virtual ClientAuthenticationMethod AuthenticationMethod { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual OAuthFlow Flow { get; set; }
        
        public virtual bool AllowRefreshToken { get; set; }

        public virtual bool Enabled { get; set; }

        // only relevant if Flow == Code or Implicit
        public virtual bool RequireConsent { get; set; }

        public virtual List<ClientRedirectUri> RedirectUris { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Flow == OAuthFlow.Implicit && AllowRefreshToken)
            {
                yield return
                    new ValidationResult("Implicit Flow can not use Refresh Tokens", new[] {"Code", "AllowRefreshToken"})
                    ;
            }
        }

        public void SetSharedSecret(string password)
        {
            AuthenticationMethod = ClientAuthenticationMethod.SharedSecret;

            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            bytes = DataProtectection.Instance.Protect(bytes);
            ClientSecret = Convert.ToBase64String(bytes);
        }

        public string GetSharedSecret()
        {
            if (AuthenticationMethod != ClientAuthenticationMethod.SharedSecret) return null;

            var bytes = Convert.FromBase64String(ClientSecret);
            bytes = DataProtectection.Instance.Unprotect(bytes);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public bool ValidateSharedSecret(string password)
        {
            if (AuthenticationMethod != ClientAuthenticationMethod.SharedSecret) return false;

            var val = GetSharedSecret();
            return IdentityModel.ObfuscatingComparer.IsEqual(password, val);
        }

        public void SetCertificateThumbprint(string thumbprint)
        {
            AuthenticationMethod = ClientAuthenticationMethod.CertificateThumbprint;
            ClientSecret = thumbprint;
        }

        public bool ValidateCertificateThumbprint(string thumbprint)
        {
            if (AuthenticationMethod != ClientAuthenticationMethod.CertificateThumbprint) return false;
            return thumbprint == ClientSecret;
        }
    }
}