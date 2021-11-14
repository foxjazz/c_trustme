
using System.IO;
using System.Collections.Generic;
using System.Xml.Schema;
using SecurityDriven.Inferno;
using Jil;
using System.Security.Cryptography;

namespace trustme
{
    public class setup
    {
        public static void createkey()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\k.tmk";
            var o = Aes.Create();
            o.GenerateKey();
            File.WriteAllBytes(fn, o.Key);
            File.WriteAllBytes(fn + "4", o.IV);

        }

        public static byte[] getKey()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\k.tmk";
            return File.ReadAllBytes(fn);

        }
        public static byte[] getKeyIV()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\k.tmk4";
            return File.ReadAllBytes(fn);

        }
        public static byte[] encrypt(string pt)
        {
            byte[] Key = getKey();
            byte[] IV = getKeyIV();
            AesManaged aesAlg = new AesManaged();
            
                // Encrypt the string to an array of bytes.
                var encryptor = aesAlg.CreateEncryptor(Key, IV);
            
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(pt);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
        public static void Begin()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\trust.tm";
            if (File.Exists(fn))
            {
                var cdata = File.ReadAllBytes(fn);
                string data = DecryptStringFromBytes_Aes(cdata);
                Monitor.tmData = Jil.JSON.Deserialize<Dictionary<string,string>>(data);
            }
            else
            {
                createkey();
                Monitor.tmData = new Dictionary<string, string>();
            }
        }
        public static void Save()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\trust.tm";
            var data = Jil.JSON.Serialize(Monitor.tmData);
            byte[] cypherdata = encrypt(data);
            File.WriteAllBytes(fn, cypherdata);
        }
        
        static string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            byte[] Key = getKey();
            byte[] IV = getKeyIV();
            // Check arguments.
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(Key, IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}