using UnityEngine;
using UnityEngine.UI;

using Amazon.EC2;
using Amazon.EC2.Model;

public class AwsInstanceView : MonoBehaviour
{
    [Header("References")]
    public RectTransform detailsPanel;
    public Text detailsText;
    public TextMesh name3dText;
    public Collider cloudCollider;
    public RotatingClouds cloudsRotator;
    public GameObject buttonPlay;
    public GameObject buttonStop;
    public GameObject buttonTerminate;

    [Header("Input")]
    public float doubleClickInterval = .2f;

    public Instance AwsInstance
    {
        get { return m_awsInstance; }
        set {
            m_awsInstance = value;
            UpdateView();
        }
    }
    private Instance m_awsInstance;

    private Material m_material;

    void Awake()
    {
        if (!cloudsRotator) cloudsRotator = GetComponentInParent<RotatingClouds>();
        if (cloudCollider) m_material = cloudCollider.gameObject.GetComponent<MeshRenderer>().material;
    }

    private void UpdateView()
    {
        if (name3dText) name3dText.text = m_awsInstance.InstanceId;
        if (detailsText)
        {
            string name = m_awsInstance.PublicDnsName;
            if (name != null) name = name.Split('.')[0];
            detailsText.text = string.Format(
                "{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}"
                , m_awsInstance.InstanceType                // 0
                , m_awsInstance.ImageId                     // 1
                , m_awsInstance.LaunchTime                  // 2
                , m_awsInstance.State.Name                  // 3
                , name                                      // 4
                , m_awsInstance.PublicIpAddress             // 5
                , string.Join(",", m_awsInstance.SecurityGroups.ConvertAll(g => g.GroupName).ToArray()) // 6
            );
        }

        // Ajusta elementos de acordo com o estado da instância.
        bool buttonsSet = buttonPlay && buttonStop && buttonTerminate;
        if (buttonsSet)
        {
            switch (m_awsInstance.State.Name)
            {
                case "running":
                    m_material.color = Color.white;
                    buttonPlay.SetActive(false);
                    buttonStop.SetActive(true);
                    buttonTerminate.SetActive(true);
                    break;
                case "stopped":
                case "stopping":
                    m_material.color = Color.red;
                    buttonPlay.SetActive(true);
                    buttonStop.SetActive(false);
                    buttonTerminate.SetActive(true);
                    break;
                default:
                    m_material.color = Color.blue;
                    buttonPlay.SetActive(false);
                    buttonStop.SetActive(false);
                    buttonTerminate.SetActive(false);
                    break;
            }
        }
    }
}
