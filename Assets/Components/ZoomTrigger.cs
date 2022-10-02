using UnityEngine;

public class ZoomTrigger : MonoBehaviour {

    public GameObject target;

    void OnMouseDown() {
        Camera.main.GetComponent<PillarZoom>().SetTarget(target.transform);
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
    }
}
