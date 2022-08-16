using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public PlayerMovement myPLayer;
    public Vector3 Offset;

    private void Start()
    {
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        if (myPLayer != null)
            transform.position = Vector3.Lerp(transform.position, myPLayer.transform.position + Offset, 5 * Time.deltaTime);
    }
}
