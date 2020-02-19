using System.Collections.Generic;
using UnityEngine;

//Used to store and manage the pool of bullets.
public class BulletPool : MonoBehaviour {
    public static BulletPool instance;

    private void Awake() {
        if (BulletPool.instance == null) {
            BulletPool.instance = this;
        } else if (BulletPool.instance != this) {
            Destroy(BulletPool.instance.gameObject);
            BulletPool.instance = this;
        }
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
}