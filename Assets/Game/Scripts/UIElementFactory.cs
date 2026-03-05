using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class UIElementFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject inventoryViewPrefab;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemViewPrefab;

    private UIObjectPooler pooler;

    [Inject]
    public void Construct(UIObjectPooler pooler)
    {
        this.pooler = pooler;
    }

    public InventoryView CreateInventoryWindow(Vector2 position, Inventory boundInventory, string inventoryName)
    {
        var obj = pooler.SpawnObject<InventoryView>(inventoryViewPrefab, position);
        obj.InventoryName = inventoryName;
        obj.Bind(boundInventory);
        return obj;
    }

    public InventorySlot CreateInventorySlot(RectTransform parent, Inventory owner)
    {
        var obj = pooler.SpawnObject<InventorySlot>(inventorySlotPrefab, Vector2.zero, parent);
        obj.Owner = owner;
        return obj;
    }

    public ItemView CreateItemView(Item boundItem)
    {
        var obj = pooler.SpawnObject<ItemView>(itemViewPrefab, Vector2.zero);
        obj.Bind(boundItem);
        return obj;
    }
}
