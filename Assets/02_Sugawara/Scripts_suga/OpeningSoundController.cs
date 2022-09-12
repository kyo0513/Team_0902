using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSoundController : MonoBehaviour
{
    public AudioSource openingAudio;
    public float audioStartTime;
    void Start()
    {
        openingAudio = GetComponent<AudioSource>();   
        openingAudio.time = audioStartTime;
        openingAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
