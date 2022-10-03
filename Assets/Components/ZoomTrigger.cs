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
        zoomAnimator.SetTrigger("Zoom");
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

    void LateUpdate()
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

    }
}
