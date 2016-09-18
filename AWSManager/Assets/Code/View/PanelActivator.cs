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
            Animator animtr = panel.GetComponent<Animator>();

            if (panel.activeSelf)
            {
                cancelClosing = false;
                if (animtr) animtr.SetTrigger("Hide");
                StartCoroutine("WaitAnimationAndInactive", animtr);
            }
            else
            {
                cancelClosing = true;
                panel.SetActive(true);
                if (animtr) animtr.SetTrigger("Show");
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
