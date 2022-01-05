using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingCollideWith : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("SM_Ship_Transport"))
            Camera.main.GetComponent<CameraFollow>().mode = 1;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.name.Contains("SM_Ship_Cruiser_02"))
            Camera.main.GetComponent<CameraFollow>().mode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
