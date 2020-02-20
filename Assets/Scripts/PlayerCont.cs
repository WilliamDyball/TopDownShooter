using System.Collections;
using UnityEngine;

public class PlayerCont : MonoBehaviour {
    private Camera camMain;

    [SerializeField]
    private Transform transFirePos;

    [SerializeField]
    private Transform transStartPos;

    [SerializeField]
    private float fFireRate;

    [SerializeField]
    private float fNextFire;

    private bool bImmune;
    private float fImmuneMax = 3f;
    private float fImmuneTimer;

    private bool bAutoFire;
    private bool bResapawn;

    [SerializeField]
    private GameObject goShield;

    private void Awake() {
        camMain = Camera.main;
    }

    private void Update() {
        if (bResapawn) {
            return;
        }
        if (bAutoFire) {
            if (Time.time > fNextFire) {
                fNextFire = (Time.time + fFireRate);
                BulletPool.instance.SpawnObjFromPool("BaseBullet", transFirePos.position, Quaternion.identity);
            }
        }
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector3 v3TouchPos = camMain.ScreenToWorldPoint(touch.position);
            v3TouchPos.z = Mathf.Clamp(v3TouchPos.z, 2.5f, 6f); //Limit the player position to the lower part of the screen.
            transform.position = new Vector3(v3TouchPos.x, 0, v3TouchPos.z);
            if (!bAutoFire) {
                //Spawn Bullets
                if (Time.time > fNextFire) {
                    fNextFire = (Time.time + fFireRate);
                    BulletPool.instance.SpawnObjFromPool("BaseBullet", transFirePos.position, Quaternion.identity);
                }
            }
        }
        if (fImmuneTimer >= 0) {
            fImmuneTimer -= Time.deltaTime;
            if (!goShield.activeSelf) {
                goShield.SetActive(true);
            }
            bImmune = true;
        } else {
            if (goShield.activeSelf) {
                goShield.SetActive(false);
            }
            bImmune = false;
        }
    }

    public void TryKillPlayer() {
        if (bImmune) {
            return;
        }
        GameManager.instance.PlayerDeath();
        StartCoroutine(IERespawn());
    }

    private readonly WaitForSeconds respawnDuration = new WaitForSeconds(.5f);

    private IEnumerator IERespawn() {
        transform.position = transStartPos.position;
        Pooled.bReset = true;
        bResapawn = true;
        yield return respawnDuration;
        Pooled.bReset = false;
        bResapawn = false;
    }

    public void SetImmune() {
        fImmuneTimer = fImmuneMax;
    }

    public void IncreaseFireRate() {
        fFireRate -= .02f;
        Mathf.Clamp(fFireRate, 0.15f, 1f);
    }
}