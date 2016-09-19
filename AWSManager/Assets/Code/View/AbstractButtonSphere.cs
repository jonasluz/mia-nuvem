using UnityEngine;
using System.Collections;

public abstract class AbstractButtonSphere : MonoBehaviour
{
    public Collider detectionCollider;
    public AudioClip actionClip;

    protected static AwsView m_controller;

    protected AudioSource m_audio; 

    protected virtual void Awake()
    {
        if (!detectionCollider) detectionCollider = GetComponentInChildren<Collider>();
        if (!m_controller) m_controller = GameObject.FindObjectOfType<AwsView>();
        m_audio = GetComponent<AudioSource>();
        if (m_audio) m_audio.playOnAwake = false;
    }

    protected void Update()
    {
        if (detectionCollider && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (detectionCollider.Raycast(ray, out hit, 1000f))
            {
                if (m_audio) m_audio.Play();
                Execute();
                if (m_audio && actionClip) m_audio.PlayOneShot(actionClip);
            }
        }
    }

    public abstract void Execute();

}
