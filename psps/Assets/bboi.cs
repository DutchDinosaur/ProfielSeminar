using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boids : MonoBehaviour {

    [SerializeField] GameObject prefab;
    [SerializeField] int count;
    [SerializeField] float spawnHeigt;

    
    [SerializeField] float spawnArea;

    [SerializeField] float speed;
    [SerializeField] float hoverForce = 1;
    [SerializeField] float hoverHeight = 2;

    [SerializeField] float overlapSphereRadius = 3;

    [SerializeField] float seperationDamp;

    GameObject[] objects;
    Rigidbody[] rbs;

    private void Start() {
        objects = new GameObject[count];
        rbs = new Rigidbody[count];
        for (int i = 0; i < objects.Length; i++) {
            objects[i] = GameObject.Instantiate(prefab, spawnPos(), spawnRot(), transform);
            rbs[i] = objects[i].GetComponent<Rigidbody>();
        }
    }

    private void Update() {
        for (int i = 0; i < count; i++) {
            if (Vector3.Distance(objects[i].transform.position,Vector3.zero) > spawnArea) {
                objects[i].transform.position = new Vector3(-objects[i].transform.position.x, objects[i].transform.position.y,-objects[i].transform.position.z) * .99f;
            }

            float floorHeight = 0;
            RaycastHit hit;
            if (Physics.Raycast(new Ray(objects[i].transform.position + objects[i].transform.up, -objects[i].transform.up), out hit, 10)) {
                floorHeight = hit.point.y;
            }
            rbs[i].AddForce(Vector3.up * ((floorHeight + hoverHeight) - objects[i].transform.position.y) * hoverForce);


            rbs[i].MovePosition(objects[i].transform.position + objects[i].transform.forward * Time.deltaTime * speed);

            Collider[] hitColliders = Physics.OverlapSphere(objects[i].transform.position, overlapSphereRadius);
            List<GameObject> boid = new List<GameObject>();
            for (int c = 0; c < hitColliders.Length; c++) {
                if (hitColliders[c].transform.parent != null) {
                    if (hitColliders[c].tag == "boid" && hitColliders[c].transform.parent.gameObject != objects[i]) {
                        boid.Add(hitColliders[c].transform.parent.gameObject);
                    }
                }
            }

            //Vector3 averagePos = Vector3.zero;
            Vector3 averageDir = Vector3.zero;
            Vector3 seperationDir = Vector3.zero;
            for (int b = 0; b < boid.Count; b++) {
                //averagePos += boid[b].transform.position;
                averageDir += boid[b].transform.forward;
                seperationDir += (objects[i].transform.position - boid[b].transform.position);
            }

            Vector3 lookat = objects[i].transform.position + averageDir.normalized + seperationDir.normalized + objects[i].transform.forward * seperationDamp;
            objects[i].transform.LookAt(new Vector3(lookat.x, objects[i].transform.position.y, lookat.z));
        }
    }
    
    Vector3 spawnPos() {
        return Quaternion.Euler(Vector3.up * Random.Range(0, 360)) * (Vector3.forward * Random.Range(0, spawnArea));
    }

    Quaternion spawnRot() {
        return Quaternion.Euler(0,Random.Range(0,360),0);
    }
}