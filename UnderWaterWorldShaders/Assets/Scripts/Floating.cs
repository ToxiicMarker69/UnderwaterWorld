using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float speed;
    public float tolerance;
    public float startYPos;
    public float endYPos;
    private float target;
    private Vector3 position;

    public float rotationX = 1;
    public float rotationY = 1;
    public float rotationZ = 1;

   
    // Update is called once per frame
    void Update ()
    {
        // Get current position
        position = transform.position;
        float curYPos = position.y;
 
        // Move between the start and end vectors
        if(isApproximate(curYPos, endYPos, tolerance))
        {
            target = startYPos;
        }else if (isApproximate(curYPos, startYPos, tolerance))
        {
            target = endYPos;
        }
 
        // Update position
        transform.position = Vector3.Lerp(position, new Vector3(position.x, target, position.z), speed * Time.deltaTime);
        transform.Rotate(rotationX, rotationY, rotationZ, Space.Self);
 
    }
 
    bool isApproximate(float a, float b, float tolerance)
    {
        return Mathf.Abs (a - b) < tolerance;
    }
}
////https://forum.unity.com/threads/lerping-back-and-forth-between-two-vectors.265298/