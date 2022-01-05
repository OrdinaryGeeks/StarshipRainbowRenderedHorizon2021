﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject starlight;
   // public GameObject astronaut;

    Quaternion startRotation;
    public int mode;
    void Start()
    {
        mode = 0;
        startRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            if(starlight.GetComponent<LightRegulator>().health > 0)
            transform.position = starlight.transform.position + new Vector3(0.0f, 80.0f, 0.0f);
            transform.rotation = startRotation;
        }
        if (mode == 1)
        {
        //    transform.position = astronaut.transform.position + (astronaut.transform.up) * 15.0f;
         //   transform.LookAt(astronaut.transform.position + astronaut.transform.forward * 4.0f);
        }
    }
}
