using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIElementFactory uiFactory;

    [SerializeField] private Transform playerInventoryPosition;
    [SerializeField] private Transform NpcInventoryPosition;

    public void OpenPlayerInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(playerInventoryPosition.position);
    }

    public void OpenNPCInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(NpcInventoryPosition.position);
    }
}
