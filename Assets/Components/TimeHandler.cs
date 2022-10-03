using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public GameObject ConstellationDrawer;
    public GameObject Stars;
    public Material glasMaterial;
    [ColorUsageAttribute(false, true)]
    public Color glasEmissoionColor;

    private ConstellationDrawer _ConstellationDrawer;
    private Animator ConstellationAnimator;
    private TestStarHandler StarParticles;
    private Animator camAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _ConstellationDrawer = ConstellationDrawer.GetComponent<ConstellationDrawer>();
        ConstellationAnimator = ConstellationDrawer.GetComponent<Animator>();
        StarParticles = Stars.GetComponent<TestStarHandler>();
        camAnimator = Camera.main.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        glasMaterial.SetColor("_EmissionColor", glasEmissoionColor);
    }


    void TriggerDusk()
    {
        StarParticles.start();
        _ConstellationDrawer.LoadNextConstellation();
        camAnimator.ResetTrigger("PanOut");
        camAnimator.SetTrigger("PanIn");
    }

    void TriggerDawn()
    {
        StarParticles.stop();
        ConstellationAnimator.ResetTrigger("FadeIn");
        ConstellationAnimator.SetTrigger("FadeOut");
        camAnimator.ResetTrigger("PanIn");
        camAnimator.SetTrigger("PanOut");
    }
}