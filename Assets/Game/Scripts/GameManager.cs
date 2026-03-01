using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerInventoryPosition;
    [SerializeField] private Transform NpcInventoryPosition;
    [SerializeField] private InventoryModel playerInventoryModel;
    [SerializeField] private InventoryModel npcInventoryModel;
    [SerializeField] private StartingItem[] playerStartingItems;

    [Serializable]
    private struct StartingItem
    {
        public ItemModel Model;
        public int Count;

        public bool IsEmpty => Model == null || Count == 0;
    }

    private UIElementFactory uiFactory;

    private Inventory playerInventory;

    private Inventory npcInventory;

    private void Awake()
    {
        playerInventory = new(playerInventoryModel);
        npcInventory = new(npcInventoryModel);

        for(int i = 0; i < playerStartingItems.Length; i++) {
            if (playerStartingItems[i].IsEmpty) continue;
            playerInventory.AddItem(playerStartingItems[i].Model, playerStartingItems[i].Count, i);
        }
    }

    [Inject]
    public void Construct(UIElementFactory uiFactory)
    {
        this.uiFactory = uiFactory;
    }

    public void OpenPlayerInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(playerInventoryPosition.position, playerInventory);
    }

    public void OpenNPCInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(NpcInventoryPosition.position, npcInventory);
    }
}
