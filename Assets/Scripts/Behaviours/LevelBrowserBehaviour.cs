using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBrowserBehaviour : MonoBehaviour
{
    public GameObject btnPrefab;
    public GameObject[] levelButtons;
    public const int LEVEL_COUNT = 2;

    private void OnEnable()
    {
        levelButtons = new GameObject[LEVEL_COUNT];
        for (int i = 0; i < LEVEL_COUNT; i++)
        {
            levelButtons[i] = Instantiate(btnPrefab, transform);

            Button btn = levelButtons[i].GetComponent<Button>();

            // Clear pre-set listeners
            btn.onClick.RemoveAllListeners();

            // Фіксуємо поточне значення індексу в локальній константній змінній циклу
            int currentLevel = i + 1; // ВАЖЛИВО, бо буде лише останнє значення

            // Передаємо значення через лямбду, яка тепер прив'язана виключно до локальної змінної ітерації
            btn.onClick.AddListener(() => {
                GoToLevel(currentLevel);
            });
        }
    }

    void GoToLevel(int levelID)
    {
        SceneManager.LoadScene($"Level{levelID}Scene");
    }
}
