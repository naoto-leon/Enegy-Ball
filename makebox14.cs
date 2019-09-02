using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makebox14 : MonoBehaviour
{

  public  GameObject Prefabsbox;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        var paprent = this.transform;

        GameObject boxx = (GameObject)Instantiate(Prefabsbox, transform.position, Random.rotation);
        Destroy(boxx, 8f);
    }
}
