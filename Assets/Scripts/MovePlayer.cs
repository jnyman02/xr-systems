using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{

    public InputActionReference movePlayerAction;

    public GameObject playerRig;
    public int locationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        movePlayerAction.action.Enable();
        movePlayerAction.action.performed += (ctx) => 
        {   
            if (locationIndex == 0) {
                playerRig.transform.position = new Vector3(35, 20, -35);
                locationIndex = 1;
            }
            else if (locationIndex == 1) {
                playerRig.transform.position = new Vector3(0, 2, 0);
                locationIndex = 0;
            }
            
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
