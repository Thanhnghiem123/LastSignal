using UnityEngine;

public static class SaveSystem
{
    private const string PlayerPrefsKeyName = "GameData";

    public static void SaveGame(GameData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            Debug.Log("JSON saved: " + json); // Log JSON trước khi lưu
            PlayerPrefs.SetString(PlayerPrefsKeyName, json);
            PlayerPrefs.Save();
            Debug.Log("Game đã được lưu bằng PlayerPrefs với key: " + PlayerPrefsKeyName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi lưu game: " + e.Message);
        }
    }

    public static GameData LoadGame()
    {
        try
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeyName))
            {
                string json = PlayerPrefs.GetString(PlayerPrefsKeyName);
                Debug.Log("JSON loaded: " + json); // Log JSON sau khi tải
                GameData data = JsonUtility.FromJson<GameData>(json);
                if (data == null)
                {
                    Debug.LogWarning("Dữ liệu JSON không hợp lệ, trả về dữ liệu mặc định.");
                    return null;
                }
                return data;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy dữ liệu lưu trữ, trả về dữ liệu mặc định.");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi tải game: " + e.Message);
            return null;
        }
    }

    public static void DeleteSave()
    {
        try
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeyName))
            {
                PlayerPrefs.DeleteKey(PlayerPrefsKeyName);
                PlayerPrefs.Save();
                Debug.Log("Dữ liệu lưu trữ đã được xóa.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi xóa dữ liệu: " + e.Message);
        }
    }
}