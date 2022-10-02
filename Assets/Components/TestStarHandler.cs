using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStarHandler : MonoBehaviour
{
    private ParticleSystem StarParticles;
    // Start is called before the first frame update
    void Start()
    {
        StarParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start()
    {
        StarParticles.Play(true);
    }
    public void stop()
    {
        StarParticles.Stop(true);
    }
}
