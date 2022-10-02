using UnityEngine;

public class ZoomTrigger : MonoBehaviour {

    public float zoomPathUnits;
    private Vector3 BasePos;

    private Animator zoomAnimator;

    void Start()
    {
        BasePos = transform.position;
        zoomAnimator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (zoomPathUnits <= 0.2f)
        {
            //start zooming in
            Debug.Log("Starting Zoom");
            zoomAnimator.ResetTrigger("ZoomOut");
            zoomAnimator.ResetTrigger("ZoomOutSoon");
            zoomAnimator.SetTrigger("ZoomIn");
        }
        else if (zoomPathUnits >= 0.8f)
        {
            //start zooming out
            Debug.Log("Ending Zoom");
            zoomAnimator.ResetTrigger("ZoomIn");
            zoomAnimator.ResetTrigger("ZoomOutSoon");
            zoomAnimator.SetTrigger("ZoomOut");
        }

    }

    void OnDrawGizmos()
    {
        Transform camTrans = Camera.main.transform;


        float NormalMultiplier = (BasePos.y - camTrans.position.y) / camTrans.forward.y; //compute Intersection Multiplier
        Vector3 camGroundIntersect = camTrans.position + NormalMultiplier * camTrans.forward;
        Gizmos.color = Color.yellow;

        Vector3 aTob = camGroundIntersect - BasePos;
        Vector3 bToc = camTrans.position - camGroundIntersect + camTrans.forward * 2; //

        Gizmos.DrawLine(BasePos, camGroundIntersect);
        Gizmos.DrawLine(camGroundIntersect + camTrans.forward, camTrans.position);

        Vector3 groundPathPos = aTob * zoomPathUnits + BasePos;

        Vector3 compVec = (bToc * zoomPathUnits + camGroundIntersect) - groundPathPos;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundPathPos, (bToc * zoomPathUnits + camGroundIntersect));


        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(BasePos, 0.2f);
        Gizmos.DrawSphere(camTrans.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(camGroundIntersect, 0.2f);
        Gizmos.DrawSphere(groundPathPos + compVec * zoomPathUnits, 0.2f);


    }

    void Update()
    {
        Transform camTrans = Camera.main.transform;
        float NormalMultiplier = (BasePos.y - camTrans.position.y) / camTrans.forward.y; //compute Intersection Multiplier


        Vector3 camGroundIntersect = camTrans.position + NormalMultiplier * camTrans.forward;

        Vector3 aTob = camGroundIntersect - BasePos;
        Vector3 bToc = camTrans.position - camGroundIntersect + camTrans.forward * 2; //

        Vector3 groundPathPos = aTob * zoomPathUnits + BasePos;

        Vector3 compVec = (bToc * zoomPathUnits + camGroundIntersect) - groundPathPos;

        transform.position = groundPathPos + compVec * zoomPathUnits;

        transform.forward = Vector3.Normalize(camTrans.position - transform.position);

        if (zoomPathUnits == 1.0f && !zoomAnimator.IsInTransition(0) && !zoomAnimator.GetCurrentAnimatorStateInfo(0).IsName("HoldZoomed"))
        {
            zoomAnimator.SetTrigger("ZoomOutSoon");
        }
    }
}
