using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrapplingHookLogic : MonoBehaviour
{
    [Header("Propiedades fisicas del gancho")]
    public float SpringForce = 5.0f;
    public float SpringDamper = 7.0f;
    public float SpringMassScale = 4.5f;
    public float maxDistance = 30;

    [Header("Layer Mask")]
    public LayerMask whatIsGrappeable;

    [Header("Objetos")]
    public Transform gunTip, cam, player;


    private LineRenderer lr;
    private SpringJoint joint;
    private Vector3 grapplingPoint = new Vector3(0,0,0);
    private bool hookAvailable;
    private float distance;


    void Start(){
        hookAvailable = true;
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
        if (Input.GetKey(KeyCode.Q) && hookAvailable == true)
        {
            if (hookAvailable == true){
                StartHook();
            }
        if(){

                StopHook();

            }
        }
        
    }

    void LateUpdate(){
        drawRope();
    }
    void StartHook()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.position, cam.forward ,out hitInfo, maxDistance,whatIsGrappeable)){
            grapplingPoint = hitInfo.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplingPoint;

            distance = Vector3.Distance(player.position, grapplingPoint);

            joint.spring = SpringForce;
            joint.damper = SpringDamper;
            joint.massScale = SpringMassScale;

            lr.positionCount = 2;
        }
    }
    void drawRope(){
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplingPoint);
    }
    void StopHook(){
        lr.positionCount = 0;
        Destroy(joint);
    }

}
