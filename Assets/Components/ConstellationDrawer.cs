using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Constellation))]
public class ConstellationDrawer : MonoBehaviour {

    public float starRadius = 1f;
    public GameObject linePrefab;
    public GameObject[] constellations;
    
    private Constellation constellation;
    private Constellation referenceConstellation = null;
    private Star lastStar = null;
    private Plane starPlane;
    private LineRenderer lineRenderer;
    private int nextConstellation = 0;
    
    void Start() {
        constellation = GetComponent<Constellation>();
        starPlane = new Plane(Vector3.up, transform.position);
        lineRenderer = GetComponent<LineRenderer>();
        LoadNextConstellation();
    }
    
    void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != null) {
                Star starAtMouse = StarAt(mousePosition);
                if (starAtMouse != null && starAtMouse != lastStar) {
                    if (lastStar != null) {
                        Constellation.Connection connection = new Constellation.Connection();
                        connection.from = lastStar.GetComponent<Star>().ID;
                        connection.to = starAtMouse.GetComponent<Star>().ID;
                        
                        if (constellation.AddConnection(connection)) {
                            if (constellation.Matches(referenceConstellation)) {
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
    
    private Star StarAt(Vector3 position) {
        int layerMask = 1 << 6; // Only stars
        Collider[] colliders = Physics.OverlapSphere(position, 0, layerMask);
        float closestDistanceSq = Mathf.Infinity;
        Star closestStar = null;
        foreach (Collider collider in colliders) {
            Star star = collider.gameObject.GetComponent<Star>();
            if (!referenceConstellation.usedStars.Contains(star)) {
                continue;
            }
            float distanceSq = (collider.transform.position - position).sqrMagnitude;
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
        if (referenceConstellation != null) {
            GameObject.Destroy(referenceConstellation.gameObject);
        }
        constellation.Clear();

        GameObject reference = GameObject.Instantiate(constellations[index], transform);
        
        referenceConstellation = reference.GetComponent<Constellation>();
        referenceConstellation.root = constellation.root;
        referenceConstellation.SetLineWidth(0.1f);
    }
}
