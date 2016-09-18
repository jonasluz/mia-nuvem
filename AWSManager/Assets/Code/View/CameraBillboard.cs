using UnityEngine;

using UnityStandardAssets.Cameras;

[RequireComponent(typeof(UnityStandardAssets.Cameras.LookatTarget))]
public class CameraBillboard : MonoBehaviour {

    private LookatTarget m_lookAt; 

    void Awake()
    {
        m_lookAt = GetComponentInChildren<LookatTarget>();
        m_lookAt.SetTarget(Camera.main.transform);
    }
	
}
