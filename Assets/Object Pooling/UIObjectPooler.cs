using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class UIObjectPooler : MonoBehaviour
{
    private Dictionary<GameObject, ObjectPool<GameObject>> objectPools;

    [SerializeField] private RectTransform uiParentTransform;

    private void Awake()
    {
        objectPools = new();
    }

    public T SpawnObject<T>(GameObject objectToSpawn, Vector3 position)
    {
        GameObject objectSourcePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(objectToSpawn);

        if (!objectPools.ContainsKey(objectSourcePrefab))
        {
            //Initialize Pool
            objectPools.Add(objectSourcePrefab, InitializePool(objectSourcePrefab));
        }

        //Spawn
        GameObject spawnedObject = objectPools[objectToSpawn].Get();

        //Set Transform
        spawnedObject.GetComponent<RectTransform>().SetParent(uiParentTransform, false);

        //Set Position
        spawnedObject.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(position);

        //Assign Poolable
        if (!spawnedObject.TryGetComponent(out PoolableObject _))
        {
            PoolableObject poolable = spawnedObject.AddComponent<PoolableObject>();
            poolable.Pool = objectPools[objectToSpawn];
        }

        return spawnedObject.GetComponent<T>();
    }

    private ObjectPool<GameObject> InitializePool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = new(
            OnCreate,
            (p) => p.SetActive(true),
            (p) => p.SetActive(false),
            (p) => DestroyInAnyMode(p),
            true
        );

        GameObject OnCreate()
        {
            GameObject p = Instantiate(prefab);
            p.SetActive(false);
            return p;
        }

        return pool;
    }

    private void DestroyInAnyMode(Object obj)
    {
        if (Application.isPlaying == false)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);
    }
}
