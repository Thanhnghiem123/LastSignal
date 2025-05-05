using UnityEngine;
using System.Collections.Generic;

public class ZombiePool : MonoBehaviour
{
    [System.Serializable]
    public class ZombieType
    {
        public string typeName; // Tên loại zombie (để dễ debug)
        public GameObject prefab; // Prefab của loại zombie
        public int poolSize; // Số lượng trong pool
        [HideInInspector] public List<GameObject> pool; // Pool cho loại này
    }

    public ZombieType[] zombieTypes; // Mảng chứa 3 loại zombie

    void Start()
    {
        // Khởi tạo pool cho từng loại zombie
        foreach (ZombieType type in zombieTypes)
        {
            type.pool = new List<GameObject>();
            for (int i = 0; i < type.poolSize; i++)
            {
                GameObject zombie = Instantiate(type.prefab, Vector3.zero, Quaternion.identity);
                zombie.SetActive(false);
                type.pool.Add(zombie);
                //Debug.Log("Khoi tao zombie: " + i + "  |   " + zombie);
            }
        }
    }

    // Lấy zombie từ pool theo loại
    public GameObject GetZombie(int typeIndex)
    {
        ZombieType type = zombieTypes[typeIndex];
        foreach (GameObject zombie in type.pool)
        {
            if (!zombie.activeInHierarchy)
            {
                return zombie;
            }
        }
        // Nếu pool hết, tạo mới
        GameObject newZombie = Instantiate(type.prefab, Vector3.zero, Quaternion.identity);
        newZombie.SetActive(false);
        type.pool.Add(newZombie);
        return newZombie;
    }

    // Tắt tất cả zombie
    public void DeactivateAllZombies()
    {
        foreach (ZombieType type in zombieTypes)
        {
            foreach (GameObject zombie in type.pool)
            {
                zombie.SetActive(false);
            }
        }
    }
}