using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public GameObject StarRoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void TriggerDusk()
    {
        StarRoot.GetComponent<ParticleSystem>().Play(true);
    }

    void TriggerDawn()
    {
        StarRoot.GetComponent<ParticleSystem>().Stop(true);
    }
}
