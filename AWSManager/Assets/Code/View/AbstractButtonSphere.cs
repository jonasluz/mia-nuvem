using UnityEngine;
using System.Collections;

public abstract class AbstractButtonSphere : MonoBehaviour
{
    public Collider detectionCollider;

    protected static AwsView m_controller;

    protected virtual void Awake()
    {
        if (!detectionCollider) detectionCollider = GetComponentInChildren<Collider>();
        if (!m_controller) m_controller = GameObject.FindObjectOfType<AwsView>();
    }

    protected void Update()
    {
        if (detectionCollider && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (detectionCollider.Raycast(ray, out hit, 1000f))
                Execute();
        }
    }

    public abstract void Execute();

}
