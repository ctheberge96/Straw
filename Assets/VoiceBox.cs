using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceBox : MonoBehaviour
{

    public float letterDelay;
    public AudioClip[] letterSounds;
    private AudioSource audioPlayer;
    private int curLetter = 0;
    private char[] curWord;
    private float curDelay;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void Update() {

        //Only do these things if we have a word
        if (curWord != null) {

            //Ensure current letter is actually a letter
            while (curLetter < curWord.Length && !char.IsLetter(curWord[curLetter])) {
                curLetter++;
            }

            //No more letters
            if (curLetter >= curWord.Length) { ResetWord(); return; }
            
            if (curDelay <= 0) {

                //Speak it
                SayLetter(curWord[curLetter]);

                //Next letter
                curLetter++;
                
                //Adjust delay
                curDelay = letterDelay;

            } else {
                curDelay -= Time.deltaTime;
            }

        }

    }

    //Says a single letter.
    public void SayLetter(char letter) {
        if (char.IsLetter(letter)) {
            letter = char.ToLower(letter);
            audioPlayer.clip = letterSounds[letter - 97];
            audioPlayer.pitch = 1 + Random.Range(-.2f,.2f);
            audioPlayer.Play();
            StartCoroutine(WaitForSpeech());
        }
    }

    //Begins the proccess for saying a full word.
    public void SayWord(string word) {
        curWord = word.ToCharArray();
        curLetter = 0;
    }

    //Sets it so there is no current word
    private void ResetWord() {
        curWord = null;
    }

    //Wait until the speech has fully finished
    IEnumerator WaitForSpeech() {
        yield return new WaitWhile (()=> audioPlayer.isPlaying);
    }

}
