using UnityEngine;
using UnityEngine.Events;

public class MouseTrigger : MonoBehaviour
{
    public UnityEvent onClick;

    void OnMouseDown() {
        onClick.Invoke();
    }
}
