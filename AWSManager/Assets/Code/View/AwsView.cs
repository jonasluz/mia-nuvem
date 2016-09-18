using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsView : MonoBehaviour {

    /// <summary>
    /// Prefab do GameObject que mantém cada instância AWS.
    /// </summary>
    public AwsInstanceView instanceViewPrefab;

    public float timeToUpdate = 1f;

    private Dictionary<string, AwsInstanceView> m_dict = 
        new Dictionary<string, AwsInstanceView>();

    #region Monobehaviour
    void Start()
    {
        UpdateInstances();
    }
    #endregion Monobehaviour

    public void NewInstance()
    {
        try
        {
            AwsManager.Aws.RunInstance();
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }

        WaitAndUpdate();
    }

    public void StartInstance(string id)
    {
        try
        {
            AwsManager.Aws.StartInstance(id);
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }

        WaitAndUpdate();
    }

    public void StopInstance(string id)
    {
        try
        {
            AwsManager.Aws.StopInstance(id);
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }

        WaitAndUpdate();
    }

    public void TerminateInstance(string id)
    {
        try
        {
            AwsManager.Aws.TerminateInstance(id);
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }

        WaitAndUpdate();
    }

    /// <summary>
    /// Atualiza a visão das nuvens de instâncias.
    /// </summary>
    public void UpdateInstances()
    {
        // Carrega as instâncias.
        IList<Instance> instances = null;
        try
        {
            instances = AwsManager.Aws.GetInstances();
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }
        if (instances == null || instances.Count == 0) return;

        // Elimina os game objects de instâncias que não existem mais. 
        IEnumerable<string> instancesIds = instances.Select(i => i.InstanceId);
        foreach (string id in m_dict.Keys)
            if (!instancesIds.Contains(id))
            {
                Destroy(m_dict[id].gameObject);
                m_dict.Remove(id);
            }
        
        // Calcula o ângulo entre instâncias.
        float angleSpacing = 360 / instances.Count;

        // Cria uma nuvem para cada instância.
        if (instanceViewPrefab)
        {
            float currentAngle = 0;
            foreach (Instance ec2Instance in instances)
            {
                string id = ec2Instance.InstanceId;
                AwsInstanceView instanceView;
                if (m_dict.Keys.Contains(id))
                {
                    instanceView = m_dict[id];
                    instanceView.transform.rotation = Quaternion.identity;
                }
                else
                {
                    instanceView = Instantiate(instanceViewPrefab, transform) as AwsInstanceView;
                    m_dict.Add(id, instanceView);
                }
                if (instanceView)
                {
                    instanceView.transform.Rotate(0, currentAngle, 0);
                    instanceView.AwsInstance = ec2Instance;
                }
                currentAngle += angleSpacing;
            }
        }
    }

    private IEnumerable WaitAndUpdate()
    {
        yield return new WaitForSeconds(timeToUpdate);
        UpdateInstances();
    }
}
