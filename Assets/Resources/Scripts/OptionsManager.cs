using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

	public Toggle musicMuteToggle;
	public Toggle soundMuteToggle;

	public Slider musicVolumeSlider;
	public Slider soundVolumeSlider;

	void Awake () {
		musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMute") == 1;
		soundMuteToggle.isOn = PlayerPrefs.GetInt("SoundMute") == 1;

		musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
		soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
	}

	public void MusicMuteToggle () {
		PlayerPrefs.SetInt("MusicMute",musicMuteToggle.isOn?1:0);
	}

	public void SoundMuteToggle () {
		PlayerPrefs.SetInt("SoundMute",soundMuteToggle.isOn?1:0);
	}

	public void MusicVolumeChange () {
		PlayerPrefs.SetFloat("MusicVolume",musicVolumeSlider.value);
	}

	public void SoundVolumeChange () {
		PlayerPrefs.SetFloat("SoundVolume",soundVolumeSlider.value);
	}
}
