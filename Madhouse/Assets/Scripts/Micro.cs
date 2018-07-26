using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Micro : MonoBehaviour {

public float sensitivity = 90;
    public float loudness = 0;

    public GameObject obj;
    public new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(null, true, 10, 44100);
        audio.loop = true;
        audio.mute = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();
    }

    void Update()
    {
        loudness = GetAveragedVolume() * sensitivity;
        if (loudness > 20)
        {
            // something happens when Player is too loud
        }
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
}
