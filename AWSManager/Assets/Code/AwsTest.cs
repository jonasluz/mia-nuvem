using UnityEngine;

using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections;

using Amazon.Runtime;
using Amazon.Util;
using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsTest : MonoBehaviour
{
    void Start()
    {
        GetServiceOutput();
    }

    private void GetServiceOutput()
    {
        Debug.Log("===========================================");
        Debug.Log("Welcome to the AWS .NET SDK!");
        Debug.Log("===========================================");

        // Print the number of Amazon EC2 instances.
        IAmazonEC2 ec2 = new AmazonEC2Client(AwsEnv.Credentials, AwsEnv.Config);
        try
        {
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();
            DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
            int numInstances = 0;
            numInstances = ec2Response.Reservations.Count;
            /*
            Debug.Log(string.Format("You have {0} Amazon EC2 instance(s) running in the {1} region.",
                                        numInstances, ConfigurationManager.AppSettings["AWSRegion"]));
            */
            foreach(Reservation ec2Reservation in ec2Response.Reservations)
                foreach (Instance ec2Instance in ec2Reservation.Instances)
                {
                    Debug.Log(ec2Instance.KeyName);
                }
        }
        catch (AmazonEC2Exception ex)
        {
            if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
            {
                Debug.Log("The account you are using is not signed up for Amazon EC2.");
                Debug.Log("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
            }
            else
            {
                Debug.Log("Caught Exception: " + ex.Message);
                Debug.Log("Response Status Code: " + ex.StatusCode);
                Debug.Log("Error Code: " + ex.ErrorCode);
                Debug.Log("Error Type: " + ex.ErrorType);
                Debug.Log("Request ID: " + ex.RequestId);
            }
        }
    }
}