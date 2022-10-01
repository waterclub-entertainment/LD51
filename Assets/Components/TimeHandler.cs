using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public GameObject StarHandler;

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
        StarHandler.GetComponent<ConstellationDrawer>().OnDusk();
    }

    void TriggerDawn()
    {
        StarHandler.GetComponent<ConstellationDrawer>().OnDawn();
    }
}
