using UnityEngine;

public class PlayerBullet : Pooled {
    private float fSpeed = 3f;

    override public void OnObjectSpawn() {
        fSpeed = 3f;
    }

    private void Update() {
        if (bReset) {
            gameObject.SetActive(false);
        }
        transform.Translate((transform.forward * fSpeed) * Time.deltaTime);
    }
}