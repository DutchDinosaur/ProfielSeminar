using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform trackpos;

    Vector3 CamPosOffset;

    Vector3 camVelocity;
    public float smoothTime;

    private void Start() {
        CamPosOffset = transform.position;
        //if (trackpos == null) {
        //    trackpos = GameObject.FindGameObjectWithTag("Player").transform;
        //}
    }

    void Update() {
        Vector3 desiredPos = trackpos.position + CamPosOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPos, ref camVelocity, smoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(trackpos.position);
    }
}