using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public GameObject ConstellationDrawer;
    public GameObject Stars;

    private ConstellationDrawer _ConstellationDrawer;
    private Animator ConstellationAnimator;
    private TestStarHandler StarParticles;

    // Start is called before the first frame update
    void Start()
    {
        _ConstellationDrawer = ConstellationDrawer.GetComponent<ConstellationDrawer>();
        ConstellationAnimator = ConstellationDrawer.GetComponent<Animator>();
        StarParticles = Stars.GetComponent<TestStarHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void TriggerDusk()
    {
        StarParticles.start();
        _ConstellationDrawer.LoadNextConstellation();
    }

    void TriggerDawn()
    {
        StarParticles.stop();
        ConstellationAnimator.ResetTrigger("FadeIn");
        ConstellationAnimator.SetTrigger("FadeOut");
    }
}
