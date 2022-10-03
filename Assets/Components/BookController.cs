using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BookController : MonoBehaviour {
    
    public MeshRenderer image;
    public MeshRenderer nextImage;
    public Material[] images;
    
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
    
    public void UpdateImages() {
        image.materials[0] = images[currentPage];
        if (currentPage != images.Length - 1) {
            nextImage.materials[0] = images[currentPage + 1];
        }
    }

}
