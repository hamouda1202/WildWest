using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public GameObject lasseau;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;



    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp("q"))
        {
            StopGrapple();
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {

        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}