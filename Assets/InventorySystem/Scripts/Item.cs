using System;

public class Item
{
    public Action<int, int> OnCountChanged;

    public ItemModel Model { get; private set; }

    private int count;

    public int Count
    {
        get {  return count; }
        set { if (count == value) return; int previousCount = count; count = value; OnCountChanged?.Invoke(previousCount, count); }
    }

    public Item(ItemModel model)
    {
        Model = model;
    }
}
