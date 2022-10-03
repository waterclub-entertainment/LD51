using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroStarAnimator : MonoBehaviour
{
    public float spherePlaneOffset;
    private Dictionary<int, Constellation> constellation;
    private float maxRange;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Computing Star Offsets");
        maxRange = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Star>() != null)
            {
                maxRange = (float)Math.Max(maxRange, Math.Sqrt(child.localPosition.x * child.localPosition.x + child.localPosition.z * child.localPosition.z));
            }
        }

        maxRange += 0.01f; //All distances in plane are less-equal this value so to guarantee that its smaller we add a little extra

        //Debug.Log("Computed Sphere Radius from Stars: " + maxRange.ToString());

        Vector3 center = new Vector3(0.0f, spherePlaneOffset + maxRange, 0.0f);

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Star>() != null)
            {
                float centerOffset = child.localPosition.x * child.localPosition.x + child.localPosition.z * child.localPosition.z;
                float delta = (float)Math.Sqrt(maxRange * maxRange - centerOffset);
                
                float offset = UnityEngine.Random.Range(maxRange - delta + spherePlaneOffset, spherePlaneOffset + maxRange + delta);
                //Debug.Log("Computed Sphere Offset from Stars in Range: (" + (maxRange - delta + spherePlaneOffset).ToString() + ", " + (maxRange + delta + spherePlaneOffset).ToString() + ")");

                child.gameObject.GetComponent<Star>().setOffset(offset);

            }
        }
    }

    void OnDrawGizmos()
    {
        maxRange = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Star>() != null)
            {
                maxRange = (float)Math.Max(maxRange, Math.Sqrt(child.localPosition.x * child.localPosition.x + child.localPosition.z * child.localPosition.z));
            }
        }

        maxRange += 0.1f; //All distances in plane are less-equal this value so to guarantee that its smaller we add a little extra

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, maxRange + spherePlaneOffset, 0.0f), maxRange);
    }

    public void SetConstellation(Dictionary<int, Constellation> _constellation)
    {
        //Set Constellation
        constellation = _constellation;
    }

    // Update is called once per frame
    void Update()
    {
        if (constellation == null)
            return;
        foreach (Constellation _c in constellation.Values)
        {
            foreach (Star s in _c.usedStars)
            {
                s.StartSeq();
            }
        }
    }
}
