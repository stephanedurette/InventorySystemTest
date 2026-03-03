using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private RectTransform itemParentTransform;

    private ItemView boundItemView;

    public void Bind(ItemView boundItemView)
    {
        this.boundItemView = boundItemView;
        (boundItemView.transform as RectTransform).SetParent(itemParentTransform, false);   
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
