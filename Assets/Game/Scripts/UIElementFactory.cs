using UnityEngine;

public class UIElementFactory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIObjectPooler pooler;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventoryViewPrefab;
    [SerializeField] private GameObject inventorySlotPrefab;

    public InventoryView CreateInventoryWindow(Vector2 position)
    {
        return pooler.SpawnObject<InventoryView>(inventoryViewPrefab, position);
    }

    public InventorySlot CreateInventorySlot(RectTransform parent)
    {
        return pooler.SpawnObject<InventorySlot>(inventorySlotPrefab, Vector2.zero, parent);
    }
}
