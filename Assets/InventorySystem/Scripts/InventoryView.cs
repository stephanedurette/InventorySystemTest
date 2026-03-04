using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Scrollbar scrollbar;

    public Inventory BoundInventory { get; private set; }

    private UIElementFactory uiElementFactory;

    private InventorySlot[] inventorySlots;

    internal static InventorySlot CurrentHoveredInventorySlot { get; private set; }

    private Vector2 pointerOffset;

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
        Debug.Log("Pointer Up");
    }

    public void OnDrag(BaseEventData eventData)
    {
        Debug.Log((eventData as PointerEventData).position);
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

        if (hoveredSlot.Owner.TryAddItem(Inventory.DraggedItem.Model, Inventory.DraggedItem.Count, hoveredSlot.Index))
        {
            Inventory.DraggedItem.Owner.RemoveItem(Inventory.DraggedItem.Model, Inventory.DraggedItem.Count, previousSlot.Index);
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
