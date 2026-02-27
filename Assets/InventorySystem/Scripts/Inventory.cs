using System;

public class Inventory
{
    public InventoryModel Model { get; private set; }

    public Action<int, Item> OnItemRemoved;

    public Action<int, Item> OnItemAdded;

    private Item[] items;

    public Inventory(InventoryModel model) {
        this.Model = model;
        items = new Item[Model.Size];
    }

    public void AddItem(Item item, int amount)
    {

    }

    public void RemoveItem(Item item, int amount)
    {

    }

    public void AddItem(Item item, int amount, int index) { 
        //if new item bind the change function
    }

    public void RemoveItem(Item item, int amount, int index) { 
        //unbind the item change function if amount < 0
    }
}
