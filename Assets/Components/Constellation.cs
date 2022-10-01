using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class Constellation : MonoBehaviour, IConstellation
{
    private Graph<int, float> graph;

    public float NodeGizmoSize;
    public IConstellation.Connection[] Connections;
    public GameObject root;

    private Dictionary<int, Star> starReference;
    private HashSet<int> usedStars;


    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph<int, float>();
        starReference = new Dictionary<int, Star>();
        usedStars = new HashSet<int>();
        //collect child star objects
        var r = root ? root : gameObject;
        foreach (Transform child in r.transform)
        {
            Star obj = child.gameObject.GetComponent<Star>();
            if (obj != null)
            {
                starReference.Add(obj.getValue(), obj);
                graph.Nodes.Add(obj);
            }
        }

        foreach (IConstellation.Connection c in Connections)
        {
            usedStars.Add(c.from);
            usedStars.Add(c.to);

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

            Handles.color = Color.black;
            Handles.Label(node.getPosition() + new Vector3(NodeGizmoSize, 0, NodeGizmoSize), node.getValue().ToString());
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

    // Return true iff star is used in this constellation
    public bool ContainsStar(int id)
    {
        foreach (IConstellation.Connection connection in Connections) {
            if (connection.from == id || connection.to == id) {
                return true;
            }
        }
        return false;
    }

    // Return all connections
    public IConstellation.Connection[] GetConnections()
    {
        return Connections;
    }

    // Return true iff there is a connection between stars with ids `from` and `to`
    // (or the other way round)
    public bool HasConnection(int from, int to)
    {
        foreach (IConstellation.Connection c in Connections)
        {
            if ((c.from == from && c.to == to) || (c.to == from && c.from == to))
            {
                return true;
            }
        }
        return false;
    }
}
