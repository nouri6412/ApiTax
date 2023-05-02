using ApiTax.Models;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Ionic.Zip;

namespace ApiTax.Controllers
{
    public class CsrController : Controller
    {
   
        public ActionResult Index()
        {
            // 1. Generate RSA Key Pair (2048bits)
            //  string html = System.IO.File.ReadAllText();
            //var certificate = new X509Certificate2(Server.MapPath("~/App_Data/Ulduz Seyr Iranian [Stamp].crt"));
            //byte[] publicKey = certificate.PublicKey.EncodedKeyValue.RawData;  
            ViewBag.in_o = "";
            ViewBag.in_ou = "";
            ViewBag.in_cn = "";
            ViewBag.in_e = "";
            ViewBag.in_sn = "";
            List<string> list_error = new List<string>();
            ViewBag.error = list_error;
            return View();
        }

        [HttpPost]
        public ActionResult get_file(FormCollection formCollection)
        {
            #region example
            //var rkpg = new RsaKeyPairGenerator();
            //rkpg.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            //AsymmetricCipherKeyPair ackp = rkpg.GenerateKeyPair();

            //// 2a. Extract Private key (PEM format)

            //var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(ackp.Private);
            //var privateKeyPem = Convert.ToBase64String(privateKeyInfo.GetDerEncoded());
            //privateKeyPem = Regex.Replace(privateKeyPem, ".{64}", "$0\n");
            //var strBuilder = new StringBuilder();
            //strBuilder.AppendLine($"-----BEGIN PRIVATE KEY-----");
            //strBuilder.AppendLine(privateKeyPem);
            //strBuilder.AppendLine($"-----END PRIVATE KEY-----");
            //privateKeyPem = strBuilder.ToString();
            //Console.WriteLine(privateKeyPem);

            //// 2b. Extract Public key (PEM)

            //var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(ackp.Public);
            //var publicKeyPem = Convert.ToBase64String(publicKeyInfo.GetDerEncoded());
            //publicKeyPem = Regex.Replace(publicKeyPem, ".{64}", "$0\n");
            //strBuilder.Clear();
            //strBuilder.AppendLine($"-----BEGIN PUBLIC KEY-----");
            //strBuilder.AppendLine(publicKeyPem);
            //strBuilder.AppendLine($"-----END PUBLIC KEY-----");
            //publicKeyPem = strBuilder.ToString();
            //Console.WriteLine(publicKeyPem);

            //// 3. Encrypt with public key

            //var bytesToEncrypt = Encoding.UTF8.GetBytes("Hello world");
            //var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            //using (var txtreader = new StringReader(publicKeyPem))
            //{
            //    var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

            //    encryptEngine.Init(true, keyParameter);
            //}
            //var encrypted = encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);
            //var encryptedB64 = Convert.ToBase64String(encrypted);
            //Console.WriteLine("Encrypted string (Base64):");
            //Console.WriteLine(encryptedB64);
            //Console.WriteLine();

            //// 4. Decrypt with private key

            //using (var txtreader = new StringReader(privateKeyPem))
            //{
            //    var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            //    decryptEngine.Init(false, ackp.Private);
            //    var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(encrypted, 0, encrypted.Length));
            //    Console.WriteLine("Decrypted string:");
            //    Console.WriteLine(decrypted);
            //}

            //// 5. Create the CSR subject

            //IDictionary attrs = new Hashtable();

            //attrs.Add(X509Name.CN, "Azin teb salamat tajhiz [Stamp]");
            //attrs.Add(X509Name.SerialNumber, "14011679475");
            //attrs.Add(X509Name.O, "Non-Governmental");
            //attrs.Add(X509Name.OU, "آذین طب سلامت تجهیز");
            //attrs.Add(X509Name.C, "IR");
            //var subject = new X509Name(new ArrayList(attrs.Keys), attrs);

            //// 6. Set Key Usage Extensions

            //var keyUsage = new KeyUsage(KeyUsage.DigitalSignature | KeyUsage.KeyEncipherment | KeyUsage.NonRepudiation);
            //var extgen = new X509ExtensionsGenerator();
            //extgen.AddExtension(X509Extensions.KeyUsage, true, keyUsage);

            //var eku = new ExtendedKeyUsage(new KeyPurposeID[] { KeyPurposeID.IdKPServerAuth });
            //extgen.AddExtension(X509Extensions.ExtendedKeyUsage, false, eku);
            //var attribute = new AttributeX509(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest,
            //                                  new DerSet(extgen.Generate()));

            //// 7. Generate the CSR

            //var pkcs10CertificationRequest = new Pkcs10CertificationRequest(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id,
            //                                                                subject,
            //                                                                ackp.Public,
            //                                                                new DerSet(attribute),
            //                                                                ackp.Private);

            //// 8. Export the CSR (PEM)

            //var csr = Convert.ToBase64String(pkcs10CertificationRequest.GetEncoded());
            //var csrPem = Regex.Replace(csr, ".{64}", "$0\n");
            //strBuilder.Clear();
            //strBuilder.AppendLine($"-----BEGIN CERTIFICATE REQUEST-----");
            //strBuilder.AppendLine(csrPem);
            //strBuilder.AppendLine($"-----END CERTIFICATE REQUEST-----");
            //Console.WriteLine(strBuilder.ToString());
            //string gen_csr = strBuilder.ToString();
            //ViewBag.csr = gen_csr;

            //return File(Encoding.UTF8.GetBytes(gen_csr),
            //  "text/plain",
            //   string.Format("{0}.txt", 1));
            #endregion

            string in_o = formCollection["in_o"];
            string in_ou = formCollection["in_ou"];
            string in_cn = formCollection["in_cn"];
            string in_e = formCollection["in_e"];
            string in_sn = formCollection["in_sn"];

            ViewBag.in_o = in_o;
            ViewBag.in_ou = in_ou;
            ViewBag.in_cn = in_cn;
            ViewBag.in_e = in_e;
            ViewBag.in_sn = in_sn;

            List<string> list_error = new List<string>();
            if(in_o==null || in_o=="")
            {
                list_error.Add("نوع متقاضی نمی تواند خالی باشد");
            }

            if (in_ou == null || in_ou == "")
            {
                list_error.Add("نام سازمان نمی تواند خالی باشد");
            }

            if (in_cn == null || in_cn == "")
            {
                list_error.Add("نام کامل سازمان نمی تواند خالی باشد");
            }

            if (in_e == null || in_e == "")
            {
                list_error.Add("  پست الکترونیک نمی تواند خالی باشد");
            }

            if (in_sn == null || in_sn == "")
            {
                list_error.Add("  شناسه ملی شرکت  نمی تواند خالی باشد");
            }

            ViewBag.error = list_error;

            if(list_error.Count()>0)
            {
                return View("Index");
            }

            var rkpg = new RsaKeyPairGenerator();
            rkpg.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair ackp = rkpg.GenerateKeyPair();

            // 2a. Extract Private key (PEM format)


            var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(ackp.Private);
            var privateKeyPem = Convert.ToBase64String(privateKeyInfo.GetDerEncoded());
            privateKeyPem = Regex.Replace(privateKeyPem, ".{64}", "$0\n");
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"-----BEGIN PRIVATE KEY-----");
            strBuilder.AppendLine(privateKeyPem);
            strBuilder.AppendLine($"-----END PRIVATE KEY-----");
            privateKeyPem = strBuilder.ToString();
         //   Console.WriteLine(privateKeyPem);

