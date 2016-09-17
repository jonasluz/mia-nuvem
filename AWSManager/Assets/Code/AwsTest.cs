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
        IAmazonEC2 ec2 = AwsManager.Ec2;
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
            Print(new AwsException(ex).ToString());
        }
    }

    private void Print(string msg)
    {
        if (output) output.text += msg + "\n";
        if (outputToConsole) Debug.Log(msg);
    }
}