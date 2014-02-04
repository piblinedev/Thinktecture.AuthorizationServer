﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.IO;
using System.Net.Http.Formatting;
using Thinktecture.AuthorizationServer.Models;

namespace Thinktecture.AuthorizationServer.WebHost.Areas.Admin.Api.Formatters
{
    public class X509CertificateReferenceFormatter : BufferedMediaTypeFormatter
    {
        readonly string _filename;
        public X509CertificateReferenceFormatter(string filename)
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/cer"));
            _filename = filename;
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(X509CertificateReference);
        }

        public override void SetDefaultContentHeaders(Type type, System.Net.Http.Headers.HttpContentHeaders headers, System.Net.Http.Headers.MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = _filename + ".cer"
                };
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content)
        {
            var item = value as X509CertificateReference;

            if (item == null)
            {
                base.WriteToStream(type, value, writeStream, content);
            }
            else
            {
                var val = Convert.ToBase64String(item.Certificate.RawData);
                using (var sw = new StreamWriter(writeStream))
                {
                    sw.Write(val);
                    sw.Flush();
                }
            }
        }
    }
}