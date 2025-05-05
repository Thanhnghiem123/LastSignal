using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // Prefab của item cần spawn
    [SerializeField] private Transform itemManager; // Tham chiếu đến ItemManager

    void Start()
    {
        if (itemPrefab == null)
        {
            Debug.LogError("Item prefab is not assigned!");
            return;
        }
        if (itemManager == null)
        {
            Debug.LogError("ItemManager is not assigned!");
            return;
        }

        SpawnItems();
    }

    private void SpawnItems()
    {
        int childCount = itemManager.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("No spawn positions found under ItemManager!");
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform spawnPoint = itemManager.GetChild(i);
            if (spawnPoint != null)
            {
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
                spawnedItem.name = $"SpawnedItem_{i + 1}";
            }
        }
    }
}