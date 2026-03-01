using UnityEngine;
using Zenject;

public class UIElementFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject inventoryViewPrefab;
    [SerializeField] private GameObject inventorySlotPrefab;

    private UIObjectPooler pooler;

    [Inject]
    public void Construct(UIObjectPooler pooler)
    {
        this.pooler = pooler;
    }

    public InventoryView CreateInventoryWindow(Vector2 position, Inventory boundInventory)
    {
        var obj = pooler.SpawnObject<InventoryView>(inventoryViewPrefab, position);
        obj.Bind(boundInventory);
        return obj;
    }

    public InventorySlot CreateInventorySlot(RectTransform parent)
    {
        return pooler.SpawnObject<InventorySlot>(inventorySlotPrefab, Vector2.zero, parent);
    }
}
