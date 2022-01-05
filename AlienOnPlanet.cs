using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.AI;

public class AlienOnPlanet : MonoBehaviour
{

    public GameObject planet;
    public Vector3 gravity;
    LayerMask spaceShip;

    NavMeshAgent navAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        spaceShip = LayerMask.GetMask(new string[] { "WalkableSpaceship" });
    }
    void OnCollisionEnter(Collision collision)
    {


  //      gravity = Vector3.zero;




        }
    void FixedUpdate()
    {


    //    gravity = -(transform.position - planet.transform.position);

   //     transform.position += gravity * Time.deltaTime;

   /*     Ray ray = new Ray(transform.position, gravity);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000))
        {


            transform.position = hit.point;


        }*/

    }


        // Update is called once per frame
        void Update()
    {


       Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, 10000, spaceShip))
        {


            navAgent.SetDestination(floorHit.point);



        }


            //    transform.LookAt(Vector3.forward, transform.position - planet.transform.position);
            //    transform.LookAt(Vector3.forward, transform.position - planet.transform.position);
            //transform.up = ( transform.position- planet.transform.position);
            //transform.right = 
            if (Input.GetKey(KeyCode.UpArrow))
        {

            transform.position += Vector3.forward;
            
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            transform.position -= Vector3.right;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {

            transform.position += Vector3.right;

        }

       // transform.position -=  (transform.position - planet.transform.position);




    }
}
