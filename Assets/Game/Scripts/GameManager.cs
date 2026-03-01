using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerInventoryPosition;
    [SerializeField] private Transform NpcInventoryPosition;
    [SerializeField] private InventoryModel playerInventoryModel;
    [SerializeField] private InventoryModel npcInventoryModel;

    private UIElementFactory uiFactory;

    private Inventory playerInventory;

    private Inventory npcInventory;

    private void Awake()
    {
        playerInventory = new(playerInventoryModel);
        npcInventory = new(npcInventoryModel);
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
