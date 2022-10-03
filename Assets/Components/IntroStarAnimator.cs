using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOffset
{
    public float offset;
    public GameObject star;
}

public class IntroStarAnimator : MonoBehaviour
{
    public float spherePlaneOffset;
    public float starOffsetAnim = 1.0f;

    private List<StarOffset> stars;

    private Dictionary<int, Constellation> constellation;
    private float maxRange;

    // Start is called before the first frame update
    void Start()
    {
        stars = new List<StarOffset>();

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

                child.localPosition = new Vector3(child.localPosition.x, offset, child.localPosition.z);
                stars.Add(new StarOffset { offset = offset, star = child.gameObject });
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

    public void ShowConstellation()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (constellation == null)
            return;
        foreach (StarOffset star in stars)
        {
            foreach (Constellation _c in constellation.Values)
            {
                if (_c.usedStars.Contains(star.star.GetComponent<Star>()))
                {
                    star.star.transform.localPosition = new Vector3(star.star.transform.localPosition.x, star.offset * starOffsetAnim, star.star.transform.localPosition.z);
                    star.star.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                }
                else
                {
                    star.star.transform.localScale = Vector3.one;
                }
            }
        }
    }
}
