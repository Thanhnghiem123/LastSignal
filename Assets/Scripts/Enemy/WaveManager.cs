using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    public ZombiePool zombiePool;
    public Transform[] spawnPoints;
    public Transform player;

    [System.Serializable]
    public class Wave
    {
        public int[] zombiesToSpawn; // Số lượng mỗi loại zombie (0: Nhanh, 1: Thường, 2: Mạnh)
    }

    public Wave[] waves;
    public float spawnDelay; // Delay giữa mỗi zombie trong 1 đợt
    public int minZombiesToSpawnNextWave; // Ngưỡng để spawn đợt mới (0: khi hết zombie)
    private int currentWave = 0;
    private int activeZombies = 0; // Đếm số zombie đang sống

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Length)
        {
            // Đợi đến khi số zombie còn lại nhỏ hơn ngưỡng
            while (activeZombies > minZombiesToSpawnNextWave)
            {
                yield return new WaitForSeconds(1f); // Kiểm tra mỗi giây
            }

            Debug.Log("Wave " + (currentWave + 1) + " bắt đầu!");

            Wave wave = waves[currentWave];
            // Spawn từng loại zombie                     5
            for (int typeIndex = 0; typeIndex < wave.zombiesToSpawn.Length; typeIndex++)
            {
                for (int i = 0; i < wave.zombiesToSpawn[typeIndex]; i++)
                {
                    SpawnZombie(typeIndex);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }

            currentWave++;
        }
    }

    void SpawnZombie(int typeIndex)
    {
        GameObject zombie = zombiePool.GetZombie(typeIndex);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        zombie.transform.position = spawnPoint.position;
        zombie.transform.rotation = spawnPoint.rotation;
        zombie.SetActive(true);

        // Tăng số zombie active
        activeZombies++;

        // Gán NavMeshAgent
        NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.SetDestination(player.position);
        }

        // Gán script để theo dõi trạng thái zombie
        EnemyHealth zombieHealth = zombie.GetComponent<EnemyHealth>();
        if (zombieHealth == null)
        {
            zombieHealth = zombie.AddComponent<EnemyHealth>();
        }
        zombieHealth.waveManager = this; // Gán để báo khi zombie chết
    }

    // Gọi khi zombie "chết" (tắt)
    public void OnZombieDeactivated()
    {
        activeZombies--;
    }
}