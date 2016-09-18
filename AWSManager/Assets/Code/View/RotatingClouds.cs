using UnityEngine;
using System.Collections;

public class RotatingClouds : MonoBehaviour {

    public float rotatingSpeed = 10f;
    public float timeToFocus = 1.5f;
    public float focusZeroLimit = .01f;

    [HideInInspector]
    public float targetRotation = 0;

    bool m_rotating = false;

    void Update ()
    {
        if (Input.GetMouseButtonDown(0)) m_rotating = true;
        else if (Input.GetMouseButtonUp(0)) m_rotating = false;

        if (m_rotating)
        {
            targetRotation = 0;
            float angle = rotatingSpeed * Input.GetAxis("Horizontal");
            transform.Rotate(0, angle, 0);
        }
	}

    void FixedUpdate()
    {
        if (targetRotation != 0)
        {
            float diff = Mathf.Abs(90 - transform.rotation.eulerAngles.y);
            if (diff <= focusZeroLimit) targetRotation = 0f;

            float delta = Time.fixedDeltaTime / timeToFocus;
            float yAngle = Mathf.LerpAngle(0, targetRotation, delta);
            transform.Rotate(0, yAngle, 0);
        }
    }
}
