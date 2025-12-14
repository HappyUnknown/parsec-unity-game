using Assets.Scripts;
using TMPro; // Необхідно для роботи з текстом
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehaviour : MonoBehaviour
{
    const string AFTERGAME_MENU_NAME = "AfterGameMenu";
    const string MIDTEXT_LABEL_NAME = "MidText";
    const string GLOBALTIME_LABEL_NAME = "GlobalTime";
    const string ROCKET_OBJ_NAME = "Rocket";
    const string MAINCAMERA_NAME = "MainCamera";
    const string MAINCONTAINER_NAME = "Main";
    const string SOUNDPLAYER_NAME = "SoundPlayer";
    const float FLIGHT_SPEED = .1f;

    bool isGameOn;
    float gameSessionTime;
    float gameLevelTime;

    BeatFlagController beatFlagCtrl;
    GameObject pnlAfterGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameOn = false;
        beatFlagCtrl = new BeatFlagController();

        gameSessionTime = 0;
        gameLevelTime = 0;

        SetUIText(MIDTEXT_LABEL_NAME, "Tap to start");

        pnlAfterGame = GameObject.Find(AFTERGAME_MENU_NAME);
        if (pnlAfterGame != null)
            if (pnlAfterGame.gameObject.activeSelf)
                pnlAfterGame.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // RELENTLESS ACTIONS
        gameSessionTime += Time.deltaTime;

        // CONDITIONAL ACTIONS
        switch (isGameOn)
        {
            case true:
                GameObject rocket = GameObject.Find(ROCKET_OBJ_NAME);

                if (rocket != null)
                {
                    gameLevelTime += Time.deltaTime;

                    Vector3 v = rocket.transform.position;
                    v.y = v.y + FLIGHT_SPEED;
                    rocket.transform.position = v;

                    if (beatFlagCtrl.IsUntakenFlagExists(gameLevelTime))
                    {
                        Debug.Log($"SPAN: {gameLevelTime}");
                        SetUIText(MIDTEXT_LABEL_NAME, "TAP!");
                        SetUIText(GLOBALTIME_LABEL_NAME, beatFlagCtrl.TimeLeftThisSpan(gameLevelTime).ToString("F2"));

                        if (Keyboard.current.ctrlKey.isPressed)
                        {
                            if (beatFlagCtrl.IsUntakenFlagExists(gameLevelTime))
                            {
                                beatFlagCtrl.SetFlagTaken(gameLevelTime);
                                Debug.Log($"FLAG: {gameLevelTime}");
                            }
                        }
                    }
                    else
                    {
                        SetUIText(MIDTEXT_LABEL_NAME, string.Empty);
                        SetUIText(GLOBALTIME_LABEL_NAME, string.Empty);
                    }


                    if (beatFlagCtrl.IsWinAchieved())
                        SetUIText(MIDTEXT_LABEL_NAME, "WIN");
                    else if (beatFlagCtrl.IsLoseAchieved(gameLevelTime))
                        SetUIText(MIDTEXT_LABEL_NAME, "LOSE");

                    if (beatFlagCtrl.IsGameEnded(gameLevelTime))
                    {
                        SetUIText(GLOBALTIME_LABEL_NAME, string.Empty); // Simplication can lead to delayed timer vanish
                        GameObject.Find(MAINCAMERA_NAME).gameObject.transform.parent = GameObject.Find(MAINCONTAINER_NAME).gameObject.transform;

                        if (pnlAfterGame != null)
                            if (!pnlAfterGame.gameObject.activeSelf)
                                pnlAfterGame.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("ROCKET does not exist");
                    return;
                }


                break;

            case false:
            default:
                if (Keyboard.current.enterKey.isPressed)
                {
                    isGameOn = true;
                    Debug.Log("STARTED");
                    PlayMusicLevel1();
                }
                break;
        }

    }
    bool SetUIText(string textCntnName, string val)
    {
        // --- ВИПРАВЛЕННЯ ТЕКСТУ (GlobalTime) ---
        GameObject globalTimeObj = GameObject.Find(textCntnName);
        if (globalTimeObj != null)
        {
            // ВАЖЛИВО: Використовуємо TextMeshProUGUI, тому що об'єкт знаходиться в Canvas (UI)
            TextMeshProUGUI tmp = globalTimeObj.GetComponent<TextMeshProUGUI>();

            if (tmp != null)
            {
                // "F2" покаже 2 знаки після коми (наприклад: 10.54)
                tmp.text = val;
                return true;
            }
            else
                Debug.Log("THERE IS NO TEXT MESH");
        }
        else
            Debug.Log("THERE IS NO GLOBAL TIME OBJ");


        return false;
    }

    #region TO SEPARATE CLASS
    void PlayMusicLevel1()
    {
        GameObject player = GameObject.Find(SOUNDPLAYER_NAME);
        if (player != null)
        {
            AudioSource src = player.GetComponent<AudioSource>();
            if (src != null)
                src.Play();
            else
                Debug.Log($"Could not get source of \"{SOUNDPLAYER_NAME}\"");
        }
        else
            Debug.Log($"Could not get object \"{SOUNDPLAYER_NAME}\"");
    }
    #endregion
}