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

    // Fijar objetivo de cámara
    public void setCameraTarget(Transform _target){
        target = _target;
    }

    // Moverse hacia el objetivo de cámara suavemente
    void FixedUpdate()
    {
        if (target != null){
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
