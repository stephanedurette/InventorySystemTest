using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform itemParentTransform;

    private ItemView boundItemView;

    public int Index => transform.GetSiblingIndex();

    public void Bind(ItemView boundItemView)
    {
        this.boundItemView = boundItemView;
        (boundItemView.transform as RectTransform).SetParent(itemParentTransform, false);   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("f");
        //set dragged inventory item to bound item
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("f");
        //if dragged item and if this inventory is different than the dragged item's inventory
            //remove the dragged item from that item's inventory
            //add it to this slot's inventory at this slot's index
    }

    public void Unbind()
    {
        if (boundItemView != null) {
            boundItemView.gameObject.SetActive(false);
            boundItemView = null;
        }
    }

    private void OnDisable()
    {
        Unbind();
    }
}
