using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConstellationDrawer : MonoBehaviour {

    public float starRadius = 1f;
    public GameObject linePrefab;
    
    private GameObject stars;
    private GameObject lastStar = null;
    private Plane starPlane;
    private LineRenderer lineRenderer;
    
    void Start() {
        starPlane = new Plane(Vector3.up, transform.position);
        stars = transform.Find("Stars").gameObject;
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != null) {
                GameObject starAtMouse = StarAt(mousePosition);
                if (starAtMouse != null && starAtMouse != lastStar) {
                    if (lastStar != null) {
                        // TODO: Add connection between stars
                        GameObject newLine = GameObject.Instantiate(linePrefab, transform);
                        LineRenderer newLineRenderer = newLine.GetComponent<LineRenderer>();
                        newLineRenderer.SetPosition(0, lastStar.transform.position);
                        newLineRenderer.SetPosition(1, starAtMouse.transform.position);
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
        Transform closestStar = null;
        foreach (Transform child in stars.transform) {
            float distanceSq = (child.position - position).sqrMagnitude;
            if (distanceSq < closestDistanceSq) {
                closestStar = child;
                closestDistanceSq = distanceSq;
            }
        }
        
        if (closestStar != null) {
            return closestStar.gameObject;
        } else {
            return null;
        }
    }
}
