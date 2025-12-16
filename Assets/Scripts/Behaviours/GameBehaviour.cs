using Assets.Scripts;
using TMPro; // Необхідно для роботи з текстом
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehaviour : MonoBehaviour
{
    const string ENDGAME_MENU_NAME = "EndgameMenu";
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
    GameObject pnlEndgame;

    public static GameBehaviour Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance.isGameOn = false;
        Instance.beatFlagCtrl = new BeatFlagController();

        Instance.gameSessionTime = 0;
        Instance.gameLevelTime = 0;

        SetUIText(MIDTEXT_LABEL_NAME, "Tap to start");

        Instance.pnlEndgame = GameObject.Find(ENDGAME_MENU_NAME);
        if (Instance.pnlEndgame != null)
            if (Instance.pnlEndgame.gameObject.activeSelf)
                Instance.pnlEndgame.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // RELENTLESS ACTIONS
        Instance.gameSessionTime += Time.deltaTime;

        // CONDITIONAL ACTIONS
        switch (Instance.isGameOn)
        {
            case true:
                GameObject rocket = GameObject.Find(ROCKET_OBJ_NAME);

                if (rocket != null)
                {
                    Instance.gameLevelTime += Time.deltaTime;

                    Vector3 v = rocket.transform.position;
                    v.y = v.y + FLIGHT_SPEED;
                    rocket.transform.position = v;

                    if (Instance.beatFlagCtrl.IsUntakenFlagExists(Instance.gameLevelTime))
                    {
                        Debug.Log($"SPAN: {Instance.gameLevelTime}");
                        SetUIText(MIDTEXT_LABEL_NAME, "TAP!");
                        SetUIText(GLOBALTIME_LABEL_NAME, Instance.beatFlagCtrl.TimeLeftThisSpan(Instance.gameLevelTime).ToString("F2"));

                        if (TapPanelBehaviour.Instance.IsClicked)
                        {
                            if (Instance.beatFlagCtrl.IsUntakenFlagExists(Instance.gameLevelTime))
                            {
                                Instance.beatFlagCtrl.SetFlagTaken(Instance.gameLevelTime);
                                Debug.Log($"FLAG: {Instance.gameLevelTime}");
                            }
                        }
                    }
                    else
                    {
                        SetUIText(MIDTEXT_LABEL_NAME, string.Empty);
                        SetUIText(GLOBALTIME_LABEL_NAME, string.Empty);
                    }


                    if (Instance.beatFlagCtrl.IsWinAchieved())
                        SetUIText(MIDTEXT_LABEL_NAME, "WIN");
                    else if (Instance.beatFlagCtrl.IsLoseAchieved(Instance.gameLevelTime))
                        SetUIText(MIDTEXT_LABEL_NAME, "LOSE");

                    if (Instance.beatFlagCtrl.IsGameEnded(Instance.gameLevelTime))
                    {
                        SetUIText(GLOBALTIME_LABEL_NAME, string.Empty); // Simplication can lead to delayed timer vanish
                        GameObject.Find(MAINCAMERA_NAME).gameObject.transform.parent = GameObject.Find(MAINCONTAINER_NAME).gameObject.transform;

                        if (Instance.pnlEndgame != null)
                            if (!Instance.pnlEndgame.gameObject.activeSelf)
                                Instance.pnlEndgame.gameObject.SetActive(true);
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
                    StartGame();
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

    #region GET/SET
    public void StartGame()
    {
        Instance.isGameOn = true;
        PlayMusicLevel1();
    }
    #endregion
}