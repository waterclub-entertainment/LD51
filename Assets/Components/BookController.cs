using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BookController : MonoBehaviour {
    
    public MeshRenderer image;
    public MeshRenderer diagramm;
    public MeshRenderer nextImage;
    public MeshRenderer nextDiagramm;
    public Material[] images;
    public Animator dayAnimator;
    public Image closeButton;
    
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
                GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }
    
    public void UpdateImagesNext() {
        image.material = images[currentPage];
        Material[] materials = new Material[] {
            diagramm.materials[0],
            diagramm.materials[1],
            images[currentPage]
        };
        diagramm.materials = materials;
        StartCoroutine(DelayedUpdateImagesNext());
    }
    
    private IEnumerator DelayedUpdateImagesNext() {
        yield return new WaitForSecondsRealtime(0f);
        if (currentPage != images.Length - 1) {
            nextDiagramm.material = images[currentPage + 1];
            Material[] materials = new Material[] {
                nextImage.materials[0],
                nextImage.materials[1],
                images[currentPage + 1]
            };
            nextImage.materials = materials;
        }
        yield return new WaitForSecondsRealtime(0f);
        image.transform.parent.rotation = Quaternion.identity;
    }

    public void UpdateImagesPrev() {
        if (currentPage != images.Length - 1) {
            nextDiagramm.material = images[currentPage + 1];
            Material[] materials = new Material[] {
                nextImage.materials[0],
                nextImage.materials[1],
                images[currentPage + 1]
            };
            nextImage.materials = materials;
        }
        StartCoroutine(DelayedUpdateImagesPrev());
    }
    
    private IEnumerator DelayedUpdateImagesPrev() {
        yield return new WaitForSecondsRealtime(0f);
        image.material = images[currentPage];
        Material[] materials = new Material[] {
            diagramm.materials[0],
            diagramm.materials[1],
            images[currentPage]
        };
        diagramm.materials = materials;
        yield return new WaitForSecondsRealtime(0f);
        image.transform.parent.rotation = Quaternion.identity;
    }

}
