using UnityEngine;

//Used to scroll the background image
public class BackgroundScroll : MonoBehaviour {
    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }

    private void Update() {
        var newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ); //Loops the background by having the position reset once it reaches 0
        transform.position = startPosition + Vector3.forward * newPosition;
    }
}