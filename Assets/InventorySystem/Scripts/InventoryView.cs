using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Text inventoryNameText;
    [SerializeField] private Text inventoryCountLabel;

    public Inventory BoundInventory { get; private set; }

    private UIElementFactory uiElementFactory;

    private InventorySlot[] inventorySlots;

    internal static InventorySlot CurrentHoveredInventorySlot { get; private set; }

    private Vector2 pointerOffset;

    public string InventoryName
    {
        get { return inventoryNameText.text; }
        set { inventoryNameText.text = value; }
    }

    [Inject]
    public void Construct(UIElementFactory uIElementFactory)
    {
        this.uiElementFactory = uIElementFactory;
    }

    private void UpdateInventoryCountLabel()
    {
        inventoryCountLabel.text = $"{BoundInventory.Items.Where(x => x != null).Count()} / {BoundInventory.Model.Size}";
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
        UpdateInventoryCountLabel();
    }

    private void OnItemRemoved(int index)
    {
        inventorySlots[index].Unbind();
        UpdateInventoryCountLabel();
    }

    private void CleanupSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false);
            slot.OnMouseDown -= OnSlotMouseDown;
            slot.OnMouseUpOverSlot -= OnSlotMouseUp;
            slot.OnMouseEnter -= OnSlotPointerEnter;
            slot.OnMouseExit -= OnSlotPointerExit;
        }
    }

    private void InitializeWindow()
    {
        SetGridCellSize();
        CreateInventorySlots();
        AssignItemsToSlots();
        UpdateInventoryCountLabel();

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
            var slot = uiElementFactory.CreateInventorySlot(gridLayoutGroup.transform as RectTransform, BoundInventory);

            slot.OnMouseDown += OnSlotMouseDown;
            slot.OnMouseUpOverSlot += OnSlotMouseUp;
            slot.OnMouseEnter += OnSlotPointerEnter;
            slot.OnMouseExit += OnSlotPointerExit;

            inventorySlots[i] = slot;
        }

        SortInventorySlotsAccordingToIndex();
    }

    private void OnSlotMouseDown(int slotIndex)
    {
        if (inventorySlots[slotIndex].BoundItemView == null) return;

        Inventory.DraggedItem = inventorySlots[slotIndex].BoundItemView.BoundItem;
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        pointerOffset = (eventData as PointerEventData).position - (Vector2)transform.position;
    }
    public void OnPointerUp(BaseEventData eventData)
    {
        
    }

    public void OnDrag(BaseEventData eventData)
    {
        transform.position = (eventData as PointerEventData).position - pointerOffset;
    }

    private void OnSlotMouseUp(InventorySlot previousSlot, InventorySlot hoveredSlot)
    {
        if(Inventory.DraggedItem == null) return;

        if (previousSlot == hoveredSlot)
        {
            Inventory.DraggedItem = null;
            return;
        }

        if (previousSlot.Owner == hoveredSlot.Owner && previousSlot.Index == hoveredSlot.Index) { 
            Inventory.DraggedItem = null;
            return;
        }

        if (previousSlot.BoundItemView != null && hoveredSlot.BoundItemView != null && previousSlot.BoundItemView.BoundItem.Model != hoveredSlot.BoundItemView.BoundItem.Model)
        {
            Inventory.Swap(previousSlot.Owner, hoveredSlot.Owner, previousSlot.Index, hoveredSlot.Index);
            Inventory.DraggedItem = null;
            return;
        }

        int addedItems = hoveredSlot.Owner.TryAddItem(Inventory.DraggedItem.Model, Inventory.DraggedItem.Count, hoveredSlot.Index);
        Inventory.DraggedItem.Owner.RemoveItem(Inventory.DraggedItem.Model, addedItems, previousSlot.Index);

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
