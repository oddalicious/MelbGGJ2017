using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	void Start() {
		SoundManager.Get().playMusic(SoundManager.musicNames.elevatorMusic);
	}

	public void PlayGame() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SceneManager.LoadScene("GameSetup");
	}

	public void Quit() {
		Application.Quit();
	}
}
