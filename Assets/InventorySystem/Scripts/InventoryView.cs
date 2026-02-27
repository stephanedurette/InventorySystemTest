using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    public Inventory BoundInventory { get; private set; }

    public void Bind(Inventory inventory)
    {
        BoundInventory = inventory;
    }

    public void Unbind()
    {
        if (BoundInventory != null) { 
            BoundInventory = null;
        }
    }

    private void OnEnable()
    {
        //spawn slots and initialize them
        //move scrollbar up
        //populate slots with inventory
    }

    private void OnDisable()
    {
        Unbind();
    }
}
