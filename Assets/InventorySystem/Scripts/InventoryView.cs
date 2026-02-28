using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Scrollbar scrollbar;

    public Inventory BoundInventory { get; private set; }

    public void Bind(Inventory inventory)
    {
        BoundInventory = inventory;
        inventory.OnItemAdded += OnItemAdded;
        inventory.OnItemRemoved += OnItemRemoved;
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

    }

    private void OnItemRemoved(int index)
    {

    }

    private void OnEnable()
    {
        InitializeWindow();
    }

    private void InitializeWindow()
    {
        //spawn slots and initialize them
        StartCoroutine(SetScrollbarCoroutine());
        //populate slots with inventory
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
