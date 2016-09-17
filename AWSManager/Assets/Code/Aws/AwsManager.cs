using UnityEngine;
using System.Collections;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsManager : MonoBehaviour {

    // Singleton
    AwsManager instance;

    public static IAmazonEC2 Ec2
    {
        get; private set;
    }

    void Awake()
    {
        // Implementar singleton.
        if (instance == null) instance = this;
        else Destroy(this);
    }

	void Start ()
    {
        // Instanciar o cliente EC2.
        Ec2 = new AmazonEC2Client(AwsEnv.Credentials, AwsEnv.Config);
	}
	
}
