using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode {
    public Vector3 getPosition();
}

public interface BaseNode<TNodeType> : INode
{
    public TNodeType getValue();
    public void setValue(TNodeType t);

    public Color getColor();
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
        Nodes = new List<BaseNode<TNodeType>>();
        Edges = new List<Edge<TEdgeType>>();
    }
    public List<BaseNode<TNodeType>> Nodes { get; private set; }
    public List<Edge<TEdgeType>> Edges { get; private set; }
}