using UnityEngine;

//Base enemy
public class Enemy : Pooled {
    private float fSpeed = 3f;
    private bool bOnScreen;

    [SerializeField]
    private GameObject goPowerUp;

    [SerializeField]
    private GameObject goHitEffect;

    private void Update() {
        if (bReset) {
            gameObject.SetActive(false);
        }
        transform.Translate((-transform.forward * fSpeed) * Time.deltaTime);
    }

    //Do stuff when spawned.
    override public void OnObjectSpawn() {
        fSpeed = Random.Range(3f, 6f);
        bOnScreen = false;
    }

    //Prevents them being shot before being on screen
    private void OnBecameVisible() {
        if (!bOnScreen) {
            bOnScreen = true;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (!bOnScreen) {
            return;
        }
        if (other.collider.CompareTag("Player")) {
            if (other.gameObject.TryGetComponent<PlayerCont>(out PlayerCont player)) {
                player.TryKillPlayer();
            }
            //Kill player.
            Death();
        } else if (other.collider.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
            GameManager.instance.IncrementScore();
            int iRand = Random.Range(0, 20);
            if (iRand >= 15) {
                Instantiate(goPowerUp, transform.position, transform.rotation, null);
            }
            Death();
        }
    }

    public void Death() {
        Instantiate(goHitEffect, transform.position, transform.rotation, null);
        gameObject.SetActive(false);
    }
}