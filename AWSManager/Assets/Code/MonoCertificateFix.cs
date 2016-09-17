﻿using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Requerido para resolver erro de certificado.
/// <see cref="http://stackoverflow.com/questions/4926676/mono-webrequest-fails-with-https"/>
/// <see cref="http://answers.unity3d.com/questions/792342/how-to-validate-ssl-certificates-when-using-httpwe.html"/>
/// <seealso cref="http://www.mono-project.com/docs/faq/security/"/>
/// <seealso cref="https://raw.githubusercontent.com/mono/mono/master/mcs/class/Mono.Security/Test/tools/tlstest/tlstest.cs"/>
/// </summary>
public static class MonoCertificateFix
{
    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}
