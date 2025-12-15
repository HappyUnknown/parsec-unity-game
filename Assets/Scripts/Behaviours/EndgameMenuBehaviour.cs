using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameMenuBehaviour : Button
{
    public async void EndgameGoMainMenu()
    {
        await SceneManager.LoadSceneAsync("MenuScene");
    }
}
