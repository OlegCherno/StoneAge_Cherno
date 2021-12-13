using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundManager")]

public class SoundManager : ScriptableObject
{

    [SerializeField] private AudioClip soundCollect;                             // контеqнер для звука
    [SerializeField] private AudioClip soundLoseLife;
    [SerializeField] private AudioClip soundStone;
    [SerializeField] private AudioClip soundAxeTree;

    private Vector3 cameraPosition;

    private void PlaySound(AudioClip audioClip)
    {
        cameraPosition = Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(audioClip, cameraPosition);
    }

    public void PlayCollect()
    {
        PlaySound(soundCollect);
    }
    public void PlayLoseLife()
    {
        PlaySound(soundLoseLife);
    }
    public void PlayStone()
    {
        PlaySound(soundStone);
    }

    public void PlayAxeTree()
    {
        PlaySound(soundAxeTree);
    }

}
