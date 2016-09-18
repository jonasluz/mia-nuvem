using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public abstract class AbstractButtonSphere : MonoBehaviour
{
    protected static AwsView m_controller;

    protected Collider m_collider;

    protected virtual void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
        if (!m_controller) m_controller = GameObject.FindObjectOfType<AwsView>();
    }

    protected void Update()
    {
        if (m_collider && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (m_collider.Raycast(ray, out hit, 1000f))
                Execute();
        }
    }

    public abstract void Execute();

}
