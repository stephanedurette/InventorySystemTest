using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private UIElementFactory uiElementFactory;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Scrollbar scrollbar;

    public Inventory BoundInventory { get; private set; }

    private UIElementFactory uiElementFactory;

    [Inject]
    public void Construct(UIElementFactory uIElementFactory)
    {
        this.uiElementFactory = uIElementFactory;
    }

    public void Bind(Inventory inventory)
    {
        BoundInventory = inventory;
        inventory.OnItemAdded += OnItemAdded;
        inventory.OnItemRemoved += OnItemRemoved;
    }

    public void Unbind()
    {
        if (BoundInventory != null) {
            BoundInventory.OnItemAdded -= OnItemAdded;
            BoundInventory.OnItemRemoved -= OnItemRemoved;
            BoundInventory = null;
        }
    }

    private void OnItemAdded(int index, Item item)
    {

    }

    private void OnItemRemoved(int index)
    {

    }

    private void OnEnable()
    {
        StartCoroutine(InitializeWindowCoroutine());
    }

    private IEnumerator InitializeWindowCoroutine()
    {
        //wait for dependencies to be injected, since construct happens before onenable
        yield return new WaitUntil(() => uiElementFactory != null);
        Debug.Log(uiElementFactory);
        //uiElementFactory.CreateInventorySlot(gridLayoutGroup.GetComponent<RectTransform>());
        StartCoroutine(SetScrollbarCoroutine());
    }

    private IEnumerator SetScrollbarCoroutine()
    {
        //need to do this after UI calculations
        yield return new WaitForEndOfFrame();
        scrollbar.value = 1;
    }

    private void OnDisable()
    {
        Unbind();
    }
}
