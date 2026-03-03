using System;
using UnityEngine;
using Zenject;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private StartingItem[] startingItems;
    [SerializeField] private InventoryModel inventoryModel;
    [SerializeField] private Transform inventoryPosition;

    [Serializable]
    private struct StartingItem
    {
        public ItemModel Model;
        public int Count;

        public bool IsEmpty => Model == null || Count == 0;
    }

    private Inventory inventory;

    public Inventory Inventory => inventory;

    private UIElementFactory uiFactory;

    [Inject]
    public void Construct(UIElementFactory uiFactory)
    {
        this.uiFactory = uiFactory;
    }

    private void Awake()
    {
        inventory = new(inventoryModel);

        for (int i = 0; i < startingItems.Length; i++)
        {
            if (startingItems[i].IsEmpty) continue;
            inventory.TryAddItem(startingItems[i].Model, startingItems[i].Count, i);
        }
    }

    public void OpenInventory()
    {
        uiFactory.CreateInventoryWindow(inventoryPosition.position, inventory);
    }
}
