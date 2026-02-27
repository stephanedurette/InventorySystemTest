using UnityEngine;

public class UIElementFactory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIObjectPooler pooler;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventoryViewPrefab;

    public InventoryView CreateInventoryWindow(Vector2 position)
    {
        return pooler.SpawnObject<InventoryView>(inventoryViewPrefab, position);
    }
}
