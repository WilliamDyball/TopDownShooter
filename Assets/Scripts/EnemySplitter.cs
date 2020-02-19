using UnityEngine;

//An enemy that splits in two when shot.
public class EnemySplitter : Pooled {
    private float fSpeed = 3f;
    private bool bOnScreen;

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
            Death();
        }
    }

    public void Death() {
        EnemyManager.instance.SpawnObjFromPool("EnemyBase", new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), transform.rotation);
        EnemyManager.instance.SpawnObjFromPool("EnemyBase", new Vector3(transform.position.x + .5f, transform.position.y, transform.position.z), transform.rotation);
        gameObject.SetActive(false);
    }
}