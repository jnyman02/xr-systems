using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    public InputActionReference action2;
    bool grabbing = false;

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private bool rotationMultiplier = false;

    private void Start()
    {
        action.action.Enable();

        action2.action.Enable();

        action2.action.performed += (ctx) => 
        {
            rotationMultiplier = !rotationMultiplier;
        };

        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>()) {
            if (c != this) {
                otherHand = c;
            }   
        }

        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    void Update()
    {
        grabbing = action.action.IsPressed();
        if (grabbing) {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject) {
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
            }

            if (grabbedObject) {
                Vector3 positionDelta = transform.position - previousPosition;
                Quaternion rotationDelta = transform.rotation * Quaternion.Inverse(previousRotation);
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                if (otherHand == null || !otherHand.grabbing) {
                    grabbedObject.position += positionDelta;
                    // grabbedObject.rotation = rotationDelta * grabbedObject.rotation;
                } else if (otherHand.grabbing && otherHand.grabbedObject == grabbedObject) {
                    Vector3 combinedPositionDelta = positionDelta + (otherHand.transform.position - otherHand.previousPosition);

                    // Average the position delta when both hands are influencing the object
                    grabbedObject.position += combinedPositionDelta * 0.5f;

                    // Combine the rotation deltas
                    Quaternion combinedRotationDelta = rotationDelta * Quaternion.Inverse(otherHand.previousRotation) * otherHand.transform.rotation;

                    rotationDelta = combinedRotationDelta;

                    // Apply the combined rotation
                    // grabbedObject.rotation = combinedRotationDelta * grabbedObject.rotation;
                }

                if (rotationMultiplier) {
                    rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
                    angle *= 2;
                    rotationDelta = Quaternion.AngleAxis(angle, axis);
                }

                grabbedObject.rotation = rotationDelta * grabbedObject.rotation;

                if (positionDelta != Vector3.zero || rotationDelta != Quaternion.identity) {
                    Vector3 directionToController = grabbedObject.position - transform.position;
                    directionToController = rotationDelta * directionToController;
                    grabbedObject.position = transform.position + directionToController;
                }
            }
        }
        // If let go of button, release object
        else if (grabbedObject) {
            grabbedObject = null;
        }

        // Should save the current position and rotation here
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}
