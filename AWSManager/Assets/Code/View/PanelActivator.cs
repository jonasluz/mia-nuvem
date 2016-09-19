using UnityEngine;
using System.Collections;
using System;

public class PanelActivator : AbstractButtonSphere {

    public GameObject panel;

    private bool cancelClosing = false;

    public override void Execute()
    {
        if (panel)
        {
            Animator animator = panel.GetComponent<Animator>();
            if (!animator) animator = GetComponent<Animator>();

            if (panel.activeSelf)
            {
                cancelClosing = false;
                if (animator) animator.SetTrigger("Hide");
                StartCoroutine("WaitAnimationAndInactive", animator);
            }
            else
            {
                cancelClosing = true;
                panel.SetActive(true);
                if (animator) animator.SetTrigger("Show");
            }
        }
    }

    IEnumerator WaitAnimationAndInactive(Animator animtr)
    {
        float length = animtr.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        if (!cancelClosing) panel.SetActive(false);
    }
}
