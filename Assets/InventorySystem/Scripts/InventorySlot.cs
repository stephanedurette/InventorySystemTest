using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform itemParentTransform;

    private ItemView boundItemView;

    public void Bind(ItemView boundItemView)
    {
        this.boundItemView = boundItemView;
        (boundItemView.transform as RectTransform).SetParent(itemParentTransform, false);   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("f");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("f");
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
