using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroFluffMover : MonoBehaviour
{
    public float zoomPathUnits;
    public GameObject GoalTarget;
    public float curveHeight;

    public bool invert;

    private Vector3 BasePos;
    private Vector3 GoalPos;


    // Start is called before the first frame update
    void Start()
    {
        BasePos = transform.position;
        GoalPos = GoalTarget.transform.position;
    }

    void OnDrawGizmos()
    {
        Start();
        Vector3 MiddleUp = (GoalPos - BasePos) * 0.5f + BasePos;
        MiddleUp.y += curveHeight;

        Vector3 aTob = MiddleUp - BasePos;
        Vector3 bToc = GoalPos - MiddleUp; //
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(BasePos, MiddleUp);
        Gizmos.DrawLine(MiddleUp, GoalPos);

        Vector3 groundPathPos = aTob * zoomPathUnits + BasePos;

        Vector3 compVec = (bToc * zoomPathUnits + MiddleUp) - groundPathPos;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundPathPos, bToc * zoomPathUnits + MiddleUp);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(BasePos, 0.2f);
        Gizmos.DrawSphere(GoalPos, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(MiddleUp, 0.2f);
        Gizmos.DrawSphere(groundPathPos + compVec * zoomPathUnits, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {

        float pathUnits = zoomPathUnits;
        if (invert)
            pathUnits = 1.0f - zoomPathUnits;

        Vector3 MiddleUp = (GoalPos - BasePos) * 0.5f + BasePos;
        MiddleUp.y += curveHeight;

        Vector3 aTob = MiddleUp - BasePos;
        Vector3 bToc = GoalPos - MiddleUp; //

        Vector3 groundPathPos = aTob * pathUnits + BasePos;

        Vector3 compVec = (bToc * pathUnits + MiddleUp) - groundPathPos;

        transform.position = groundPathPos + compVec * pathUnits;

        transform.forward = Vector3.Normalize(Camera.main.transform.position - transform.position);
    }
}
