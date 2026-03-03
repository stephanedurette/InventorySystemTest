using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown inventorySelectDropdown;
    [SerializeField] private TMP_Dropdown itemSelectDropdown;
    [SerializeField] private TMP_Dropdown indexSelectDropdown;
    [SerializeField] private Slider itemAmountSlider;

    [Header("Settings")]
    [SerializeField] private List<ItemModel> itemSelectContent;
    [SerializeField] private List<InventoryHolder> inventorySelectContent;

    public void OnOpenInventoryClicked()
    {

    }

    public void OnAddItemClicked()
    {

    }

    public void OnRemoveItemClicked()
    {

    }
}
