using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    //take value on slider and set volume
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }
    //toggle fullscreen
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
