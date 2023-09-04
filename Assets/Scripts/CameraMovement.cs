using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CameraMovement : NetworkBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    GameObject[] cameraTargets;


    public void setCameraTarget(Transform _target){
        /*cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");
        foreach (GameObject cameraTarget in cameraTargets)
        {
            if (cameraTarget.GetComponent<NetworkObject>().OwnerClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId) {
                target = cameraTarget.transform;
            }
        }*/
        target = _target;
    }


    void FixedUpdate()
    {
        if (target != null){
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
        

        //transform.position = targetPosition;
    }
}
