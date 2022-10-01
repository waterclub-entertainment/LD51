using System.Collections.Generic;
using UnityEngine;
using System;


public class Constellation : MonoBehaviour
{
	[Serializable]
	public class Connection
	{
	    public int from, to;
	}

    public class ConnectionComparer : IEqualityComparer<Connection>
    {

        bool IEqualityComparer<Connection>.Equals(Connection x, Connection y)
        {
            return (x.from == y.from && x.to == y.to) || (x.from == y.to && x.to == y.from);
        }

        int IEqualityComparer<Connection>.GetHashCode(Connection obj)
        {
			return Math.Min(obj.from, obj.to) * 1000 + Math.Max(obj.from, obj.to);
        }
    }

    public float NodeGizmoSize;
    public Constellation.Connection[] Connections = new Connection[0];
    public GameObject root;
    public GameObject linePrefab;

    private Dictionary<int, Star> _starReference;
    public Dictionary<int, Star> starReference {
        get {
            if (_starReference == null) {
                _starReference = new Dictionary<int, Star>();
                var r = root ? root : gameObject;
                foreach (Transform child in r.transform)
                {
                    Star obj = child.gameObject.GetComponent<Star>();
                    if (obj != null)
                    {
                        _starReference.Add(obj.getValue(), obj);
                    }
                }
            }
            return _starReference;
        }
    }
    private HashSet<Star> _usedStars;
    public HashSet<Star> usedStars {
        get {
            if (_usedStars == null) {
                _usedStars = new HashSet<Star>();
                foreach (Connection c in Connections)
                {
                    _usedStars.Add(starReference[c.from]);
                    _usedStars.Add(starReference[c.to]);
                }
            }
            return _usedStars;
        }
    }
    private Dictionary<Constellation.Connection, GameObject> lines;
    private float lineWidth = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        lines = new Dictionary<Constellation.Connection, GameObject>(new Constellation.ConnectionComparer());
        foreach (Constellation.Connection c in Connections) {
            AddConnection(c);
        }
    }

    void OnDrawGizmos()
    {
        // foreach (var node in graph.Nodes)
        // {
        //     Gizmos.color = node.getColor();
        //     Gizmos.DrawSphere(node.getPosition(), NodeGizmoSize);

        //     Handles.color = Color.black;
        //     Handles.Label(node.getPosition() + new Vector3(NodeGizmoSize, 0, NodeGizmoSize), node.getValue().ToString());
        // }
        foreach (Constellation.Connection c in Connections)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(starReference[c.from].transform.position, starReference[c.to].transform.position);
        }
    }

    // Return all connections
    public bool Matches(Constellation other)
    {
        if (lines.Count != other.lines.Count) {
            return false;
        }
        
        foreach (Connection c in lines.Keys) {
            if (!other.lines.ContainsKey(c)) {
                return false;
            }
        }

        return true;
    }
    
    public bool AddConnection(Connection c) {
        if (lines.ContainsKey(c)) {
            return false;
        }
        GameObject line = GameObject.Instantiate(linePrefab, transform);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, starReference[c.from].transform.position);
        lineRenderer.SetPosition(1, starReference[c.to].transform.position);
        lineRenderer.widthMultiplier = lineWidth;
        lines.Add(c, line);
        return true;
    }
    
    public void Clear() {
        foreach (GameObject line in lines.Values) {
            GameObject.Destroy(line);
        }
        lines.Clear();
    }
    
    public void SetLineWidth(float lineWidth) {
        this.lineWidth = lineWidth;
        if (lines != null) {
            foreach (GameObject line in lines.Values) {
                line.GetComponent<LineRenderer>().widthMultiplier = lineWidth;
            }
        }
    }

}
