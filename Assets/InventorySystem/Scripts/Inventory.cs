using System;
using System.Collections.Generic;

public class Inventory
{
    public InventoryModel Model { get; private set; }

    public Action<int, Item> OnItemRemoved;

    public Action<int, Item> OnItemAdded;

    private Item[] items;

    public Inventory(InventoryModel model)
    {
        this.Model = model;
        items = new Item[Model.Size];
    }

    public void AddItem(Item item, int amount)
    {
        int remainingAmountToAdd = Math.Min(amount, Model.MaxItemCount - GetTotalItemCount(item));

        if (GetItemIndices(item, out var indices))
        {
            foreach (var index in indices)
            {
                int amountToAdd = Math.Min(Model.MaxStackCount - items[index].Count, remainingAmountToAdd);
                AddItem(item, amountToAdd, index);
                remainingAmountToAdd -= amountToAdd;

                if (remainingAmountToAdd == 0)
                    return;
            }
        }

        while (remainingAmountToAdd > 0)
        {
            if (GetNextEmptyPosition(out int index))
            {
                int amountToAdd = Math.Min(Model.MaxStackCount, remainingAmountToAdd);
                AddItem(item, amountToAdd, index);
                remainingAmountToAdd -= amountToAdd;
            }
        }
    }

    public void RemoveItem(Item item, int amount)
    {

    }



    public void AddItem(Item item, int amount, int index)
    {
        if (items[index] == null)
        {
            Item newItem = new Item(item.Model);
            newItem.OnCountChanged += OnItemCountChanged;
            newItem.Count = Math.Min(amount, Model.MaxStackCount);
            items[index] = newItem;
            OnItemAdded?.Invoke(index, newItem);
        }
        else
        {
            int amountToAdd = Math.Min(amount, Model.MaxStackCount - items[index].Count);
            items[index].Count += amountToAdd;
        }
    }

    public void RemoveItem(Item item, int amount, int index)
    {
        //unbind the item change function if amount < 0
    }

    private void OnItemCountChanged(int oldValue, int newValue)
    {
        //nothing for now
    }

    private bool GetItemIndices(Item item, out List<int> indices)
    {
        indices = new List<int>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Model == item.Model)
            {
                indices.Add(i);
            }
        }
        return indices.Count > 0;
    }

    private bool GetNextEmptyPosition(out int nextEmptyPosition)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                nextEmptyPosition = i;
                return true;
            }
        }
        nextEmptyPosition = -1;
        return false;
    }

    private int GetTotalItemCount(Item item)
    {
        int count = 0;
        foreach (var i in items)
        {
            if (i.Model == item.Model) count += i.Count;
        }
        return count;
    }
}
