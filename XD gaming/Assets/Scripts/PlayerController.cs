using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Propiedades de la camara")]
    public Camera cam;
    public Transform orientation;
    public float sensitivity = 300;

    private float mouseX = 0, mouseY = 0;
    private float xCamRotation;
    private float yCamRotation;

    [Header("Efectos")]
    public GameObject particula;

    [Header("Propiedades del jugador")]
    public float moveForce = 250;
    public float jumpForce = 60;
    public float jumpCooldown = 1;
    public float airMultiplier = 1;

    private bool jumpReady;

    private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    bool isGrounded;


    private Ray rayo;
    private float horizontalInput, verticalInput;

    void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGrounded = true;
        jumpReady = true;
    }

    // Update is called once per frame
    private void Update() {


        //Control de la camara
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        yCamRotation += mouseX;

        xCamRotation -= mouseY;
        xCamRotation = Mathf.Clamp(xCamRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xCamRotation, yCamRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yCamRotation, 0);
        //---------------------

        //Control del personaje
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpReady)
        {
            jumpReady = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(resetJump), jumpCooldown);
        }
        //---------------------

        //GroundCheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (isGrounded) {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        //---------------------

        //Control de velocidad
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveForce)
        {
            Vector3 limitedVel = flatVel.normalized * moveForce;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        //---------------------

        if (Input.GetKeyDown(KeyCode.E)) {

            rayo = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayo, out hitInfo))
            {
                GameObject temp;
                temp = Instantiate(particula, hitInfo.point, Quaternion.identity);
                temp.transform.forward = hitInfo.normal;
                Destroy(temp, 1.0f);
            }
        }

    }
    private void FixedUpdate() {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (isGrounded) {
            rb.AddForce(moveDirection.normalized * moveForce * 10f);
        }
        else if (!isGrounded){
            rb.AddForce(moveDirection.normalized * moveForce * airMultiplier * 10f);
        }

    }
    void resetJump()
    {
        jumpReady = true;
    }
}
