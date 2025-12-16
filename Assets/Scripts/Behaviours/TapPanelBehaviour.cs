using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapPanelBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static TapPanelBehaviour Instance { get; set; }

    private bool isClicked = false;
    public bool IsClicked
    {
        get
        {
            if (Instance == null)
                return false;
            return isClicked;
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Instance != null)
            Instance.isClicked = false;
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (Instance != null)
            Instance.isClicked = true;
    }
}
