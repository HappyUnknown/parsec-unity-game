using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerBehaviour : MonoBehaviour
{
    public int levelID;
    public GameObject homeMenu;
    public GameObject levelMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToLevelSelector()
    {
        levelMenu?.SetActive(true);
        homeMenu?.SetActive(false);
    }

    public void GoToLevelByID(int lvlID)
    {
        SceneManager.LoadSceneAsync($"Level{lvlID}Scene");
    }

    public void GoToSelectedLevel()
    {
        GoToLevelByID(levelID);
    }
}
