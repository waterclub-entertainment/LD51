using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    private Graph<GameObject, float> graph;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph<GameObject, float>();
        //collect child star objects
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            if (obj.GetComponent<Star>() != null)
            {
                graph.Nodes.Add(obj);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (graph == null)
        {
            Start();
        }

        foreach (var node in graph.Nodes)
        {
            Gizmos.color = node.GizmoColor;
            Gizmos.DrawSphere(node.Value, 0.125f);
        }
        foreach (var edge in graph.Edges)
        {
            Gizmos.color = edge.GizmoColor;
            Gizmos.DrawLine(edge.From.Pos, edge.To.Pos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
