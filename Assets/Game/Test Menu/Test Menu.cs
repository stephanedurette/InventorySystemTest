using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Dropdown inventorySelectDropdown;
    [SerializeField] private Dropdown itemSelectDropdown;
    [SerializeField] private Dropdown indexSelectDropdown;
    [SerializeField] private Slider itemAmountSlider;

    [Header("Settings")]
    [SerializeField] private List<ItemModel> itemSelectContent;
    [SerializeField] private List<ItemModel> inventorySelectContent;

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
