using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GunScript gunScript;
    private static GameManager _instance;

    private Vector3 playerPosition;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject manager = new GameObject("GameManager");
                    _instance = manager.AddComponent<GameManager>();
                    DontDestroyOnLoad(manager);
                    Debug.Log("GameManager created dynamically");
                }
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    private GameData gameData;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager đã được khởi tạo: " + gameObject.name);
        }
        else if (_instance != this)
        {
            Debug.Log("Phát hiện GameManager trùng lặp, hủy: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        gameData = new GameData();
        playerPosition = gameData.playerPosition;
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            Debug.Log("GameManager bị hủy, Instance được đặt về null");
        }
    }

    public void OnNewGame()
    {
        //playerPosition = new Vector3(-102.368713f, -24.4370003f, 62.3272438f);
        SaveSystem.DeleteSave();
        gameData = new GameData();

        PlayerPrefs.SetFloat("BulletsIHave_0", 540);
        PlayerPrefs.SetFloat("BulletsInTheGun_0", 40);
        PlayerPrefs.SetFloat("AmountOfBulletsPerLoad_0", 40);

        // Lưu cho nonautomatic (index 1)
        PlayerPrefs.SetFloat("BulletsIHave_1", 60);
        PlayerPrefs.SetFloat("BulletsInTheGun_1",12);
        PlayerPrefs.SetFloat("AmountOfBulletsPerLoad_1", 12);

        Debug.Log("New game started, player position: " + playerPosition);
        //SceneManager.LoadScene(1);
    }

    public void OnContinue()
    {
        //playerPosition = new Vector3(-102.368713f, 100f, 62.3272438f);
        Debug.Log("Continue game, player position: " + playerPosition);
        gameData = SaveSystem.LoadGame();
        if(gameData == null)
        {
            Debug.LogError("Failed to load game data, starting new game.");
            OnNewGame();
            return;
        }
        Debug.Log("Game loaded, player position: " + gameData.playerPosition);
        //SceneManager.LoadScene(1);
    }

    public void SaveGameState(float health, Vector3 position)
    {
        gameData.health = health;
        gameData.playerPosition = position;
        SaveSystem.SaveGame(gameData);
    }

    public void ApplyGameData(GameObject player)
    {
        player.transform.localPosition = gameData.playerPosition;
        Debug.Log("Player position set to: " + gameData.playerPosition);
        PlayerController playerController = player.GetComponent<PlayerController>();
        Debug.Log("PlayerController found: " + (playerController != null));
        if (playerController != null)
        {
            Debug.Log("Co playerrrrrrrrrrrrrrrrrrrrrrrrrrrr");
            playerController.SetHealth(gameData.health);
            //playerController.SetAmmo(gameData.bulletsIHave, gameData.bulletsInTheGun, gameData.amountOfBulletsPerLoad);
        }
    }
}