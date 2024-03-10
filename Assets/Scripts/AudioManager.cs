using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to manage the audio, this can be called with an index to
// play a specified sound
public class AudioManager : MonoBehaviour
{
    // Used to reference this one and only instance 
    public static AudioManager instance;

    public AudioSource[] SFX;

    void Awake()
    {
        // If this instance doesnt already exist, create it
        // otherwise destroy the game object calling Awake
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Takes in an int and stops the sound effect if it already playing
    // then it plays the sound effect linked to that int.
    public void PlaySFX(int sfx)
    {
        SFX[sfx].Stop();

        SFX[sfx].Play();
    }
}