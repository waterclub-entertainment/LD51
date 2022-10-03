using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ConstellationGroup
{
    public GameObject constellationPrefab;
    public GameObject statue;
}

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Constellation))]
public class ConstellationDrawer : MonoBehaviour {

    public float starRadius = 1f;
    public GameObject linePrefab;
    public ConstellationGroup[] constellations;
    public float clickTime = 0.25f;
    public float lineMultiplier = 1; //varaible for animation to centralize animation for all children
    public float referenceLineSize = 0.2f;
    public AudioClip successSound;

    private HashSet<int> completedConstellations;
    private Constellation constellation;
    private Constellation referenceConstellation = null;
    private Star lastStar = null;
    private Plane starPlane;
    private LineRenderer lineRenderer;
    private int currentConstellation = -1;
    private float mouseDownTime = 0;
    
    void Start() {
        constellation = GetComponent<Constellation>();
        starPlane = new Plane(Vector3.up, transform.position);
        lineRenderer = GetComponent<LineRenderer>();
        completedConstellations = new HashSet<int>();
    }

    void Update() {
        SetLineSize(referenceLineSize * lineMultiplier * (1.0f + Mathf.Pow(Mathf.Sin(Time.time), 2))); //this line effectively serves to forward the animation data in the multiplier to the objects
        Vector3 mousePosition = MousePosition();
        if (Input.GetMouseButtonDown(0)) {
            mouseDownTime = Time.unscaledTime;
        }
        if (Input.GetMouseButtonUp(0) && Time.unscaledTime - mouseDownTime <= clickTime) {
            GameObject lineAtMouse = LineAt(mousePosition);
            if (lineAtMouse != null) {
                constellation.RemoveConnection(lineAtMouse);
                if (constellation.Matches(referenceConstellation)) {
                    HandleConstellationCompletion();
                } else {
                    GetComponent<RandomSound>().PlayRandomSound();
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
                                HandleConstellationCompletion();
                                return;
                            } else {
                                GetComponent<RandomSound>().PlayRandomSound();
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
    
    private void HandleConstellationCompletion()
    {
        GetComponent<AudioSource>().PlayOneShot(successSound);
        completedConstellations.Add(currentConstellation);
        if (completedConstellations.Count == 12)
        {
            SceneManager.LoadScene(sceneName:"Scenes/Win Scene");
            return;
        }
        
        if (constellations[currentConstellation].statue != null)
        {
            foreach (Transform child in constellations[currentConstellation].statue.transform)
            {
                if (child.gameObject.name != "Plane") //not pretty
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
            }
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
    
    public void LoadNextConstellation() {
        currentConstellation = (currentConstellation + 1) % constellations.Length;

        // skip all completed
        while (completedConstellations.Contains(currentConstellation))
            currentConstellation = (currentConstellation + 1) % constellations.Length;

        LoadConstellation(currentConstellation);
        GetComponent<Animator>().ResetTrigger("FadeOut");
        GetComponent<Animator>().SetTrigger("FadeIn");

    }
    
    private void UnloadConstellation()
    {
        if (referenceConstellation != null)
        {
            GameObject.Destroy(referenceConstellation.gameObject);
        }
        foreach (Transform star in constellation.root.transform)
        {
            star.GetComponent<SphereCollider>().radius = 0f;
            star.localScale = Vector3.one * 0.3f;
        }
        constellation.Clear();
        lastStar = null;
        lineRenderer.enabled = false;
        Debug.Log("Unloaded Constellation");
    }

    private void LoadConstellation(int index) {
        UnloadConstellation();

        GameObject reference = GameObject.Instantiate(constellations[index].constellationPrefab, transform);
        reference.tag = "constellation";

        referenceConstellation = reference.GetComponent<Constellation>();
        referenceConstellation.root = constellation.root;
        SetLineSize(referenceLineSize * lineMultiplier);

        foreach (Star star in referenceConstellation.usedStars) {
            star.gameObject.GetComponent<SphereCollider>().radius = 0.5f;
            star.transform.localScale = Vector3.one;
        }
        Debug.Log("Loaded Constellation " + index.ToString());
    }

    //TODO maybe move iteration to Constellation.cs
    public void SetLineSize(float val)
    {
        if (referenceConstellation == null)
            return;
        referenceConstellation.SetLineWidth(val);
    }

}
