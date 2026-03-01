using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private RectTransform itemParentTransform;

    public RectTransform ItemParentTransform => itemParentTransform;
}
