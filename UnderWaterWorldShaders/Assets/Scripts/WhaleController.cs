using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : MonoBehaviour
{
 
    public Transform[] waypoint;        // The array of Waypoints
    public float patrolSpeed = 3f;       // The walking speed between Waypoints
    public bool  loop = true;       // Repeate Waypoints?
    public float dampingLook= 6.0f;          // How slowly to turn
    public float pauseDuration = 0;   // How long to pause at a Waypoint
    
    private float curTime;
    private int currentWaypoint = 0;
    private CharacterController character;
    public Quaternion _lookRotation;
    private Vector3 _direction;
    
    void  Start (){
    
        character = GetComponent<CharacterController>();
    }
    
    void  Update (){
    
        if(currentWaypoint < waypoint.Length){
            patrol();
        }else{    
            if(loop){
                currentWaypoint=0;
            } 
        }
    }
    
    void  patrol (){
    
        Vector3 target = waypoint[currentWaypoint].position;
        target.y = transform.position.y; // $$anonymous$$eep waypoint at character's height
        Vector3 moveDirection = target - transform.position;
    
        if(moveDirection.magnitude < 0.5f){
            if (curTime == 0)
                curTime = Time.time; // Pause over the Waypoint
            if ((Time.time - curTime) >= pauseDuration){
                currentWaypoint++;
                curTime = 0;
            }
        }else{        
            
            _direction = (target - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(-_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * dampingLook);

            character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
        }  
    }
 }

//Source Code:
//https://answers.unity.com/questions/429623/enemy-movement-from-waypoint-to-waypoint.html


