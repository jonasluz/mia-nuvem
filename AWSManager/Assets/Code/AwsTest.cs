using System.Collections.Generic;

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
        ListInstances();
        //RunNewInstance();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            Application.Quit();
    }

    private void ListInstances()
    {
        Print("===========================================");
        Print("Welcome to the AWS .NET SDK!");
        Print("===========================================");

        try
        {
            // Imprime dados das instâncias.
            foreach (Instance ec2Instance in AwsManager.Aws.GetInstances())
            {
                Print(
                    "Instance #{0}: {1} - Type: {2} - Monitoring? {3} - {4} - Public DNS: {5} ({6}) - Since: {7} - Security Groups: {8}"
                    , ec2Instance.InstanceId        // 0
                    , ec2Instance.KeyName           // 1
                    , ec2Instance.InstanceType      // 2
                    , ec2Instance.Monitoring.State  // 3
                    , ec2Instance.State.Name        // 4
                    , ec2Instance.PublicDnsName     // 5
                    , ec2Instance.PublicIpAddress   // 6
                    , ec2Instance.LaunchTime        // 7
                    , string.Join(",", ec2Instance.SecurityGroups.ConvertAll(g => g.GroupName).ToArray()) // 8
                );
            }

        }
        catch (AwsException ex)
        {
            Print(ex.ToString());
        }
    }

    private void RunNewInstance()
    {
        try
        {
            // Executa uma instância nova.
            Print("Running a new instance...");
            IDictionary<string, string> response = AwsManager.Aws.RunInstance();
            if (response != null)
                foreach (string key in response.Keys)
                {
                    Print("{0} - {1}", key, response[key]);
                }
        }
        catch (AwsException ex)
        {
            Print(ex.ToString());
        }
    }

    #region Printing
    private void Print(string format, params object[] data)
    {
        Print(string.Format(format, data));
    }
    private void Print(string msg)
    {
        if (output) output.text += msg + "\n";
        if (outputToConsole) Debug.Log(msg);
    }
    #endregion Printing
}