using UnityEngine;

public class HelicopterInteract : MonoBehaviour
{
    public static HelicopterInteract Instance;

    public GameObject playerCameraObject; // GameObject chứa camera người chơi
    public GameObject helicopterCameraObject; // GameObject chứa camera máy bay
    private bool hasSwitched = false;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void SwitchToHelicopterCamera()
    {
        BlockUI();
        if (hasSwitched || playerCameraObject == null || helicopterCameraObject == null)
        {
            Debug.LogWarning("Không thể chuyển camera: Đã chuyển hoặc một GameObject camera bị null.");
            return;
        }

        Debug.Log("Đang chuyển sang camera máy bay...");
        playerCameraObject.SetActive(false);
        helicopterCameraObject.SetActive(true);
        hasSwitched = true;

        Debug.Log("Chuyển camera hoàn tất. PlayerCameraObject Active: " + playerCameraObject.activeSelf +
                  ", HelicopterCameraObject Active: " + helicopterCameraObject.activeSelf);
    }

    void BlockUI()
    {
        GameObject playerUI = GameObject.Find("PlayerUI");
        if (playerUI != null)
        {
            // Khóa PlayerUI
            playerUI.SetActive(false);
            Debug.Log("PlayerUI đã bị khóa.");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy PlayerUI!");
        }
    }

}
