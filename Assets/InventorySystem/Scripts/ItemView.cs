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

    }

    public void Unbind()
    {

    }
}
