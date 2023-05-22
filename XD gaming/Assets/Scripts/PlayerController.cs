using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera cam;
    public GameObject particula;

    public float moveForce = 250;
    public float jumpForce = 60;
    public float maxSpeed = 30;

    private Rigidbody rb;

    private Ray rayo;
    private Vector3 rayPos;

    private float xForce = 0, yForce = 0, zForce = 0;
    private float mouseX = 0, mouseY = 0;

    bool jumpRequest;

    void Start(){
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update() {

        mouseX = Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxis("Mouse Y");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            xForce = 0;
            yForce = jumpForce;
            zForce = 0;
        }
        else
        {
            yForce = 0;
            xForce = Input.GetAxis("Horizontal") * Time.deltaTime * moveForce;
            zForce = Input.GetAxis("Vertical") * Time.deltaTime * moveForce;

            if (rb.velocity.magnitude > maxSpeed)
            {
                xForce = -xForce;
                zForce = -zForce;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.E)){

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

    private void FixedUpdate(){
        transform.Rotate(Vector3.up, mouseX * 10, Space.Self);
        cam.transform.Rotate(Vector3.right, mouseY * 10, Space.Self);
        rb.AddRelativeForce(new Vector3(xForce, yForce, zForce), ForceMode.Impulse);
    }
}
