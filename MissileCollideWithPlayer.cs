using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollideWithPlayer : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {

        //   Debug.Log("OPC");

     //   other.GetComponent<LightRegulator>().health -= 5;
     //
       /*
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }*/
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
