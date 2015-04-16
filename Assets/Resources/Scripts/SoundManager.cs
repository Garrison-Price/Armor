using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private AudioSource player;
	private static SoundManager _sm;
	
	public AudioClip menuMusic;
	public AudioClip levelMusic;

	private float musicVolume;
	private float soundVolume;

	private bool musicMuted;
	private bool soundMuted;

	// Use this for initialization
	void Awake () {
		if(_sm == null) {
			//First instance of the scene manager so make this the only one
			_sm = this;
			//Make the scene manager persist in all scenes
			_sm.player = GetComponentInChildren<AudioSource>();
			DontDestroyOnLoad(_sm.gameObject);
		} else {
			//Not the first instance
			if(_sm != this) //I am not the scene manager, destroy me
				Destroy(this.gameObject);
		}
		SetVolume();
	}

	void OnLevelWasLoaded() {
		if(Application.loadedLevelName.Equals("GameLevel"))
			PlayLevelMusic();
		else
			PlayMenuMusic();
	}

	void Update() {
		SetVolume();
	}
	
	private void PlayMenuMusic() {
		if(_sm.player.clip == null || !_sm.player.clip.Equals (menuMusic)) {
			_sm.player.clip = menuMusic;
			_sm.player.loop = true;
			_sm.player.Play();
		}
	}

	private void PlayLevelMusic() {
		if(_sm.player.clip == null || !player.clip.Equals (levelMusic)) {
			_sm.player.clip = levelMusic;
			_sm.player.loop = true;
			_sm.player.Play();
		}
	}

	private void SetVolume() {
		musicVolume = PlayerPrefs.GetFloat("MusicVolume",1f);
		musicMuted = PlayerPrefs.GetInt("MusicMute") == 1;

		soundVolume = PlayerPrefs.GetFloat("SoundVolume",0.5f);
		soundMuted = PlayerPrefs.GetInt("SoundMute") == 1;

		_sm.player.mute = soundMuted || musicMuted;
		_sm.player.volume = musicVolume * soundVolume;
	}
}
