/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.IO;

namespace Thinktecture.AuthorizationServer
{
    public class KeyFileProtection : KeyProtection
    {
        public class KeyFile
        {
            public string EncryptKey;
            public string SignKey;
        }

        public KeyFileProtection(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("filePath");

            string encrKey, signKey;
            InitializeAndGetLocalKeys(filePath, out encrKey, out signKey);
            SetKeys(encrKey, signKey);
        }

        private void InitializeAndGetLocalKeys(string path, out string encrKey, out string signKey)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var keys = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyFile>(json);
                encrKey = keys.EncryptKey;
                signKey = keys.SignKey;
            }
            else
            {
                CreateKeyFile(path, out encrKey, out signKey);
            }
        }

        private static void CreateKeyFile(string path, out string encrKey, out string signKey)
        {
            CreateKeys(out encrKey, out signKey);

            var keys = new KeyFile { EncryptKey = encrKey, SignKey = signKey };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(keys);
            File.WriteAllText(path, json);
        }
    }
}
