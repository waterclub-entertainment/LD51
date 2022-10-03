using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BookController : MonoBehaviour {
    
    public MeshRenderer image;
    public MeshRenderer nextImage;
    public Material[] images;
    public Animator dayAnimator;
    
    private int currentPage = 0;
    
    public void NextPage() {
        if (currentPage == images.Length - 1) {
            return;
        }
        GetComponent<Animator>().SetTrigger("NextPage");
        currentPage++;
    }
    
    public void PreviousPage() {
        if (currentPage == 0) {
            return;
        }
        GetComponent<Animator>().SetTrigger("PreviousPage");
        currentPage--;
    }
    
    public void Update() {
        bool isPickedUp = !transform.parent.GetComponent<Collider>().enabled;
        if (isPickedUp && Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool didHit = false;
            if (Physics.Raycast(ray, out hitInfo)) {
                if (hitInfo.collider.transform.parent?.parent == transform) {
                    if (hitInfo.collider.transform.GetSiblingIndex() == 0) {
                        PreviousPage();
                    } else {
                        NextPage();
                    }
                    didHit = true;
                }
            }
            if (!didHit) {
                transform.parent.GetComponent<Collider>().enabled = true;
                transform.parent.GetComponent<Animator>().SetTrigger("Zoom");
                dayAnimator.speed = 1f;
            }
        }
    }
    
    public void UpdateImages() {
        image.material = images[currentPage];
        if (currentPage != images.Length - 1) {
            nextImage.material = images[currentPage + 1];
        }
    }

}
