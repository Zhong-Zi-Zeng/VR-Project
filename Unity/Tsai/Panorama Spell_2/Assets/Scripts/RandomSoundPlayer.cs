using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // 指定音頻源
    public AudioClip[] audioClips;  // 用於存儲所有音頻剪輯的數組

    void Start()
    {
        // 加載所有音頻剪輯
        audioClips = Resources.LoadAll<AudioClip>("sounds");
        PlayRandomSound();
    }

    void PlayRandomSound()
    {
        // 從音頻剪輯數組中隨機選擇一個
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
}
