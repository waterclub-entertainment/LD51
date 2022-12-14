using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//As with most components here this is really just replacing the hassle of manual placement
public class EquidistantFluffDistributor : MonoBehaviour
{
    public GameObject referenceCenter;
    public float distance;
    public Vector3 offset;
    public float rots = 1.0f;
    public float rotOffset = 0.0f;
    public GameObject[] lst;
    // Start is called before the first frame update
    void Awake()
    {
        float factor = 1.0f / transform.childCount;
        float pos = rotOffset;
        foreach (GameObject go in lst)
        {
            Transform child = go.transform;
            Vector3 p = new Vector3((float)Math.Cos(2 * Math.PI * pos * rots), 0.0f, (float)Math.Sin(2 * Math.PI * pos * rots)) * distance + referenceCenter.transform.position + offset;
            child.position = p;

            Debug.Log(p);

            child.up = Vector3.Normalize(referenceCenter.transform.position - child.position);
            pos += factor;

            //other setup
            if (child.gameObject.GetComponent<Animator>() != null)
                child.gameObject.GetComponent<Animator>().enabled = false;
            if (child.gameObject.GetComponent<IntroFluffMover>() != null)
                child.gameObject.GetComponent<IntroFluffMover>().SetBasePos(p);
            foreach (Transform c1 in child)
            {
                if (c1.gameObject.GetComponent<Animator>() != null)
                    c1.gameObject.GetComponent<Animator>().enabled = false;
                if (c1.gameObject.GetComponent<IntroFluffMover>() != null)
                    c1.gameObject.GetComponent<IntroFluffMover>().SetBasePos(p);
            }
        }
    }
}
