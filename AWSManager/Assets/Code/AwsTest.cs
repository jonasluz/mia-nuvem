using UnityEngine;
using UnityEngine.UI;

using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsTest : MonoBehaviour
{
    public Text output;
    public bool outputToConsole = true;

    void Start()
    {
        GetServiceOutput();
    }

    private void GetServiceOutput()
    {
        Print("===========================================");
        Print("Welcome to the AWS .NET SDK!");
        Print("===========================================");

        // Print the number of Amazon EC2 instances.
        IAmazonEC2 ec2 = new AmazonEC2Client(AwsEnv.Credentials, AwsEnv.Config);
        try
        {
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();
            DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
            int numInstances = 0;
            numInstances = ec2Response.Reservations.Count;
            /*
            Print(string.Format("You have {0} Amazon EC2 instance(s) running in the {1} region.",
                                        numInstances, ConfigurationManager.AppSettings["AWSRegion"]));
            */
            foreach(Reservation ec2Reservation in ec2Response.Reservations)
                foreach (Instance ec2Instance in ec2Reservation.Instances)
                {
                    Print(ec2Instance.KeyName);
                }
        }
        catch (AmazonEC2Exception ex)
        {
            if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
            {
                Print("The account you are using is not signed up for Amazon EC2.");
                Print("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
            }
            else
            {
                Print("Caught Exception: " + ex.Message);
                Print("Response Status Code: " + ex.StatusCode);
                Print("Error Code: " + ex.ErrorCode);
                Print("Error Type: " + ex.ErrorType);
                Print("Request ID: " + ex.RequestId);
            }
        }
    }

    private void Print(string msg)
    {
        if (output) output.text += msg + "\n";
        if (outputToConsole) Debug.Log(msg);
    }
}