using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour
{
    public IObjectPool<GameObject> Pool; // Reference to the pool instance

    void OnDisable()
    {
        Pool?.Release(this.gameObject);
    }
}
