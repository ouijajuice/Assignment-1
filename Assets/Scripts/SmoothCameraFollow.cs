using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 moveposition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, moveposition, ref velocity, damping);
    }
}
