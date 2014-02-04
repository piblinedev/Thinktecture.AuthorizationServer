/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
using Thinktecture.AuthorizationServer.Models;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Models
{
    public enum FindType
    {
        Thumbprint, SubjectName
    }

    public class X509KeyModel
    {
        public X509KeyModel()
        {
        }

        public X509KeyModel(X509CertificateReference key)
        {
            ID = key.ID;
            Name = key.Name;
            FindType =
                key.FindType == System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint ?
                FindType.Thumbprint : FindType.SubjectName;
            if (key.Certificate != null)
            {
                Thumbprint = key.Certificate.Thumbprint;
            }
        }

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public FindType FindType { get; set; }
        [Required]
        public string Thumbprint { get; set; }
    }
}