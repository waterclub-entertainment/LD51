using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBillboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform camTrans = Camera.main.transform;
        float NormalMultiplier = (transform.position.y - camTrans.position.y) / camTrans.forward.y; //compute Intersection Multiplier


        Vector3 camGroundIntersect = camTrans.position + NormalMultiplier * camTrans.forward;

        Vector3 dir = camGroundIntersect - transform.position;
        dir.y = 0;
        transform.forward = Vector3.Normalize(dir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
