using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : Button
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public async void MenuPlayGameTemp() 
    {
        await SceneManager.LoadSceneAsync("Level1Scene");
    }
}
