using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private UIElementFactory uiElementFactory;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Scrollbar scrollbar;

    public Inventory BoundInventory { get; private set; }

    private UIElementFactory uiElementFactory;

    private InventorySlot[] inventorySlots;

    [Inject]
    public void Construct(UIElementFactory uIElementFactory)
    {
        this.uiElementFactory = uIElementFactory;
    }

    public void Bind(Inventory inventory)
    {
        BoundInventory = inventory;
        inventory.OnItemAdded += OnItemAdded;
        inventory.OnItemRemoved += OnItemRemoved;

        InitializeWindow();
    }

    public void Unbind()
    {
        if (BoundInventory != null) {
            BoundInventory.OnItemAdded -= OnItemAdded;
            BoundInventory.OnItemRemoved -= OnItemRemoved;
            BoundInventory = null;

            CleanupWindow();
        }
    }

    private void OnItemAdded(int index, Item item)
    {

    }

    private void OnItemRemoved(int index)
    {

    }

    private void CleanupWindow()
    {
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void InitializeWindow()
    {
        SetGridCellSize();
        CreateInventorySlots();

        //add itemviews to slots

        scrollbar.value = 1;
    }

    private void SetGridCellSize()
    {
        gridLayoutGroup.constraintCount = BoundInventory.Model.ColumnCount;

        float widthWithoutPadding = (gridLayoutGroup.transform as RectTransform).rect.width - gridLayoutGroup.padding.right - gridLayoutGroup.padding.left;
        float totalSpacing = (gridLayoutGroup.constraintCount - 1) * gridLayoutGroup.spacing.x;
        float slotSize = (widthWithoutPadding - totalSpacing) / gridLayoutGroup.constraintCount;

        gridLayoutGroup.cellSize = Vector2.one * slotSize;
    }

    private void CreateInventorySlots()
    {
        inventorySlots = new InventorySlot[BoundInventory.Model.Size];
        for (int i = 0; i < BoundInventory.Model.Size; i++)
        {
            var slot = uiElementFactory.CreateInventorySlot(gridLayoutGroup.transform as RectTransform);
            inventorySlots[i] = slot;
        }
    }

    private void OnDisable()
    {
        Unbind();
    }
}
