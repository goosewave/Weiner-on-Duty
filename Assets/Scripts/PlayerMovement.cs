using UnityEngine;

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
        Vector3 horizontalDirection = new Vector3(horizontal, 0f, 0f).normalized;
        Vector3 verticalDirection = new Vector3(0f, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            if (horizontalflag == false && horizontal != 0) {
                horizontalflag = true;
                if (array[0] == 0) {
                    array[0] = 1;
                }
                else {
                    array[0] = 1;
                }
            }
            if (verticalflag == false && vertical != 0) {
                verticalflag = true;
                if (array[0] == 0) {
                    array[0] = 2;
                }
                else {
                    array[1] = 2;
                }
            }

            if (horizontalflag == true && horizontal == 0) {
                horizontalflag = false;
                if (array[0] == 1) {
                    array[0] = 0;
                }
                else {
                    array[1] = 0;
                }
            }
            if (verticalflag == true && vertical == 0) {
                verticalflag = false;
                if (array[0] == 2) {
                    array[0] = 0;
                }
                else {
                    array[1] = 0;
                }
            }

            if (array[0] == 0 && array[1] != 0) {
                array[0] = array[1];
                array[1] = 0;
            }

            if (array[1] == 0) {
                if (array[0] == 1) {
                    direction = horizontalDirection;
                }
                else if (array[0] == 2) {
                    direction = verticalDirection;
                }
            }

            if (array[1] != 0) {
                if (array[1] == 1) {
                    direction = horizontalDirection;
                }
                else if (array[1] == 2) {
                    direction = verticalDirection;
                }
            }

            controller.Move(direction * speed * Time.deltaTime);
            lastDirection = direction;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            horizontalflag = false;
            verticalflag = false;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
