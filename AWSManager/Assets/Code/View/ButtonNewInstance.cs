using UnityEngine;
using System.Collections;
using System;

public class ButtonNewInstance : AbstractButtonSphere
{
    public ParticleSystem particles;
    public float waitingInterval = 3f;

    // Cria nova instância.
    public override void Execute()
    {
        if (m_controller) m_controller.NewInstance();
        if (particles) particles.Play();
    }

}
