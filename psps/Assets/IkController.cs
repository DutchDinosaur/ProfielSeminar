using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkController : MonoBehaviour {

    [SerializeField] float distanceTreshold = 2;
    [SerializeField] float stepDistanceMultip = 1;
    [SerializeField] IKLeg[] Leggs;
    [SerializeField] float moovementMult = .05f;

    Vector3[] Targets;
  
    private void Start() {
        Targets = new Vector3[Leggs.Length];
    }

    int aaaa;
    bool up;
    Vector3 previousPosition;
    Vector3 stepVector;

    void FixedUpdate() {
        stepVector = (transform.position - previousPosition) * stepDistanceMultip;

        for (int i = 0; i < Leggs.Length; i++) {
            Leggs[i].target += (Targets[i] - Leggs[i].target) * moovementMult;

            if (Leggs[i].extended) {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(Leggs[i].Target.position + transform.up * 2 + stepVector, -transform.up), out hit, 5)) {
                    Targets[i] = hit.point;
                }
            }
        }

        if (aaaa >= 30) {
            doSteps(true);
            aaaa = 0;
        }
        else if (aaaa == 15) {
            doSteps(false);
        }
        aaaa += 1;

        previousPosition = transform.position;
    }

    void doSteps(bool even) {
        int startLeg;
        if (even) startLeg = 0;
        else startLeg = 1;

        for (int i = startLeg; i < Leggs.Length; i += 2)  {
            float distToTarget = Vector3.Distance(Leggs[i].Target.position, Leggs[i].target);

            if (distToTarget >= distanceTreshold) {

                RaycastHit hit;
                if (Physics.Raycast(new Ray(Leggs[i].Target.position + transform.up * 2 + stepVector, -transform.up), out hit, 5))  {
                    Targets[i] = hit.point;
                }
            }
        }
    }
}