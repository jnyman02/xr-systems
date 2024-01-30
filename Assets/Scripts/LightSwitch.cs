using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light light;
    public InputActionReference lightAction;

    private Color[] lightColors = {
        Color.white,
        Color.red,
        Color.green,
        Color.blue
    };

    int lightColorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {   
        light = GetComponent<Light>();
        lightAction.action.Enable();
        lightAction.action.performed += (ctx) => 
        {
            lightColorIndex = (lightColorIndex + 1) % lightColors.Length;
            light.color = lightColors[lightColorIndex];
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
