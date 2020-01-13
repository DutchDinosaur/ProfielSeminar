using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkController : MonoBehaviour {

    public float minExtention = .2f;
    public float maxExtention = .7f;
    public int stepChance = 20;
    public IKLeg[] Leggs;
    public float moovementMult = .05f;

    Vector3[] Targets;
  
    private void Start() {
        Targets = new Vector3[Leggs.Length];
    }

    void FixedUpdate() {
        for (int i = 0; i < Leggs.Length; i++) {


            if (Leggs[i].extended ||((Leggs[i].extention > maxExtention || Leggs[i].extention < minExtention) && stepChance > Random.Range(1,100))) {
                //Targets[i] = Leggs[i].Target.position;




                RaycastHit hit;
                Ray ray = new Ray(Leggs[i].Target.position + transform.up * 2, -transform.up);

                if (Physics.Raycast(ray, out hit, 4)) {
                    Targets[i] = hit.point;
                    
                }
            }

            Leggs[i].target += (Targets[i] - Leggs[i].target) * moovementMult;
        }
    }
}