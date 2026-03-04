using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private RectTransform itemParentTransform;

    private ItemView boundItemView;

    public ItemView BoundItemView => boundItemView;

    public Action<int> OnMouseDown;
    public Action<int ,int> OnMouseUp;
    public Action<int> OnMouseEnter;
    public Action<int> OnMouseExit;

    public int Index => transform.GetSiblingIndex();

    public void Bind(ItemView boundItemView)
    {
        this.boundItemView = boundItemView;
        (boundItemView.transform as RectTransform).SetParent(itemParentTransform, false);   
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        OnMouseDown?.Invoke(Index);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (ListContainsComponent((eventData as PointerEventData).hovered, out InventorySlot inventorySlot))
        {
            OnMouseUp?.Invoke(Index, inventorySlot.Index);
        }
    }

    private bool ListContainsComponent<T>(List<GameObject> list, out T component) where T : Component
    {
        foreach(var item in list)
        {
            if (item.TryGetComponent(out T t))
            {
                component = t;
                return true;
            }
        }
        component = null;
        return false;
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        OnMouseEnter?.Invoke(Index);
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        OnMouseExit?.Invoke(Index);
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
