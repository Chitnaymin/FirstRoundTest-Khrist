using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip startSound;
	public AudioClip flipSound;
	public AudioClip matchSound;
	public AudioClip mismatchSound;
	public AudioClip gameFinishSound;
	public AudioClip backgroundSound;
	public AudioSource backgroundMusicSource;
	public AudioSource soundEffectSource;

	void Awake() {
		backgroundMusicSource = GetComponent<AudioSource>();
		soundEffectSource = GetComponent<AudioSource>();
	}
	private void Start() {
		backgroundMusicSource.PlayOneShot(backgroundSound);
	}

	public void PlayStartSound() {
		soundEffectSource.PlayOneShot(startSound);
	}

	public void PlayFlipSound() {
		soundEffectSource.PlayOneShot(flipSound);
	}

	public void PlayMatchSound() {
		soundEffectSource.PlayOneShot(matchSound);
	}

	public void PlayMismatchSound() {
		soundEffectSource.PlayOneShot(mismatchSound);
	}

	public void PlayGameFinishSound() {
		soundEffectSource.PlayOneShot(gameFinishSound);
	}
}
