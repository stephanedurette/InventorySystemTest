using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image iconImage;

    public Item BoundItem { get; private set; }

    public void Bind(Item item)
    {
        BoundItem = item;
        iconImage.sprite = BoundItem.Model.Icon;
        BoundItem.OnCountChanged += OnBoundItemAmountChanged;
    }

    private void OnBoundItemAmountChanged(int previousAmount, int newAmount) { 
        countText.text = newAmount.ToString();
    }

    public void Unbind()
    {
        if (BoundItem == null) return;
        BoundItem.OnCountChanged -= OnBoundItemAmountChanged;
    }

    private void OnDisable()
    {
        Unbind();
    }
}
