using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmStarter : MonoBehaviour
{
    AudioSource bgmAudio;
    // Start is called before the first frame update
    void Start()
    {
        bgmAudio = GetComponent<AudioSource>();
        bgmAudio.playOnAwake = true;
        bgmAudio.loop = false;
        SoundManager.Instance.GetSound(bgmAudio, 1, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
