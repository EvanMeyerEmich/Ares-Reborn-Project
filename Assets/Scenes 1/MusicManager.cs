using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // The audio source that plays the music
    public AudioClip beginningLoop; // Music for the beginning (played once)
    public AudioClip generalLoop; // Music for the general loop
    public AudioClip combatLoop;
    private float loopDuration = 14f;
    private bool beginningPlayed = false; // Track if the beginning music has played
    private bool hasStarted = false;
    public EnemyController enemy;

    void Start()
    {
        // Start by playing the beginning loop
        PlayMusic(beginningLoop, false);
        StartCoroutine(WaitForLoopEnd());
    }
      private IEnumerator WaitForLoopEnd()
    {
        yield return new WaitForSeconds(loopDuration);
        if (!hasStarted)
        {
            hasStarted = true;
            PlayMusic(generalLoop, true); // Start looping the general music
        }
        if(enemy.isChasing == true){
            PlayMusic(combatLoop, true);
        }

    }
    // Function to play a given music clip
    private void PlayMusic(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
