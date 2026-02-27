using UnityEngine;

[CreateAssetMenu(fileName = "ItemModel", menuName = "Scriptable Objects/ItemModel")]
public class ItemModel : ScriptableObject
{
    public Texture2D Icon;
    public string Description;
}
