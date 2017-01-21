using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	void Start() {
		SoundManager.Get().playMusic(SoundManager.musicNames.elevatorMusic);
	}

	public void PlayGame() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("GameSetup");
	}

	public void LoadAbout() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("About");
	}

	public void BackFromAbout() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("Title");
	}

	public void Quit() {
		Application.Quit();
	}
}
