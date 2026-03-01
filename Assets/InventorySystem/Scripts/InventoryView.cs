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
        inventorySlots = new InventorySlot[BoundInventory.Model.Size];

        gridLayoutGroup.constraintCount = BoundInventory.Model.ColumnCount;

        //set slot size

        for (int i = 0; i < BoundInventory.Model.Size; i++) {
            var slot = uiElementFactory.CreateInventorySlot(gridLayoutGroup.GetComponent<RectTransform>());
            inventorySlots[i] = slot;
        }

        StartCoroutine(SetScrollbarCoroutine());
    }

    private IEnumerator SetScrollbarCoroutine()
    {
        //need to do this after UI calculations
        yield return new WaitForEndOfFrame();
        scrollbar.value = 1;
    }

    private void OnDisable()
    {
        Unbind();
    }
}
