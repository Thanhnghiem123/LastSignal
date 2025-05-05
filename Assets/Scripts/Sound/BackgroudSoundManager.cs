using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudSoundManager : MonoBehaviour
{
    public List<AudioSource> childAudioSources; // Danh sách các AudioSource trong các child objects

    void Start()
    {
        // Tìm tất cả các AudioSource trong các child objects
        childAudioSources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());

        // Kiểm tra nếu không có AudioSource nào
        if (childAudioSources.Count == 0)
        {
            Debug.LogWarning("Không tìm thấy AudioSource nào trong các child objects!");
        }
    }

    public void PlayAllSounds()
    {
        // Phát tất cả các âm thanh trong các child objects
        foreach (AudioSource audioSource in childAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    public void StopAllSounds()
    {
        // Dừng tất cả các âm thanh trong các child objects
        foreach (AudioSource audioSource in childAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void PlaySoundByIndex(int index)
    {
        Debug.Log($"PlaySoundByIndex được gọi với index: {index}");

        // Phát âm thanh của một child object cụ thể theo index
        if (index >= 0 && index < childAudioSources.Count)
        {
            Debug.Log($"Index hợp lệ. Số lượng AudioSource: {childAudioSources.Count}");

            if (!childAudioSources[index].isPlaying)
            {
                Debug.Log($"Phát âm thanh cho AudioSource tại index: {index}");
                childAudioSources[index].Play();
            }
            else
            {
                Debug.Log($"AudioSource tại index {index} đang phát.");
            }
        }
        else
        {
            Debug.LogWarning("Index không hợp lệ!");
        }
    }

    public void StopSoundByIndex(int index)
    {
        // Dừng âm thanh của một child object cụ thể theo index
        if (index >= 0 && index < childAudioSources.Count)
        {
            if (childAudioSources[index].isPlaying)
            {
                childAudioSources[index].Stop();
            }
        }
        else
        {
            Debug.LogWarning("Index không hợp lệ!");
        }
    }
}
