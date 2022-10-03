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
    // Start is called before the first frame update
    void Awake()
    {
        float factor = 1.0f / transform.childCount;
        float pos = 0.0f;
        foreach (Transform child in transform)
        {
            child.position = new Vector3((float)Math.Cos(2 * Math.PI * pos), 0.0f, (float)Math.Sin(2 * Math.PI * pos)) * distance + referenceCenter.transform.position + offset;

            child.up = Vector3.Normalize(referenceCenter.transform.position - child.position);
            pos += factor;

            //other setup
            child.gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
