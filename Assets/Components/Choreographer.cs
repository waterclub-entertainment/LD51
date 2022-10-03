using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Choreographer : MonoBehaviour
{
    [Serializable]
    public class ConstellationMap
    {
        public GameObject summoner;
        public GameObject constellation;
        public Material decal;
    }
    public GameObject StarRoot;
    public ConstellationMap[] constellationMap;
    public GameObject SymbolPlane;

    public GameObject menuRef;


    public float fluffAnimatorSpeed = 1.0f;
    
    public AudioClip choirSound;
    public AudioClip portalSound;

    private Dictionary<int, Constellation> referenceConstellation;
    private Constellation visibleConstellation;

    // Start is called before the first frame update
    void Start()
    {
        referenceConstellation = new Dictionary<int, Constellation>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().enabled = !menuRef.activeSelf;
    }

    private void UnloadConstellation(int index)
    {
        Constellation c = referenceConstellation[index];
        if (c == visibleConstellation)
        {
            visibleConstellation = null;
        }
        referenceConstellation.Remove(index);
        GameObject.Destroy(c.gameObject);

        Debug.Log("Unloaded Constellation");
    }

    private void LoadConstellation(int index)
    {
        Debug.Log("Loading Constellation " + index.ToString());
        GameObject reference = GameObject.Instantiate(constellationMap[index].constellation, transform);
        reference.tag = "constellation";

        Constellation c = reference.GetComponent<Constellation>();
        c.root = StarRoot;
        reference.SetActive(false);

        referenceConstellation.Add(index, c);

        Debug.Log("Loaded Constellation " + index.ToString());
    }

    void SetStarConstellation(int constellation)
    {
            LoadConstellation(constellation);
            StarRoot.GetComponent<IntroStarAnimator>().SetConstellation(referenceConstellation);
    }
    void ShowDecal(int constellation)
    {
        if (visibleConstellation != null)
        {
            visibleConstellation.gameObject.SetActive(false);
        }
        visibleConstellation = referenceConstellation[constellation];
        visibleConstellation.gameObject.SetActive(true);
        visibleConstellation.UpdateConnectionPositions();

        Material[] mats = new Material[1] { constellationMap[constellation].decal};
        SymbolPlane.GetComponent<MeshRenderer>().materials = mats;
        SymbolPlane.SetActive(true);
    }
    void UnsetStarConstellation(int constellation)
    {

        UnloadConstellation(constellation);
        StarRoot.GetComponent<IntroStarAnimator>().SetConstellation(referenceConstellation);
    }

    void FallFluff(int fluff)
    {
        Animator aniRef = constellationMap[fluff].summoner.GetComponent<Animator>();
        aniRef.enabled = true;
        Debug.Log("Setting Speed at " + fluffAnimatorSpeed.ToString());
        aniRef.SetFloat("animSpeed", fluffAnimatorSpeed);
    }

    void ToMainScene()
    {
        SceneManager.LoadScene("Scenes/Main");
    }
    
    void Choir()
    {
        GetComponent<AudioSource>().PlayOneShot(choirSound);
    }
    
    void Portal()
    {
        GetComponent<AudioSource>().PlayOneShot(portalSound);
    }
}
