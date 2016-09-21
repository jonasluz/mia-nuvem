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

    public static readonly string DEFAULT_AMI_ID = "ami-eed9a5f9";
    public static readonly string DEFAULT_INST_TYPE = "t2.micro";
    public static readonly string DEFAULT_KEY_NAME = "JALJ_AWS_MIA-1";
    public static readonly string DEFAULT_SECURITY_GROUP = "default";
    
    public static string KeyName
    {
        get
        {
            return s_keyname == null ? DEFAULT_KEY_NAME : s_keyname;
        }
    }
    private static string s_keyname; 

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
        ServicePointManager.ServerCertificateValidationCallback = 
            MonoCertificateFix.MyRemoteCertificateValidationCallback;

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
        List<string> cfg = new List<string>(4);
        foreach (string line in data.text.Split())
            if (!line.Equals("")) cfg.Add(line);

        // Registra o profile.
        s_keyname = cfg.Count == 4 ? cfg[3] : null;
        ProfileManager.RegisterProfile(cfg[0], cfg[1], cfg[2]);
        // Recupera as credenciais.
        Credentials = ProfileManager.GetAWSCredentials(cfg[0]);
    }

}
