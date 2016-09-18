using System.Collections.Generic;

using UnityEngine;
//using UnityEngine.UI;

using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsTest : MonoBehaviour
{
    enum Operation
    {
        START, STOP, TERMINATE
    }

    public UnityEngine.UI.Text output;
    public bool outputToConsole = true;

    void Start()
    {
        ListImages();

        //RunNewInstance();
        //SpecifiedOperation(Operation.TERMINATE, "i-0591fc0197943a566");
        //SpecifiedOperation(Operation.STOP, "i-08cb9177e3acff07f");
        //SpecifiedOperation(Operation.START, "i-08cb9177e3acff07f");

        ListInstances();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
            Application.Quit();
    }

    private void ListImages()
    {
        Print("*** IMAGES ***");
        try
        {
            // Imprime dados das instâncias.
            foreach (Image ec2Image in AwsManager.Aws.GetImages())
            {
                Print(
                    "Image #{0}: {6} - Location: {1} - Type: {2} - Owner {3} - Since: {4} - Description: {5}"
                    , ec2Image.ImageId          // 0
                    , ec2Image.ImageLocation    // 1
                    , ec2Image.ImageType        // 2
                    , ec2Image.ImageOwnerAlias  // 3
                    , ec2Image.CreationDate     // 4
                    , ec2Image.Description      // 5
                    , ec2Image.Name             // 6
                );
            }

        }
        catch (AwsException ex)
        {
            Print(ex.ToString());
        }
    }

    private void ListInstances()
    {
        Print("*** INSTANCES ***");
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

    private void SpecifiedOperation(Operation op, string instance)
    {
        try
        {
            IDictionary<string, string> response = null;

            // Executa operação.
            switch (op)
            {
                case Operation.START:
                    Print("Starting instance {0}...", instance);
                    response = AwsManager.Aws.StartInstance(instance);
                    break;
                case Operation.STOP:
                    Print("Stoping instance {0}...", instance);
                    response = AwsManager.Aws.StopInstance(instance);
                    break;
                case Operation.TERMINATE:
                    Print("Terminating instance {0}...", instance);
                    response = AwsManager.Aws.TerminateInstance(instance);
                    break;
            }
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