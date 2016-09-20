using UnityEngine;
using System.Collections;

using Amazon.EC2;
using Amazon.EC2.Model;

public class AmiView : MonoBehaviour {

    public TextMesh textData;

    public Image Ami
    {
        get
        {
            return m_ami;
        }
        set
        {
            m_ami = value;
            UpdateView();
        }
    }
    private Image m_ami;

    private void UpdateView()
    {
        if (textData)
        {
            textData.text =
                string.Format("{0} - {1} ({2})",
                m_ami.ImageId,
                m_ami.Name,
                m_ami.Platform
            );
        }
    }
}
