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

    private float m_clicked = 0;

    void Awake()
    {
        if (!cloudsRotator) cloudsRotator = GetComponentInParent<RotatingClouds>();
    }

    void Update()
    {
        // Exibir os detalhes em caso de click duplo.
        if (cloudCollider && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (cloudCollider.Raycast(ray, out hit, 1000f))
            {
                bool doubleClick = m_clicked > 0 && m_clicked + doubleClickInterval > Time.time;
                Debug.Log(transform.rotation.eulerAngles.y);
                if (doubleClick)
                { // double click.
                    m_clicked = 0;
                    ShowDetails(true);
                }
                else
                {
                    m_clicked = Time.time;
                }
            } else
            {
                ShowDetails(false);
            }
        }
    }

    private void UpdateView()
    {
        if (name3dText) name3dText.text = m_awsInstance.InstanceId;
        if (detailsText)
        {
            detailsText.text = string.Format(
                "{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}"
                , m_awsInstance.InstanceId        // 0
                , m_awsInstance.KeyName           // 1
                , m_awsInstance.InstanceType      // 2
                , m_awsInstance.Monitoring.State  // 3
                , m_awsInstance.State.Name        // 4
                , m_awsInstance.PublicDnsName     // 5
                , m_awsInstance.PublicIpAddress   // 6
                , m_awsInstance.LaunchTime        // 7
                , string.Join(",", m_awsInstance.SecurityGroups.ConvertAll(g => g.GroupName).ToArray()) // 8
            );
        }
    }

    private void ShowDetails(bool value)
    {
        if (cloudsRotator)
        {
            float rotationDiff = 90 - transform.rotation.eulerAngles.y;
            cloudsRotator.targetRotation = value ? rotationDiff : 0;
        }
        if (detailsPanel) detailsPanel.gameObject.SetActive(value);
    }
}
