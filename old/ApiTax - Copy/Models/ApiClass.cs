using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
namespace ApiTax.Models
{
    public class ApiClass
    {
        //(string)روش امضا رشته 
        public static string SignData(String stringToBeSigned, string privateKey)
        {
            var pem = "-----BEGIN PRIVATE KEY-----\n" + privateKey + "\n-----END PRIVATE KEY---- - "; // Add header and footer
            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricKeyParameter privateKeyParams =
            (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams =
            DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)privateKeyParams)
            ;
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();// cspParams);
            csp.ImportParameters((RSAParameters)rsaParams);
            var dataBytes = Encoding.UTF8.GetBytes(stringToBeSigned);
            return Convert.ToBase64String(csp.SignData(dataBytes,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
        }

        // GCM/ AES کد روش رمزگذاری به روش متقارن 
        public static string AesEncrypt(byte[] payload, byte[] key, byte[] iv)
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            byte[] baPayload = new byte[0];
            cipher.Init(true, new AeadParameters(new KeyParameter(key), 128, iv,
           baPayload));
            var cipherBytes = new byte[cipher.GetOutputSize(payload.Length)];
            int len = cipher.ProcessBytes(payload, 0, payload.Length, cipherBytes, 0);
            cipher.DoFinal(cipherBytes, len);
            return Convert.ToBase64String(cipherBytes);
        }
        //xorکردنصورتحساب با کلید متقثارن
        public static byte[] Xor(byte[] left, byte[] right)
        {/*from w w w. ja v a 2 s . c o m*/
            byte[] val = new byte[left.Length];
            for (int i = 0; i < left.Length; i++)
                val[i] = (byte)(left[i] ^ right[i]);
            return val;
        }
        //SHA256-OAEP-R کد روش رمزگذاری به روش نامتقارن 
        public static string EncryptData(String stringToBeEncrypted, string publicKey)
        {
            try
            {
                //var pem = "-----BEGIN PUBLIC KEY-----\n" + publicKey + "\n- ----END PUBLIC KEY-----"; // Add header and footer                actory.CreateKey(Convert.FromBase64String(publicKey));
                AsymmetricKeyParameter asymmetricKeyParameter =
   PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
                RsaKeyParameters rsaKeyParameters =
                (RsaKeyParameters)asymmetricKeyParameter;
                RSAParameters rsaParameters = new RSAParameters();
                rsaParameters.Modulus =
                rsaKeyParameters.Modulus.ToByteArrayUnsigned();
                rsaParameters.Exponent =
                rsaKeyParameters.Exponent.ToByteArrayUnsigned();

                RSACng rsa = new RSACng();
                rsa.ImportParameters(rsaParameters);
                var get_b = Encoding.UTF8.GetBytes(stringToBeEncrypted
                );
                var en = rsa.Encrypt(get_b, RSAEncryptionPadding.OaepSHA256);
                string base64 =
                Convert.ToBase64String(en);
                if (base64.Length % 4 == 3)
                {
                    base64 += "=";
                }
                else if (base64.Length % 4 == 2)
                {
                    base64 += "==";
                }
                return base64;
            }
            catch
            {
                return "error";
            }
        }

        public static byte[] getRandomNonce(int byteSize)
        {
            byte[] nonce = new byte[byteSize];
            new SecureRandom().NextBytes(nonce);
            return nonce;
        }

    }
}