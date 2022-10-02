using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public GameObject ConstellationDrawer;

    private ConstellationDrawer _ConstellationDrawer;
    private Animator ConstellationAnimator;
    private GameObject StarRoot;

    // Start is called before the first frame update
    void Start()
    {
        _ConstellationDrawer = ConstellationDrawer.GetComponent<ConstellationDrawer>();
        ConstellationAnimator = ConstellationDrawer.GetComponent<Animator>();
        StarRoot = _ConstellationDrawer.getRoot();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void TriggerDusk()
    {
        StarRoot.GetComponent<ParticleSystem>().Play(true);
        _ConstellationDrawer.LoadNextConstellation();
    }

    void TriggerDawn()
    {
        StarRoot.GetComponent<ParticleSystem>().Stop(true);
        ConstellationAnimator.SetTrigger("FadeOut");
    }
}
