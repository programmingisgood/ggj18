using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
	private AudioSource audioSource;

	void Start ()
	{
		audioSource = this.GetComponent<AudioSource>();
	}

	public void playAudio()
	{
		audioSource.PlayOneShot(audioSource.clip);
	}
}
