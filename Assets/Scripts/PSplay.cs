using UnityEngine;

public class PSplay : MonoBehaviour {

    [SerializeField]
    private ParticleSystem particleSystem;

    private void Start() {
        //Unparent the particles.
        transform.parent = null;

        //Play the particle system.
        particleSystem.Play();

        //Once the particles have finished, destroy the gameobject they are on.
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}