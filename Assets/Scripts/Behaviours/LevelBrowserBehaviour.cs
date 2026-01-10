using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBrowserBehaviour : MonoBehaviour
{
    public GameObject btnPrefab;

    private void OnEnable()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject btn = Instantiate(btnPrefab, transform);
            btn.GetComponent<Button>().onClick.AddListener(() => GoToLevel(1));
        }
    }

    void GoToLevel(int levelID)
    {
        SceneManager.LoadScene($"Level{levelID}Scene");
    }
}
