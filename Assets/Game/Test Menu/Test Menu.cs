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
    [SerializeField] private TextMeshProUGUI itemCountLabel;

    [Header("Settings")]
    [SerializeField] private List<ItemModel> itemSelectContent;
    [SerializeField] private List<InventoryHolder> inventorySelectContent;
    [SerializeField] private int maxInventorySelectIndex;

    private void Awake()
    {
        InitializeItemDropdownContent();
        InitializeInventoryDropdownContent();
        InitializeIndexSelectDropdownContent();

        OnItemNumberSliderValueChanged(0);
    }

    private void InitializeItemDropdownContent()
    {
        foreach (var item in itemSelectContent)
        {
            itemSelectDropdown.options.Add(new(item.name));
        }
    }
    private void InitializeInventoryDropdownContent()
    {
        foreach (var item in inventorySelectContent)
        {
            inventorySelectDropdown.options.Add(new(item.name));
        }
    }

    private void InitializeIndexSelectDropdownContent()
    {
        indexSelectDropdown.options.Add(new("First Available"));
        for (int i = 1; i <= maxInventorySelectIndex; i++)
            indexSelectDropdown.options.Add(new(i.ToString()));
    }

    public void OnOpenInventoryClicked()
    {
        inventorySelectContent[inventorySelectDropdown.value].OpenInventory();
    }

    public void OnAddItemClicked()
    {
        if (indexSelectDropdown.value == 0)
        {
            inventorySelectContent[inventorySelectDropdown.value].Inventory.AddItem(itemSelectContent[itemSelectDropdown.value], (int)itemAmountSlider.value);
        }
        else
        {
            inventorySelectContent[inventorySelectDropdown.value].Inventory.AddItem(itemSelectContent[itemSelectDropdown.value], (int)itemAmountSlider.value, indexSelectDropdown.value - 1);
        }
    }

    public void OnRemoveItemClicked()
    {

    }

    public void OnItemNumberSliderValueChanged(float value)
    {
        itemCountLabel.text = value.ToString();
    }
}
