using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Block")]
    [SerializeField] private AudioSource blockAudio;
    [SerializeField] private AudioClip blockShort;
    [SerializeField] private AudioClip blockMed;
    [SerializeField] private AudioClip blockLong;
    
    [Header("Runes")]
    [SerializeField] private AudioSource runeAudio;
    [SerializeField] private AudioClip runePickUp;
    [SerializeField] private AudioClip runeDrop;

    private PlayerMovement player;
    [Header("Player")]
    [Tooltip("playerAudio source should have playerMovement sounds attached; looped")]
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioSource clickAudio;
    [SerializeField] private AudioSource levelAudio;

    private void Awake(){
        // playerAudio.Pause();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update(){
        if(player.checkIsMoving){
            if (!playerAudio.isPlaying) playerAudio.Play();
        }
        else{
            if (playerAudio.isPlaying) playerAudio.Stop();
        }
    }

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

    public void playRuneSound(int n){
        if(n == 0) runeAudio.PlayOneShot(runePickUp, 1f);
        else runeAudio.PlayOneShot(runeDrop, 1f);
    }

    public void playClickerSound(){
        if (clickAudio != null) clickAudio.PlayOneShot(clickAudio.clip, 1f);
    }

    public void playEndingSound(){
        levelAudio.Play();
    }
}