using UnityEngine;

using System.IO;
using System.Text;
using System.Configuration;
using System.Collections;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsTest : MonoBehaviour
{

    AmazonEC2Client ec2;

    // Use this for initialization
    void Start()
    {
        Debug.Log(GetServiceOutput());
    }

    private string GetServiceOutput()
    {
        StringBuilder sb = new StringBuilder(1024);
        using (StringWriter sr = new StringWriter(sb))
        {
            sr.WriteLine("===========================================");
            sr.WriteLine("Welcome to the AWS .NET SDK!");
            sr.WriteLine("===========================================");

            // Print the number of Amazon EC2 instances.
            IAmazonEC2 ec2 = new AmazonEC2Client();
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();

            try
            {
                DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
                int numInstances = 0;
                numInstances = ec2Response.Reservations.Count;
                //sr.WriteLine(string.Format("You have {0} Amazon EC2 instance(s) running in the {1} region.",
                //                           numInstances, ConfigurationManager.AppSettings["AWSRegion"]));
            }
            catch (AmazonEC2Exception ex)
            {
                if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                {
                    sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                    sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                }
                else
                {
                    sr.WriteLine("Caught Exception: " + ex.Message);
                    sr.WriteLine("Response Status Code: " + ex.StatusCode);
                    sr.WriteLine("Error Code: " + ex.ErrorCode);
                    sr.WriteLine("Error Type: " + ex.ErrorType);
                    sr.WriteLine("Request ID: " + ex.RequestId);
                }
            }
            sr.WriteLine();
        }

        return sb.ToString();
    }
}