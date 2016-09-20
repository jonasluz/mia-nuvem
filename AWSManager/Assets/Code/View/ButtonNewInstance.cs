using UnityEngine;
using System.Collections;
using System;

public class ButtonNewInstance : AbstractButtonSphere
{
    public ParticleSystem particles;
    public float waitingInterval = 3f;

    private string m_amiId = null;

    // Cria nova instância.
    public override void Execute()
    {
        if (m_amiId == null) ReadAmiId();
        if (m_controller) m_controller.NewInstance(m_amiId);
        if (particles) particles.Play();
    }

    private void ReadAmiId()
    {
        AmiView view = GetComponentInParent<AmiView>();
        if (view) m_amiId = view.Ami.ImageId;
    }
}
