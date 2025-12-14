using Assets.Scripts;
using TMPro; // Необхідно для роботи з текстом
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehaviour : MonoBehaviour
{
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

        SetUIText("MidText", "Tap to start");

        pnlAfterGame = GameObject.Find("PanelAfterGame");
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
                GameObject rocket = GameObject.Find("Rocket");

                if (rocket != null)
                {
                    gameLevelTime += Time.deltaTime;

                    Vector3 v = rocket.transform.position;
                    v.y = v.y + FLIGHT_SPEED;
                    rocket.transform.position = v;
                }
                else
                {
                    Debug.Log("ROCKET does not exist");
                    return;
                }

                if (beatFlagCtrl.IsUntakenFlagExists(gameLevelTime))
                {
                    Debug.Log($"SPAN: {gameLevelTime}");
                    SetUIText("MidText", "TAP!");
                    SetUIText("GlobalTime", beatFlagCtrl.TimeLeftThisSpan(gameLevelTime).ToString("F2"));
                }
                else
                {
                    SetUIText("MidText", string.Empty);
                    SetUIText("GlobalTime", string.Empty);
                }

                if (Keyboard.current.ctrlKey.isPressed)
                {
                    if (beatFlagCtrl.IsUntakenFlagExists(gameLevelTime))
                    {
                        beatFlagCtrl.SetFlagTaken(gameLevelTime);
                        Debug.Log($"FLAG: {gameLevelTime}");
                    }
                }

                if (beatFlagCtrl.IsWinAchieved())
                    SetUIText("MidText", "WIN");
                else if (beatFlagCtrl.IsLoseAchieved(gameLevelTime))
                    SetUIText("MidText", "LOSE");

                if (beatFlagCtrl.IsGameEnded(gameLevelTime))
                {
                    SetUIText("GlobalTime", string.Empty); // Simplication can lead to delayed timer vanish
                    GameObject.Find("MainCamera").gameObject.transform.parent = GameObject.Find("Main").gameObject.transform;

                    if (pnlAfterGame != null)
                        if (!pnlAfterGame.gameObject.activeSelf)
                            pnlAfterGame.gameObject.SetActive(true);
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
        string playerName = "SoundPlayer";
        GameObject player = GameObject.Find(playerName);
        if (player != null)
        {
            AudioSource src = player.GetComponent<AudioSource>();
            if (src != null)
                src.Play();
            else
                Debug.Log($"Could not get source of \"{playerName}\"");
        }
        else
            Debug.Log($"Could not get object \"{playerName}\"");
    }
    #endregion
}