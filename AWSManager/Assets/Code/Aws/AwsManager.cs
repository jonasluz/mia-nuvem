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

    /// <summary>
    /// Recupera as instâncias EC2 existentes.
    /// </summary>
    /// <returns>Lista de instâncias EC2</returns>
    public IList<Instance> GetInstances()
    {
        List<Instance> result = new List<Instance>();

        // Recupera as instancias.
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

        return result;
    }

    public IList<Image> GetImages()
    {
        IList<Image> result = new List<Image>();

        // Recupera as imagens.
        try
        {
            DescribeImagesRequest ec2Request = new DescribeImagesRequest();
            ec2Request.Owners.Add("self");
            DescribeImagesResponse ec2Response = Ec2.DescribeImages(ec2Request);

            foreach (Image ec2Image in ec2Response.Images)
            {
                result.Add(ec2Image);
            }
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result;
    }

    public IDictionary<string, string> RunInstance(string amiId = null)
    {
        IDictionary<string, string> result = null;

        // Executa uma instância.
        try
        {
            if (amiId == null) amiId = AwsEnv.DEFAULT_AMI_ID;
            RunInstancesRequest runRequest = new RunInstancesRequest(amiId, 1, 1);
            runRequest.InstanceType = AwsEnv.DEFAULT_INST_TYPE;
            runRequest.KeyName = AwsEnv.DEFAULT_KEY_NAME;
            runRequest.SecurityGroups.Add(AwsEnv.DEFAULT_SECURITY_GROUP);

            RunInstancesResponse runResponse = Ec2.RunInstances(runRequest);
            result = runResponse.ResponseMetadata.Metadata;
            result.Add("STATUS_CODE", runResponse.HttpStatusCode.ToString());
            result.Add("RESPONSE", runResponse.ToString());
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result;
    }

    public IDictionary<string, string> StartInstance(string instanceId)
    {
        return StartInstance(new List<string>(){ instanceId });
    }
    public IDictionary<string, string> StartInstance(List<string> instancesIds)
    {
        IDictionary<string, string> result;

        // Inicia uma instância.
        try
        {
            StartInstancesRequest startRequest = new StartInstancesRequest(instancesIds);
            StartInstancesResponse startResponse = Ec2.StartInstances(startRequest);
            result = startResponse.ResponseMetadata.Metadata;
            result.Add("STATUS_CODE", startResponse.HttpStatusCode.ToString());
            result.Add("RESPONSE", startResponse.ToString());
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result;
    }

    public IDictionary<string, string> StopInstance(string instanceId)
    {
        return StopInstance(new List<string>() { instanceId });
    }
    public IDictionary<string, string> StopInstance(List<string> instancesIds)
    {
        IDictionary<string, string> result;

        // Para uma instância.
        try
        {
            StopInstancesRequest stopRequest = new StopInstancesRequest(instancesIds);
            StopInstancesResponse stopResponse = Ec2.StopInstances(stopRequest);
            result = stopResponse.ResponseMetadata.Metadata;
            result.Add("STATUS_CODE", stopResponse.HttpStatusCode.ToString());
            result.Add("RESPONSE", stopResponse.ToString());
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result;
    }

    public IDictionary<string, string> TerminateInstance(string instanceId)
    {
        return TerminateInstance(new List<string>() { instanceId });
    }
    public IDictionary<string, string> TerminateInstance(List<string> instancesIds)
    {
        IDictionary<string, string> result;

        // Destrói uma instância.
        try
        {
            TerminateInstancesRequest terminateRequest = new TerminateInstancesRequest(instancesIds);
            TerminateInstancesResponse terminateResponse = Ec2.TerminateInstances(terminateRequest);
            result = terminateResponse.ResponseMetadata.Metadata;
            result.Add("STATUS_CODE", terminateResponse.HttpStatusCode.ToString());
            result.Add("RESPONSE", terminateResponse.ToString());
        }
        catch (AmazonEC2Exception ex)
        {
            throw new AwsException(ex);
        }

        return result;
    }

}
