using UnityEngine;

using System.Collections.Generic;
using System.Net;

using Amazon.Runtime;
using Amazon.Util;
using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsEnv : MonoBehaviour {

    // Singleton
    AwsEnv instance;

    public static AWSCredentials Credentials
    {
        get; private set;
    }
    public static AmazonEC2Config Config
    {
        get; private set;
    }

    void Awake()
    {
        // Implementa Singleton.
        if (instance == null) instance = this;
        else Destroy(this);
    }

	void Start ()
    {
        // Evitar erro de certificado no Mono.NET
        ServicePointManager.ServerCertificateValidationCallback = MonoCertificateFix.MyRemoteCertificateValidationCallback;

        // Ler o profile a partir de arquivo de configuração externo.
        ReadProfileData();

        // Define a configuração do endpoint.
        Config = new AmazonEC2Config();
        Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
    }

    void ReadProfileData()
    {
        // Ler as credenciais do arquivo de configuração externo.
        TextAsset data = Resources.Load<TextAsset>("credentials");
        List<string> cfg = new List<string>(3);
        foreach (string line in data.text.Split())
            if (!line.Equals("")) cfg.Add(line);

        // Registra o profile.
        ProfileManager.RegisterProfile(cfg[0], cfg[1], cfg[2]);
        // Recupera as credenciais.
        Credentials = ProfileManager.GetAWSCredentials(cfg[0]);
    }

}
