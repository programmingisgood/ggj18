using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClip : MonoBehaviour 
{
	private AudioSource audio;

	void Start () 
	{
		audio = this.GetComponent<AudioSource>();
	}
	
	public void playAudio()
	{
		audio.PlayOneShot(audio.clip);
	}
}
