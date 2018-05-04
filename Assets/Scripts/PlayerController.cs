using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public Camera cam; // The camera in the scene

    // How fast the player is moving
    public float walkSpeed = 30;
    public float runSpeed = 50;
    public float speed;

    public float jumpForce; // How high the player can jump
    public LayerMask groundedMask;

    bool grounded;
    Transform cameraT; // Transform of the camera
    float verticalLookRotation;
    private Rigidbody rb; // The rigid body of the player
    private Vector3 moveDir;

    // ---------------------------------------------------------------
    private void Start()
    {
        
        cameraT = cam.transform;
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
    }
    // ---------------------------------------------------------------
    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(this.transform.position);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -5, 60);
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(speed == runSpeed)
            {
                speed = walkSpeed;
            }else if(speed != runSpeed)
            {
                speed = runSpeed;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(transform.up * jumpForce);
            }
        }
        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

    }
    // ---------------------------------------------------------------
    private void FixedUpdate()
    {       
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * speed * Time.deltaTime);

    }
    // ---------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
