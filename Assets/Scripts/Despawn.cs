using UnityEngine;

public class Despawn : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
        }
    }
}