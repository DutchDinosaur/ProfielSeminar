using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour {

    [SerializeField] float hoverForce = 1;
    [SerializeField] float hoverHeight = 2;
    [SerializeField] float speed;
    [SerializeField] float runMultip;
    [SerializeField] float rotationSpeed;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        float floorHeight = 0;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.up, -transform.up), out hit, 10)) {
            floorHeight = hit.point.y;
        }

        rb.AddForce(Vector3.up * ((floorHeight + hoverHeight) - transform.position.y) * hoverForce);

        float spd = speed;
        float rotspd = rotationSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            spd *= runMultip;
            rotspd *= runMultip;
        }
        rb.MovePosition(transform.position + transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * spd);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(Vector3.up * (rotspd * Input.GetAxis("Horizontal") * Time.deltaTime)));
    }
}