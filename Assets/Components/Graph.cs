using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode {
    public Vector3 getPosition();
}

public interface Node<TNodeType> : INode
{
    public TNodeType getValue();
    public void setValue(TNodeType t);

    public Color getValue();
}

public class Edge<TEdgeType>
{
    public Color GizmoColor { get; set; }
    public TEdgeType Value { get; set; }

    public INode From { get; set; }
    public INode To { get; set; }

}

public class Graph<TNodeType, TEdgeType>
{
    public Graph()
    {
        Nodes = new List<Node<TNodeType>>();
        Edges = new List<Edge<TEdgeType>>();
    }
    public List<Node<TNodeType>> Nodes { get; private set; }
    public List<Edge<TEdgeType>> Edges { get; private set; }
}