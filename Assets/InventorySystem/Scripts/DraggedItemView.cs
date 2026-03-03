using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggedItemView : MonoBehaviour
{
    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;

    private void OnEnable()
    {
        Inventory.OnDraggedItemChanged += OnDraggedItemChanged;
        Inventory.OnDraggedItemCleared += OnDraggedItemCleared;
    }

    private void OnDisable()
    {
        Inventory.OnDraggedItemChanged -= OnDraggedItemChanged;
        Inventory.OnDraggedItemCleared -= OnDraggedItemCleared;
    }

    private void Awake()
    {
        contentTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void OnDraggedItemChanged(Item item)
    {
        image.sprite = item.Model.Icon;
        amountText.text = item.Count.ToString();

        contentTransform.gameObject.SetActive(true);
    }

    private void OnDraggedItemCleared()
    {
        contentTransform.gameObject.SetActive(false);
    }
}
