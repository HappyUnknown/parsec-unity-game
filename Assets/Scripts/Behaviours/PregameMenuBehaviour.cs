using UnityEngine;
using UnityEngine.EventSystems;

public class PregameMenuBehaviour : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Tap detected! Starting the game...");

        if (GameBehaviour.Instance != null)
            GameBehaviour.Instance.StartGame();
        else
            Debug.LogError("GameBehaviour Singleton not found!");

        gameObject.SetActive(false);
    }
}