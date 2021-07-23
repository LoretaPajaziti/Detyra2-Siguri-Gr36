using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace Detyra2_TCPserver
{
    class x509
    {
        public static byte[] merrCelesinPublik()
        {
            //string Certificate = "C:\\Users\\PC\\Desktop\\Certifikata_X509\\CertifikataX509.pfx";
            string Certificate = "C:\\Users\\loret\\Desktop\\Detyra2 - Siguri - Gr36\\Detyra2_TCPserver\\Detyra2_TCPserver\\bin\\Debug\\CertifikataX509.pfx";
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(Certificate), "12345678", X509KeyStorageFlags.MachineKeySet);
       
            byte[] celesipublik = cert.GetPublicKey();
            //string result = System.Convert.ToBase64String(celesipublik)    

            return cert.GetPublicKey();
        }
    }
}
