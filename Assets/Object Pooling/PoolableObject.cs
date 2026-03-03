using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour
{
    public IObjectPool<GameObject> Pool;

    void OnDisable()
    {
        Pool?.Release(this.gameObject);
    }
}
