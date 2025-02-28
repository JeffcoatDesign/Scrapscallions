using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoramaCamera : MonoBehaviour
{
    public Vector3 target; // The target object to rotate around
    public float rotationSpeed = 10f; // The speed at which the camera rotates
    private UIManager manager;

    void Start()
    {
        target = Vector3.zero;
        manager = FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        if (manager.isMainMenuOpen)
        {
            // If no target is assigned, return
            if (target == null)
            {
                return;
            }

            // Rotate the camera around the target at a constant rate
            transform.RotateAround(target, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
