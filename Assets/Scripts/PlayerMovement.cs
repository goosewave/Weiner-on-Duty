using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float rotationSpeed = 720f;
    private bool horizontalflag;
    private bool verticalflag;
    private int[] array;
    private Vector3 lastDirection = Vector3.forward;

    void Start()
    {
        // Flags to represent intial press of held key
        horizontalflag = false;
        verticalflag = false;
        // If entry is 1, it means horizontal movement is active, if 2, vertical movement is active
        // Order of 1 and 2 determines the order of movement, with last one taking priority
        array = new int[2];
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 horizontalDirection = (horizontal * Vector3.right).normalized;
        Vector3 verticalDirection = (vertical * Vector3.forward).normalized;


        // Reset flags if no input is detected
        if (horizontalflag && horizontal == 0) {
            horizontalflag = false;
            if (array[0] == 1) array[0] = 0;
            else if (array[1] == 1) array[1] = 0;
        }

        // Reset flags if no input is detected
        if (verticalflag && vertical == 0) {
            verticalflag = false;
            if (array[0] == 2) array[0] = 0;
            else if (array[1] == 2) array[1] = 0;
        }

        // Shift second element to front if front is zero
        if (array[0] == 0 && array[1] != 0) {
            array[0] = array[1];
            array[1] = 0;
        }

        if (direction.magnitude >= 0.1f)
        {

            if (!horizontalflag && horizontal != 0) {
                horizontalflag = true;
                if (array[0] == 0) array[0] = 1;
                else array[1] = 1;
            }

            if (!verticalflag && vertical != 0) {
                verticalflag = true;
                if (array[0] == 0) array[0] = 2;
                else array[1] = 2;
            }

            // Movement direction selection
            if (array[1] == 0) {
                if (array[0] == 1) direction = horizontalDirection;
                else if (array[0] == 2) direction = verticalDirection;
            } else {
                if (array[1] == 1) direction = horizontalDirection;
                else if (array[1] == 2) direction = verticalDirection;
            }

            controller.Move(direction * speed * Time.deltaTime);
            lastDirection = direction;

            if (direction != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (lastDirection != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
