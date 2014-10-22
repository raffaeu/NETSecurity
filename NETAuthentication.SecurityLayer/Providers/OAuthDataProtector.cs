using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Security.DataProtection;

namespace NETAuthentication.SecurityLayer.Providers
{
    public sealed class OAuthDataProtector : IDataProtector
    {
        #region Fields

        private readonly byte[] key;

        #endregion Fields

        #region Constructors

        public OAuthDataProtector(string key)
        {
            using (var sha1 = new SHA256Managed())
            {
                this.key = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        #endregion Constructors

        #region IDataProtector Methods

        public byte[] Protect(byte[] data)
        {
            byte[] dataHash;
            using (var sha = new SHA256Managed())
            {
                dataHash = sha.ComputeHash(data);
            }

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var bwEncrypt = new BinaryWriter(csEncrypt))
                    {
                        bwEncrypt.Write(dataHash);
                        bwEncrypt.Write(data.Length);
                        bwEncrypt.Write(data);
                    }
                    byte[] protectedData = msEncrypt.ToArray();
                    return protectedData;
                }
            }
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = key;

                using (var msDecrypt = new MemoryStream(protectedData))
                {
                    var iv = new byte[16];
                    msDecrypt.Read(iv, 0, 16);

                    aesAlg.IV = iv;

                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var brDecrypt = new BinaryReader(csDecrypt))
                    {
                        byte[] signature = brDecrypt.ReadBytes(32);
                        int len = brDecrypt.ReadInt32();
                        byte[] data = brDecrypt.ReadBytes(len);

                        byte[] dataHash;
                        using (var sha = new SHA256Managed())
                        {
                            dataHash = sha.ComputeHash(data);
                        }

                        if (!dataHash.SequenceEqual(signature))
                            throw new SecurityException("Signature does not match the computed hash");

                        return data;
                    }
                }
            }
        }

        #endregion IDataProtector Methods    
    }
}