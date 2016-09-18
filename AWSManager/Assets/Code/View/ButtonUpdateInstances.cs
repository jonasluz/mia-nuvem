using UnityEngine;
using System.Collections;
using System;

public class ButtonUpdateInstances : AbstractButtonSphere
{
    // Cria nova instância.
    public override void Execute()
    {
        if (m_controller) m_controller.UpdateInstances();
    }
}
