using UnityEngine;
using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsManager : MonoBehaviour {

    // Singleton
    public static AwsManager Aws
    {
        get; private set;
    }

    public IAmazonEC2 Ec2
    {
        get; private set;
    }

    #region Monobehaviour
    void Awake()
    {
        // Implementar singleton.
        if (Aws == null) Aws = this;
        else Destroy(this);
    }

	void Start ()
    {
        // Instanciar o cliente EC2.
        Ec2 = new AmazonEC2Client(AwsEnv.Credentials, AwsEnv.Config);
	}
    #endregion Monobehavior

    public IEnumerable<Instance> GetInstances()
    {
        List<Instance> result = new List<Instance>();

        try
        {
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();
            DescribeInstancesResponse ec2Response = Ec2.DescribeInstances(ec2Request);

            foreach (Reservation ec2Reservation in ec2Response.Reservations)
                foreach (Instance ec2Instance in ec2Reservation.Instances)
                {
                    result.Add(ec2Instance);
                }
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result.ToArray();
    }
}
