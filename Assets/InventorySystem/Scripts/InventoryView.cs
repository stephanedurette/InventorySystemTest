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

    internal static InventorySlot CurrentHoveredInventorySlot { get; private set; }

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
        }
    }

    private void OnItemAdded(int index, Item item)
    {
        ItemView newItemView = uiElementFactory.CreateItemView(item);
        inventorySlots[index].Bind(newItemView);
    }

    private void OnItemRemoved(int index)
    {
        inventorySlots[index].Unbind();
    }

    private void CleanupSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false);
            slot.OnMouseDown -= OnSlotMouseDown;
            slot.OnMouseUp -= OnSlotMouseUp;
            slot.OnMouseEnter -= OnSlotPointerEnter;
            slot.OnMouseExit -= OnSlotPointerExit;
        }
    }

    private void InitializeWindow()
    {
        SetGridCellSize();
        CreateInventorySlots();
        AssignItemsToSlots();

        scrollbar.value = 1;
    }

    private void AssignItemsToSlots()
    {
        for (int i = 0; i < BoundInventory.Items.Length; i++)
        {
            var item = BoundInventory.Items[i];
            var slot = inventorySlots[i];

            if (item == null) continue;

            ItemView newItemView = uiElementFactory.CreateItemView(item);
            slot.Bind(newItemView);
        }
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

            slot.OnMouseDown += OnSlotMouseDown;
            slot.OnMouseUp += OnSlotMouseUp;
            slot.OnMouseEnter += OnSlotPointerEnter;
            slot.OnMouseExit += OnSlotPointerExit;

            inventorySlots[i] = slot;
        }

        SortInventorySlotsAccordingToIndex();
    }

    private void OnSlotMouseDown(int slotIndex)
    {
        Debug.Log(slotIndex);
        if (inventorySlots[slotIndex].BoundItemView == null) return;

        Inventory.DraggedItem = inventorySlots[slotIndex].BoundItemView.BoundItem;
    }

    private void OnSlotMouseUp(int slotIndex)
    {
        Debug.Log(slotIndex);

        if (CurrentHoveredInventorySlot != null) {
            //Debug.Log("released over slot");
        } else
        {
            //Debug.Log("released over nothing");
        }

        Inventory.DraggedItem = null;
    }

    private void OnSlotPointerEnter(int slotIndex)
    {
        CurrentHoveredInventorySlot = inventorySlots[slotIndex];
    }

    private void OnSlotPointerExit(int slotIndex)
    {
        CurrentHoveredInventorySlot = null;
    }

    private void SortInventorySlotsAccordingToIndex()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            (inventorySlots[i].transform as RectTransform).SetSiblingIndex(i);
        }
    }

    private void OnDisable()
    {
        Unbind();
        CleanupSlots();
    }
}
