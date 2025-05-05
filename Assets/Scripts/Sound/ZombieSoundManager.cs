using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource roarSound1;
    [SerializeField] private AudioSource roarSound2;

    private bool hasStarted = false;

    void Start()
    {
        // Bắt đầu sau 30s
        Invoke("StartRoaring", 30f);
    }

    void StartRoaring()
    {
        hasStarted = true;
        // Bắt đầu phát âm thanh đầu tiên
        PlayRandomRoar();
        // Lặp lại mỗi 15s
        InvokeRepeating("PlayRandomRoar", 60f, 60f);
    }

    void PlayRandomRoar()
    {
        if (!hasStarted) return;

        // Chọn ngẫu nhiên giữa 2 âm thanh
        int randomSound = Random.Range(0, 2);

        switch (randomSound)
        {
            case 0:
                if (roarSound1 != null && !roarSound1.isPlaying)
                {
                    roarSound1.Play();
                }
                break;
            case 1:
                if (roarSound2 != null && !roarSound2.isPlaying)
                {
                    roarSound2.Play();
                }
                break;
        }
    }
}