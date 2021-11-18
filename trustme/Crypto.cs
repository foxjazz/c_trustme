using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace trustme
{
    public sealed class Crypto
    {
        private static Crypto instance = null;

        private Crypto co;
        public static Crypto Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Crypto();
                    instance.setKey();
                }
                return instance;
            }
        }

        private byte[] pfnKey;
        private byte[] pfnKey4;
        Aes aes;
        private  void setKey()
        {

            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "/k.tmk";
            var fn4 = cd + "/k4.tmk";
            if (!File.Exists(fn)){
                aes = Aes.Create();
                aes.GenerateKey();

                File.WriteAllBytes(fn, aes.Key);
                File.WriteAllBytes(fn4, aes.IV);
            }
            this.pfnKey = File.ReadAllBytes(fn);
            this.pfnKey4 = File.ReadAllBytes(fn4);

        }

        public string decrypt(byte[] cipherText)
        {
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {


                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(pfnKey, pfnKey4);

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
        public byte[] encrypt(string pt)
        {
            
            AesManaged aesAlg = new AesManaged();
            // Encrypt the string to an array of bytes.
            var encryptor = aesAlg.CreateEncryptor(pfnKey, pfnKey4);

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

    }
}