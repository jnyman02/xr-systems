using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnificationScript : MonoBehaviour
{

    public Camera magnifyingCamera; // Magnification camera
    public Camera playerCamera; // Player camera for magnification camera to follow
    public RenderTexture renderTexture; // Magnification camera render texture
    public float magnification = 5.0f; // Magnification factor

    // Start is called before the first frame update
    void Start()
    {
         magnifyingCamera.targetTexture = renderTexture;
         magnifyingCamera.fieldOfView = CalculateFOV(magnifyingCamera.fieldOfView);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionFromPlayerCamToLens = magnifyingCamera.transform.position - playerCamera.transform.position;
        directionFromPlayerCamToLens.Normalize();
        magnifyingCamera.transform.rotation = Quaternion.LookRotation(directionFromPlayerCamToLens);
    }

    float CalculateFOV(float originalFOV)
    {
        // Adjust the FOV based on the magnification factor
        return originalFOV / magnification;
    }
}
