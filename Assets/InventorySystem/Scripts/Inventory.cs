using System;
using System.Collections.Generic;

public class Inventory
{
    public InventoryModel Model { get; private set; }

    public Action<int> OnItemRemoved;

    public Action<int, Item> OnItemAdded;

    private Item[] items;

    public Item[] Items => items;

    public Inventory(InventoryModel model)
    {
        this.Model = model;
        items = new Item[Model.Size];
    }

    public bool TryAddItem(ItemModel itemModel, int amount)
    {
        int remainingAmountToAdd = Math.Min(amount, Model.MaxItemCount - GetTotalItemCount(itemModel));

        if (GetItemIndices(itemModel, out var indices))
        {
            foreach (var index in indices)
            {
                int amountToAdd = Math.Min(Model.MaxStackCount - items[index].Count, remainingAmountToAdd);
                TryAddItem(itemModel, amountToAdd, index);
                remainingAmountToAdd -= amountToAdd;

                if (remainingAmountToAdd == 0)
                    return true;
            }
        }

        while (remainingAmountToAdd > 0)
        {
            if (GetNextEmptyPosition(out int index))
            {
                int amountToAdd = Math.Min(Model.MaxStackCount, remainingAmountToAdd);
                TryAddItem(itemModel, amountToAdd, index);
                remainingAmountToAdd -= amountToAdd;
            }
        }

        return true;
    }

    public void RemoveItem(ItemModel itemModel, int amount)
    {
        int remainingAmountToRemove = amount;

        if (GetItemIndices(itemModel, out var indices))
        {
            foreach (var index in indices)
            {
                int amountToRemove = Math.Min(items[index].Count, remainingAmountToRemove);
                RemoveItem(amountToRemove, index);
                remainingAmountToRemove -= amountToRemove;

                if (remainingAmountToRemove == 0)
                    return;
            }
        }
    }

    public bool TryAddItem(ItemModel itemModel, int amount, int index)
    {
        if (items[index] == null)
        {
            Item newItem = new Item(itemModel);
            newItem.OnCountChanged += OnItemCountChanged;
            newItem.Count = Math.Min(amount, Model.MaxStackCount);
            items[index] = newItem;
            OnItemAdded?.Invoke(index, newItem);
        }
        else
        {
            int amountToAdd = Math.Min(amount, Model.MaxStackCount - items[index].Count);

            if (amountToAdd == 0 || items[index].Model != itemModel) return false;

            items[index].Count += amountToAdd;
        }

        return true;
    }

    public void RemoveItem(int amount, int index)
    {
        items[index].Count -= amount;
        if (items[index].Count <= 0)
        {
            items[index].OnCountChanged -= OnItemCountChanged;
            items[index] = null;
            OnItemRemoved?.Invoke(index);
        }
    }

    private void OnItemCountChanged(int oldValue, int newValue)
    {
        //nothing for now
    }

    private bool GetItemIndices(ItemModel itemModel, out List<int> indices)
    {
        indices = new List<int>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;
            if (items[i].Model == itemModel)
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

    private int GetTotalItemCount(ItemModel itemModel)
    {
        int count = 0;
        foreach (var i in items)
        {
            if (i == null) continue;

            if (i.Model == itemModel) count += i.Count;
        }
        return count;
    }
}
