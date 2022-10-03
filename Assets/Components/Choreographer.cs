using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choreographer : MonoBehaviour
{
    public GameObject StarRoot;
    public GameObject[] constellationMap;
    private Constellation referenceConstellation = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UnloadConstellation()
    {
        if (referenceConstellation != null)
        {
            GameObject.Destroy(referenceConstellation.gameObject);
        }

        Debug.Log("Unloaded Constellation");
    }

    private void LoadConstellation(int index)
    {
        UnloadConstellation();

        GameObject reference = GameObject.Instantiate(constellationMap[index], transform);
        reference.tag = "constellation";

        referenceConstellation = reference.GetComponent<Constellation>();
        referenceConstellation.root = StarRoot;

        Debug.Log("Loaded Constellation " + index.ToString());
    }

    void SetStarConstellation(int constellation)
    {
        if (constellation >= 0)
        {
            LoadConstellation(constellation);

            StarRoot.GetComponent<IntroStarAnimator>().SetConstellation(referenceConstellation);
        }
        else
        {
            UnloadConstellation();
            StarRoot.GetComponent<IntroStarAnimator>().SetConstellation(null);
        }
    }
    void ShowConstellation()
    {
        StarRoot.GetComponent<IntroStarAnimator>().ShowConstellation();
    }
}
