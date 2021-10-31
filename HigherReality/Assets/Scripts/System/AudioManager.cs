using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource blockAudio;
    [SerializeField] private AudioClip blockShort;
    [SerializeField] private AudioClip blockMed;
    [SerializeField] private AudioClip blockLong;

    private AudioManager playerAudio;
    private AudioManager backgroundAudio;

    public void playBlockSound(int n){
        switch(n){
            case 0:
                blockAudio.clip = blockShort;
                break;
            case 1:
                blockAudio.clip = blockMed;
                break;
            case 2:
                blockAudio.clip = blockLong;
                break;
        }

        if (!blockAudio.isPlaying) blockAudio.Play();
    }
    public void playPlayerSound(){
        
    }
}
