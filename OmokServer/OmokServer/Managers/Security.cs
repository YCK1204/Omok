using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer.Managers
{
    public class SecurityManager
    {
        RSACryptoServiceProvider prirsa = new RSACryptoServiceProvider();
        RSACryptoServiceProvider pubrsa = new RSACryptoServiceProvider();
        public SecurityManager()
        {
            // 개인키 생성(복호화용)
            RSAParameters privateKey = RSA.Create().ExportParameters(true);
            pubrsa.ImportParameters(privateKey);

            // 공개키 생성(암호화용)
            RSAParameters publicKey = new RSAParameters();
            publicKey.Modulus = privateKey.Modulus;
            publicKey.Exponent = privateKey.Exponent;
            prirsa.ImportParameters(publicKey);
        }
        #region RSA
        public string RSAKey { get { return pubrsa.ToXmlString(false); } }
        public byte[] RSAEncrypt(byte[] value)
        {
            return prirsa.Encrypt(value, false);
        }
        public byte[] RSADecrypt(byte[] value)
        {
            return pubrsa.Decrypt(value, false);
        }
        #endregion
        #region AES
        public Aes GenerateAes()
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }
        public byte[] AESEncrypt(byte[] value, ICryptoTransform encryptor)
        {
            return encryptor.TransformFinalBlock(value, 0, value.Length);
        }
        public byte[] AESDecrypt(byte[] value, ICryptoTransform decryptor)
        {
            return decryptor.TransformFinalBlock(value, 0, value.Length);
        }
        #endregion
    }
}