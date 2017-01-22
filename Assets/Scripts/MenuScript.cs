using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public bool options = false;
	public Toggle musicToggle;
	public Toggle soundEffectToggle;
	public bool mainMenu = false;

	void Start() {
		if (options) {
			musicToggle.isOn = SoundManager.instance.getMusicEnabled();
			soundEffectToggle.isOn = SoundManager.instance.getSoundEffectEnabled();
		}
		if (!SoundManager.instance.musicSource.GetComponent<AudioSource>().isPlaying) {
			SoundManager.Get().playMusic(SoundManager.musicNames.elevatorMusic);
		}
		
	}

	public void PlayGame() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("GameSetup");
	}

	public void LoadAbout() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("About");
	}

	public void LoadInstructions() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("Instructions");
	}

	public void LoadOptions() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("Options");
	}

	public void BackToTitle() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("Title");
	}

	public void ToggleSoundEffects() {
		bool value = SoundManager.instance.ToggleSoundEffectSource();
		soundEffectToggle.isOn = value;
	}

	public void ToggleMusic() {
		bool value = SoundManager.instance.ToggleMusicSource();
		musicToggle.isOn = value;
	}

	public void Quit() {
		Application.Quit();
	}
}
