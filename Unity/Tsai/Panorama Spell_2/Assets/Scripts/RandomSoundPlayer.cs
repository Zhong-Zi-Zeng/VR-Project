using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // ���w���W��
    public AudioClip[] audioClips;  // �Ω�s�x�Ҧ����W�ſ誺�Ʋ�

    void Start()
    {
        // �[���Ҧ����W�ſ�
        audioClips = Resources.LoadAll<AudioClip>("sounds");
        PlayRandomSound();
    }

    void PlayRandomSound()
    {
        // �q���W�ſ�Ʋդ��H����ܤ@��
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
}
