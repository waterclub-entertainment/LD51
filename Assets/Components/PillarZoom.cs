using UnityEngine;

public class PillarZoom : MonoBehaviour {

    public float zoom = 0f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start() {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update() {
        transform.position = Vector3.Lerp(originalPosition, targetPosition, zoom);
        transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, zoom);
    }
    
    public void SetTarget(Transform transform) {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }
}
