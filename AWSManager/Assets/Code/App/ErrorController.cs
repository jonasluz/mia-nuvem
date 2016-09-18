using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;

public class ErrorController : MonoBehaviour {

    // Singleton 
    public static ErrorController EC
    {
        get; private set;
    }

    public float showingInterval = 3f;
    public GameObject errorDisplay;

    void Awake()
    {
        // Singleton.
        if (EC == null) EC = this;
        else Destroy(this);
    }

    public void ShowError(Exception ex)
    {
        if (errorDisplay)
        {
            Text textUI = errorDisplay.GetComponentInChildren<Text>();
            if (textUI)
                textUI.text = ex.Message;
            StartCoroutine("ShowDisplay", errorDisplay);
        }
    }

    IEnumerable ShowDisplay(GameObject display)
    {
        display.SetActive(true);
        yield return new WaitForSeconds(showingInterval);
        display.SetActive(false);
    }
}
