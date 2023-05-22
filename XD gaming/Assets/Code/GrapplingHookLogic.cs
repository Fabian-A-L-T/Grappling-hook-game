using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookLogic : MonoBehaviour
{
    public Camera cam;
    public float HookSpeed = 30;
    
    private bool hooking;
    private Transform playerTransform;
    private Rigidbody playerRb;
    private Ray rayo;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Q) && !hooking){
            hooking = true;
            rayo = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayo, out hitInfo))
            {
                StartCoroutine(HookRoutine(hitInfo.point));
            }
                
        }
    }

    IEnumerator HookRoutine(Vector3 hookPos)
    {
        //player se le cancela gravedad
        playerRb.useGravity = false;
        
        //se escoge la posicion final
        float distance = Vector3.Distance(transform.position, hookPos);

        // mientras la distancia del player y de la posicion final sea mayor a 0.1f movemos al player en direccion de la posicion final
        while (distance > 1.0f)
        {
            Debug.Log("hooking    " + distance);
            Vector3 direction = hookPos - transform.position;
            playerRb.velocity = direction.normalized * HookSpeed;
            distance = Vector3.Distance(transform.position, hookPos);
            yield return null;
        }
        //se le devuelve la gravedad al player
        playerRb.useGravity = true;
        hooking = false;
        Debug.Log("hook end");
    }
}
