using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerInventoryPosition;
    [SerializeField] private Transform NpcInventoryPosition;

    private UIElementFactory uiFactory;

    [Inject]
    public void Construct(UIElementFactory uiFactory)
    {
        this.uiFactory = uiFactory;
    }

    public void OpenPlayerInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(playerInventoryPosition.position);
    }

    public void OpenNPCInventoryClicked()
    {
        uiFactory.CreateInventoryWindow(NpcInventoryPosition.position);
    }
}
