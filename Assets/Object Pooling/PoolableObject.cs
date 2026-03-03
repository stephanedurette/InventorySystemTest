using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour
{
    public IObjectPool<GameObject> Pool; // Reference to the pool instance
    //public RectTransform ParentReturnTransform;

    void OnDisable()
    {
        //(transform as RectTransform).SetParent(ParentReturnTransform, false);
        Pool?.Release(this.gameObject);
        //transform.localScale = Vector3.one;
    }
}
