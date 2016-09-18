using UnityEngine;
using System.Collections;

public class ButtonStartInstance : AbstractButtonSphere
{
    public AwsInstanceView instanceView;

    // Elimina a instância atual.
    public override void Execute()
    {
        string id = instanceView.AwsInstance.InstanceId;
        if (m_controller) m_controller.StartInstance(id);
    }
}