            // 2b. Extract Public key (PEM)

            var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(ackp.Public);
            var publicKeyPem = Convert.ToBase64String(publicKeyInfo.GetDerEncoded());
            publicKeyPem = Regex.Replace(publicKeyPem, ".{64}", "$0\n");
            strBuilder.Clear();
            strBuilder.AppendLine($"-----BEGIN PUBLIC KEY-----");
            strBuilder.AppendLine(publicKeyPem);
            strBuilder.AppendLine($"-----END PUBLIC KEY-----");
            publicKeyPem = strBuilder.ToString();
          //  Console.WriteLine(publicKeyPem);

            // 5. Create the CSR subject

            IDictionary attrs = new Hashtable();
            attrs.Add(X509Name.C, "IR");
            attrs.Add(X509Name.O, formCollection["in_o"]);
            attrs.Add(X509Name.OU, formCollection["in_ou"]);
            attrs.Add(X509Name.CN, formCollection["in_cn"]);
            attrs.Add(X509Name.E, formCollection["in_e"]);
            attrs.Add(X509Name.SerialNumber, formCollection["in_sn"]);

            var subject = new X509Name(new ArrayList(attrs.Keys), attrs);

            // 6. Set Key Usage Extensions

            var keyUsage = new KeyUsage(KeyUsage.DigitalSignature | KeyUsage.KeyEncipherment | KeyUsage.NonRepudiation);
            var extgen = new X509ExtensionsGenerator();
            extgen.AddExtension(X509Extensions.KeyUsage, true, keyUsage);

            var eku = new ExtendedKeyUsage(new KeyPurposeID[] { KeyPurposeID.IdKPServerAuth });
            extgen.AddExtension(X509Extensions.ExtendedKeyUsage, false, eku);
            var attribute = new AttributeX509(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest,
                                              new DerSet(extgen.Generate()));

            // 7. Generate the CSR

            var pkcs10CertificationRequest = new Pkcs10CertificationRequest(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id,
                                                                            subject,
                                                                            ackp.Public,
                                                                            new DerSet(attribute),
                                                                            ackp.Private);

            // 8. Export the CSR (PEM)

            var csr = Convert.ToBase64String(pkcs10CertificationRequest.GetEncoded());
            var csrPem = Regex.Replace(csr, ".{64}", "$0\n");
            strBuilder.Clear();
            strBuilder.AppendLine($"-----BEGIN CERTIFICATE REQUEST-----");
            strBuilder.AppendLine(csrPem);
            strBuilder.AppendLine($"-----END CERTIFICATE REQUEST-----");
            var cert = strBuilder.ToString();
           // Console.WriteLine(strBuilder.ToString());


            var outputStream = new MemoryStream();

            using (var zip = new ZipFile())
            {
                zip.AddEntry("CER.csr", cert);
                zip.AddEntry("PublicKey.txt", publicKeyPem);
                zip.AddEntry("PrivateKey.key", privateKeyPem);
                zip.Save(outputStream);
            }

            outputStream.Position = 0;
            return File(outputStream, "application/zip", "certificate.zip");
        }
        private enum RootLength
        {
            RootLength2048 = 2048,
            RootLength3072 = 3072,
            RootLength4096 = 4096,
        }

    }
}