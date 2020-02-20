using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to store and manage the pool of enemies.
public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private Transform transSpawn;

    private Coroutine spawnCoroutine;
    public bool bSpawning;
    public static EnemyManager instance;
    private int iRandSpawnMin;
    private int iRandSpawnType;

    private void Awake() {
        if (EnemyManager.instance == null) {
            EnemyManager.instance = this;
        } else if (EnemyManager.instance != this) {
            Destroy(EnemyManager.instance.gameObject);
            EnemyManager.instance = this;
        }
        bSpawning = false;
    }

    [System.Serializable]
    public class Pool {
        public string strName;
        public GameObject goPrefab;
        public int iSize;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDict;

    private void Start() {
        PoolDict = new Dictionary<string, Queue<GameObject>>();
        for (int i = 0; i < pools.Count; i++) {
            Queue<GameObject> objects = new Queue<GameObject>();
            for (int j = 0; j < pools[i].iSize; j++) {
                GameObject obj = Instantiate(pools[i].goPrefab);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }
            PoolDict.Add(pools[i].strName, objects);
        }
    }

    public GameObject SpawnObjFromPool(string _strName, Vector3 _v3Pos, Quaternion _qRot) {
        if (!PoolDict.ContainsKey(_strName)) {
            Debug.LogError("The pool dictionary does not contain: " + _strName);
            return null;
        }
        GameObject objToSpawn = PoolDict[_strName].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = _v3Pos;
        objToSpawn.transform.rotation = _qRot;

        if (objToSpawn.TryGetComponent<Pooled>(out Pooled pooled)) {
            pooled.OnObjectSpawn();
        }

        PoolDict[_strName].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public void StartSpawning() {
        if (bSpawning) {
            return;
        }
        spawnCoroutine = StartCoroutine(IESpawnEnemies());
    }

    public void StopSpawning() {
        StopCoroutine(spawnCoroutine);
    }

    private WaitForSeconds wait = new WaitForSeconds(.5f);

    private IEnumerator IESpawnEnemies() {
        Debug.Log("Starting coroutine.");
        bSpawning = true;
        while (bSpawning) {
            yield return wait;
            if (Random.Range(iRandSpawnMin, 10) > 5) {
                iRandSpawnMin = 0;
                Vector3 v3SpawnPos = new Vector3(Random.Range(-3.5f, 3.5f), 0f, transSpawn.position.z);
                Quaternion qRot = transSpawn.rotation;
                switch (Random.Range(0, 2)) {
                    case 0:
                        SpawnObjFromPool("EnemyBase", v3SpawnPos, qRot);
                        break;
                    case 1:
                        SpawnObjFromPool("EnemySplit", v3SpawnPos, qRot);
                        break;
                    default:
                        SpawnObjFromPool("EnemyBase", v3SpawnPos, qRot);
                        break;
                }
            } else {
                iRandSpawnMin++;
            }
        }
    }
}