using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class ConstellationDrawer : MonoBehaviour {

    public float starRadius = 1f;
    public GameObject linePrefab;
    public GameObject[] constellations;
    
    private Dictionary<IConstellation.Connection, GameObject> lines;
    private List<GameObject> stars;
    private GameObject currentConstellation = null;
    private GameObject lastStar = null;
    private Plane starPlane;
    private LineRenderer lineRenderer;
    private int nextConstellation = 0;
    
    void Start() {
        stars = new List<GameObject>();
        lines = new Dictionary<IConstellation.Connection, GameObject>(new IConstellation.ConnectionComparer());
        starPlane = new Plane(Vector3.up, transform.position);
        lineRenderer = GetComponent<LineRenderer>();
        LoadNextConstellation();
    }
    
    void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != null) {
                GameObject starAtMouse = StarAt(mousePosition);
                if (starAtMouse != null && starAtMouse != lastStar) {
                    if (lastStar != null) {
                        // TODO: Add connection between stars
                        IConstellation.Connection connection = new IConstellation.Connection();
                        connection.from = lastStar.GetComponent<Star>().ID;
                        connection.to = starAtMouse.GetComponent<Star>().ID;
                        Debug.Log(connection.from + ", " + connection.to);
                        
                        if (!lines.ContainsKey(connection)) {
                            GameObject newLine = GameObject.Instantiate(linePrefab, transform);
                            LineRenderer newLineRenderer = newLine.GetComponent<LineRenderer>();
                            newLineRenderer.SetPosition(0, lastStar.transform.position);
                            newLineRenderer.SetPosition(1, starAtMouse.transform.position);
                            lines.Add(connection, newLine);
                            if (ConstellationMatches()) {
                                LoadNextConstellation();
                            }
                        }
                    }
                    lastStar = starAtMouse;
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, lastStar.transform.position);
                }
                lineRenderer.SetPosition(1, mousePosition);
            }
        } 
        if (Input.GetMouseButtonUp(0)) {
            GetComponent<LineRenderer>().enabled = false;
            lastStar = null;
        }
    }
    
    private Vector3 MousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float d;
        if (!starPlane.Raycast(ray, out d)) {
            return ray.GetPoint(0);
        }
        return ray.GetPoint(d);
    }
    
    private GameObject StarAt(Vector3 position) {
        float closestDistanceSq = starRadius * starRadius;
        GameObject closestStar = null;
        foreach (GameObject star in stars) {
            float distanceSq = (star.transform.position - position).sqrMagnitude;
            if (distanceSq < closestDistanceSq) {
                closestStar = star;
                closestDistanceSq = distanceSq;
            }
        }
        
        if (closestStar != null) {
            return closestStar;
        } else {
            return null;
        }
    }
    
    private bool ConstellationMatches() {
        IConstellation constellation = currentConstellation.GetComponent<IConstellation>();

        foreach (IConstellation.Connection line in lines.Keys) {
            if (!constellation.HasConnection(line.from, line.to)) {
                return false;
            }
        }
        
        foreach (IConstellation.Connection connection in constellation.GetConnections()) {
            if (!lines.ContainsKey(connection)) {
                return false;
            }
        }

        return true;
    }
   
    private void LoadNextConstellation() {
        if (nextConstellation >= constellations.Length) {
            // TODO: Win or sth
            Debug.Log("WIN");
            return;
        }
        LoadConstellation(nextConstellation);
        nextConstellation++;
    }
    
    private void LoadConstellation(int index) {
        if (currentConstellation != null) {
            GameObject.Destroy(currentConstellation);
        }
        foreach (GameObject line in lines.Values) {
            GameObject.Destroy(line);
        }
        lines.Clear();
        stars.Clear();

        currentConstellation = GameObject.Instantiate(constellations[index], transform);
        
        Constellation constellation = currentConstellation.GetComponent<Constellation>();
        constellation.root = transform.Find("Stars").gameObject;
       
        foreach (Transform child in transform.Find("Stars")) {
            Star star = child.GetComponent<Star>();
            if (constellation.ContainsStar(star.ID)) {
                stars.Add(child.gameObject);
            }
        }
    }
}
