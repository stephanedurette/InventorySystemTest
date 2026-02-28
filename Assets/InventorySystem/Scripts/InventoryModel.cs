using UnityEngine;

[CreateAssetMenu(fileName = "InventoryModel", menuName = "Scriptable Objects/InventoryModel")]
public class InventoryModel : ScriptableObject
{
    public int Size;
    public int ColumnCount;
    public int MaxStackCount;
    public int MaxItemCount;
}
