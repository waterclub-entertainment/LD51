using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Connection
{
    public int from, to;
}

public class Constellation : MonoBehaviour
{
    private Graph<int, float> graph;

    public float NodeGizmoSize;
    public Connection[] Connections;


    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph<int, float>();
        Dictionary<int, Star> starReference = new Dictionary<int, Star>();
        //collect child star objects
        foreach (Transform child in transform)
        {
            Star obj = child.gameObject.GetComponent<Star>();
            if (obj != null)
            {
                starReference.Add(obj.getValue(), obj);
                graph.Nodes.Add(obj);
            }
        }

        foreach (Connection c in Connections)
        {
            INode fromNode = starReference[c.from];
            INode toNode = starReference[c.to];
            if (fromNode != null && toNode != null)
            {
                graph.Edges.Add(new Edge<float>()
                {
                    GizmoColor = Color.red,
                    Value = 1.0f,
                    From = fromNode,
                    To = toNode
                });
            }
            else
            {
                Debug.Log("Failed to add Connection from " + c.from.ToString() + " to " + c.to.ToString());
            }
        }
    }

    void OnDrawGizmos()
    {
        //sadly this will have to be redone each time as critical data may change
        //if (graph == null)
        {
            Start();
        }

        foreach (var node in graph.Nodes)
        {
            Gizmos.color = node.getColor();
            Gizmos.DrawSphere(node.getPosition(), NodeGizmoSize);
        }
        foreach (var edge in graph.Edges)
        {
            Gizmos.color = edge.GizmoColor;
            Gizmos.DrawLine(edge.From.getPosition(), edge.To.getPosition());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
