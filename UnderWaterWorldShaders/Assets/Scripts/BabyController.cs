using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour
{
    public Transform[] waypoint;
    public float dampingLook= 6.0f;
     private Quaternion _lookRotation;
    private Vector3 _direction;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject bigWhale = GameObject.Find("BigWhale");
        WhaleController whaleController = bigWhale.GetComponent<WhaleController>();
        transform.rotation = Quaternion.Slerp(transform.rotation, whaleController._lookRotation, Time.deltaTime * dampingLook);
    }
}
