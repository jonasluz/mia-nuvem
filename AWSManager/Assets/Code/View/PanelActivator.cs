using UnityEngine;
using System.Collections;
using System;

public class PanelActivator : AbstractButtonSphere {

    public GameObject panel;

    private Animator m_animator;
    private bool m_cancelClosing = false;

    public override void Execute()
    {
        if (panel)
        {
            if (!m_animator) m_animator = panel.GetComponent<Animator>();
            if (!m_animator) m_animator = GetComponent<Animator>();

            if (panel.activeSelf)
            {
                m_cancelClosing = false;
                if (m_animator) m_animator.SetTrigger("Hide");
                StartCoroutine("WaitAnimationAndInactive", m_animator);
            }
            else
            {
                m_cancelClosing = true;
                panel.SetActive(true);
                if (m_animator) m_animator.SetTrigger("Show");
            }
        }
    }

    IEnumerator WaitAnimationAndInactive(Animator animtr)
    {
        float length = animtr.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        if (!m_cancelClosing && panel.activeSelf) panel.SetActive(false);
    }
}
