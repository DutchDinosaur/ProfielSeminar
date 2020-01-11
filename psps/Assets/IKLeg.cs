using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLeg : MonoBehaviour
{
    [SerializeField] int BonesCount = 2;
    [SerializeField] Transform Target;
    [SerializeField] Transform Pole;
    [SerializeField] Transform LastBone;

    [SerializeField, Range(0, 1)] float SnapStrength = 1;

    [SerializeField] int iterations = 10;
    [SerializeField] float delta = 0.01f;

    float[] Lengths;
    float Length;
    Transform[] bones;
    Vector3[] poss;

    Vector3[] startDir;
    Quaternion[] startRot;
    Quaternion startRotTarget;
    Quaternion startRotRoot;

    private void Awake() {
        Initialize();
    }

    void Initialize() {
        bones = new Transform[BonesCount + 1];
        poss = new Vector3[BonesCount + 1];
        Lengths = new float[BonesCount];

        startDir = new Vector3[BonesCount + 1];
        startRot = new Quaternion[BonesCount + 1];

        startRotTarget = Target.rotation;

        Length = 0;

        Transform tr = LastBone;
        for (int i = BonesCount; i >= 0; i--) {
            bones[i] = tr;
            startRot[i] = tr.rotation;

            if (i == BonesCount) {
                startDir[i] = Target.position - tr.position;
            }
            else {
                Length += Lengths[i] = (bones[i + 1].position - tr.position).magnitude;
                startDir[i] = bones[i + 1].position - tr.position;
            }

            tr = tr.parent;
        }
    }

    private void LateUpdate() {
        IK();
    }

    void IK() {
        for (int i = 0; i < bones.Length; i++) {
            poss[i] = bones[i].position;
        }

        Quaternion rootRot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        Quaternion rootrotDiff = rootRot * Quaternion.Inverse(startRotRoot);

        if ((Target.position - bones[0].position).sqrMagnitude >= Length * Length) {

            Vector3 dir = (Target.position - poss[0]).normalized;

            for (int i = 1; i < poss.Length; i++) {
                poss[i] = poss[i - 1] + dir * Lengths[i -1];
            }
        }
        else {
            for (int i = 0; i < iterations; i++) {
                poss[BonesCount] = Target.position;
                for (int b = BonesCount - 1; b > 0; b--) {
                    poss[b] = poss[b + 1] + (poss[b] - poss[b+1]).normalized * Lengths[b];
                }
                for (int b = 1; b < poss.Length; b++) {
                    poss[b] = poss[b - 1] + (poss[b] - poss[b-1]).normalized * Lengths[b - 1];
                }

                if ((poss[BonesCount] - Target.position).sqrMagnitude < delta * delta) {
                    break;
                }
            }
        }

        if (Pole != null) {
            for (int i = 1; i < BonesCount; i++) {
                Plane plane = new Plane(poss[i + 1] - poss[i - 1], poss[i -1]);
                Vector3 pojectedpole = plane.ClosestPointOnPlane(Pole.position);
                Vector3 pojectedbone = plane.ClosestPointOnPlane(poss[i]);
                float angle = Vector3.SignedAngle(pojectedbone - poss[i - 1], pojectedpole - poss[i -1], plane.normal);
                poss[i] = Quaternion.AngleAxis(angle, plane.normal) * (poss[i] - poss[i -1]) + poss[i - 1];
            }
        }

        for (int i = 0; i < bones.Length; i++) {
            if (i == BonesCount) {
                bones[i].rotation = Target.rotation * Quaternion.Inverse(startRotTarget) * startRot[i];
            }
            else {
                bones[i].rotation = Quaternion.FromToRotation(startDir[i],poss[i + 1] - poss[i]) * startRot[i];
            }
            bones[i].position = poss[i];
        }
    }
}