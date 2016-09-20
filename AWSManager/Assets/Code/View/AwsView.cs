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

    public AmiView imageViewPrefab;

    public Transform amiButtonsRoot;

    public float timeToUpdate = 1f;

    private Dictionary<string, AwsInstanceView> m_dictInstances = 
        new Dictionary<string, AwsInstanceView>();
    private Dictionary<string, AmiView> m_dictImages = 
        new Dictionary<string, AmiView>();

    #region Monobehaviour
    void Start()
    {
        UpdateImages();
        UpdateInstances();
    }
    #endregion Monobehaviour

    public void NewInstance(string amiId)
    {
        try
        {
            AwsManager.Aws.RunInstance(amiId);
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

    public void UpdateImages()
    {
        IList<Image> images = null;
        try
        {
            images = AwsManager.Aws.GetImages();
        }
        catch (Exception ex)
        {
            ErrorController.EC.ShowError(ex);
        }
        if (images == null || images.Count == 0) return;

        // Elimina os game objects de imagens que não existem mais. 
        IEnumerable<string> imagesIds = images.Select(i => i.ImageId);
        foreach (string id in m_dictImages.Keys)
            if (!imagesIds.Contains(id))
            {
                Destroy(m_dictImages[id].gameObject);
                m_dictImages.Remove(id);
            }

        // Altura entre os botões.
        float yDiff = 1.1f;

        // Cria um botão para cada imagem.
        if (imageViewPrefab)
        {
            float localY = 0;
            foreach (Image ec2Image in images)
            {
                string id = ec2Image.ImageId;
                AmiView imageView;
                if (m_dictImages.Keys.Contains(id))
                {
                    imageView = m_dictImages[id];
                }
                else
                {
                    Transform parent = amiButtonsRoot ? amiButtonsRoot.transform : transform;
                    imageView = Instantiate(imageViewPrefab, parent) as AmiView;
                    m_dictImages.Add(id, imageView);
                }
                if (imageView)
                {
                    imageView.transform.localPosition = new Vector3(0, localY, 0);
                    imageView.Ami = ec2Image;
                }
                localY += yDiff;
                Debug.Log("Added Image " + id);
            }
        }

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
        foreach (string id in m_dictInstances.Keys)
            if (!instancesIds.Contains(id))
            {
                Destroy(m_dictInstances[id].gameObject);
                m_dictInstances.Remove(id);
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
                if (m_dictInstances.Keys.Contains(id))
                {
                    instanceView = m_dictInstances[id];
                    instanceView.transform.rotation = Quaternion.identity;
                }
                else
                {
                    instanceView = Instantiate(instanceViewPrefab, transform) as AwsInstanceView;
                    m_dictInstances.Add(id, instanceView);
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
