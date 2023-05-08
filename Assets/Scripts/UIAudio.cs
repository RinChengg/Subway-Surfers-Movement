using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip _hover;
    [SerializeField] private AudioClip _select;

    [Header("Speakers")]
    [SerializeField] private AudioSource _sfxSpeaker;

    public void OnHover() { _sfxSpeaker.PlayOneShot(_hover); }
    public void OnSelect() { _sfxSpeaker.PlayOneShot(_select); }
}
