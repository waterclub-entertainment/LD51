using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Constellation))]
public class ConstellationDrawer : MonoBehaviour {

    public float starRadius = 1f;
    public GameObject linePrefab;
    public GameObject[] constellations;
    public float clickTime = 0.25f;
    
    private Constellation constellation;
    private Constellation referenceConstellation = null;
    private Star lastStar = null;
    private Plane starPlane;
    private LineRenderer lineRenderer;
    private int nextConstellation = 0;
    private float mouseDownTime = 0;
    
    void Start() {
        constellation = GetComponent<Constellation>();
        starPlane = new Plane(Vector3.up, transform.position);
        lineRenderer = GetComponent<LineRenderer>();
        LoadNextConstellation();
    }
    
    void Update() {
        Vector3 mousePosition = MousePosition();
        if (Input.GetMouseButtonDown(0)) {
            mouseDownTime = Time.unscaledTime;
        }
        if (Input.GetMouseButtonUp(0) && Time.unscaledTime - mouseDownTime <= clickTime) {
            GameObject lineAtMouse = LineAt(mousePosition);
            if (lineAtMouse != null) {
                constellation.RemoveConnection(lineAtMouse);
                if (constellation.Matches(referenceConstellation)) {
                    LoadNextConstellation();
                }
            }
        }
        if (Input.GetMouseButton(0)) {
            if (mousePosition != null) {
                Star starAtMouse = StarAt(mousePosition);
                if (starAtMouse == null && lastStar != null) {
                    starAtMouse = StarOnLine(lastStar.transform.position, mousePosition);
                }
                if (starAtMouse != null && starAtMouse != lastStar) {
                    if (lastStar != null) {
                        Constellation.Connection connection = new Constellation.Connection();
                        connection.from = lastStar.GetComponent<Star>().ID;
                        connection.to = starAtMouse.GetComponent<Star>().ID;
                        
                        if (constellation.AddConnection(connection)) {
                            if (constellation.Matches(referenceConstellation)) {
                                LoadNextConstellation();
                                return;
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
    
    private Star StarOnLine(Vector3 from, Vector3 to) {
        int layerMask = 1 << 6; // Only stars
        
        RaycastHit hitInfo;

        if (Physics.Linecast(from, to, out hitInfo, layerMask)) {
            return hitInfo.transform.GetComponent<Star>();
        }
        
        return null;
    }
    
    private GameObject LineAt(Vector3 position) {
        int layerMask = 1 << 7; // Only lines
        Collider[] colliders = Physics.OverlapSphere(position, 0, layerMask);
        foreach (Collider collider in colliders) {
            if (collider.transform.parent == transform) {
                return collider.gameObject;
            }
        }
        
        return null;
    }
    
    private void LoadNextConstellation() {
        if (nextConstellation >= constellations.Length) {
            // TODO: Win or sth
            Debug.Log("WIN");
            SceneManager.LoadScene(sceneName:"Scenes/Win Scene");
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
        lastStar = null;
        lineRenderer.enabled = false;

        GameObject reference = GameObject.Instantiate(constellations[index], transform);
        
        referenceConstellation = reference.GetComponent<Constellation>();
        referenceConstellation.root = constellation.root;
        referenceConstellation.SetLineWidth(0.1f);

        foreach (Transform child in constellation.root.transform) {
            child.gameObject.SetActive(
                referenceConstellation.usedStars.Contains(child.GetComponent<Star>()));
        }
    }
}
