using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bboi : MonoBehaviour {

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
                objects[i].transform.position = new Vector3(-objects[i].transform.position.x, objects[i].transform.position.y, -objects[i].transform.position.z) * .99f;
            }
            if (Vector3.Distance(objects[i].transform.position, Vector3.zero) < 2.5f)
            {
                objects[i].transform.position = Quaternion.Euler(Vector3.up * Random.Range(0, 360)) * (Vector3.forward * spawnArea);
            }

           float floorHeight = 0;
            RaycastHit hit;
            if (Physics.Raycast(new Ray(objects[i].transform.position + objects[i].transform.up, -objects[i].transform.up), out hit, 10)) {
                floorHeight = hit.point.y;
            }
            rbs[i].AddForce(Vector3.up * ((floorHeight + hoverHeight) - objects[i].transform.position.y) * hoverForce);


            rbs[i].MovePosition(objects[i].transform.position + objects[i].transform.forward * Time.deltaTime * speed);

            objects[i].transform.LookAt(new Vector3(0, 2, 0));
        }
    }
    
    Vector3 spawnPos() {
        return Quaternion.Euler(Vector3.up * Random.Range(0, 360)) * (Vector3.forward * Random.Range(0, spawnArea));
    }

    Quaternion spawnRot() {
        return Quaternion.Euler(0,Random.Range(0,360),0);
    }
}