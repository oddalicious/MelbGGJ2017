using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Private variables
	private Dictionary<string, string> musicList;
	private Dictionary<string, string> soundEffectsList;

	// Public variables
	public enum musicNames {
		elevatorMusic,
		upbeatMusic,
		grimMusic,
		fastPaceMusic
	}

	public enum SFXNames {
		incrementOrDecrementSFX,
		correctAnswerSFX,
		gameOverFailSFX,
		gameOverSuccessSFX,
		timerUrgentSFX,
		buttonTapSFX,
		timerNormalSFX,
		wrongAnswerSFX
	}


	// Common methods
	void Start() {
		setupMusic();
		setupSoundEffects();
	}


	// Private methods
	private void setupMusic() {
		musicList = new Dictionary<string, string>();

		musicList[musicNames.elevatorMusic.ToString()] = "01.mp3";
		musicList[musicNames.upbeatMusic.ToString()] = "02.mp3";
		musicList[musicNames.grimMusic.ToString()] = "03.mp3";
		musicList[musicNames.fastPaceMusic.ToString()] = "04.mp3";
	}

	private void setupSoundEffects() {
		soundEffectsList = new Dictionary<string, string>();

		soundEffectsList[SFXNames.incrementOrDecrementSFX.ToString()] = "01incOrDecPlayerCount.ogg";
		soundEffectsList[SFXNames.correctAnswerSFX.ToString()] = "02correct.ogg";
		soundEffectsList[SFXNames.gameOverFailSFX.ToString()] = "03gameOverFail.ogg";
		soundEffectsList[SFXNames.gameOverSuccessSFX.ToString()] = "04gameOverSuccess.ogg";
		soundEffectsList[SFXNames.timerUrgentSFX.ToString()] = "05timerUrgent.ogg";
		soundEffectsList[SFXNames.buttonTapSFX.ToString()] = "06buttonTap.ogg";
		soundEffectsList[SFXNames.timerNormalSFX.ToString()] = "07timerNormal.ogg";
		soundEffectsList[SFXNames.wrongAnswerSFX.ToString()] = "08wrong.ogg";
	}




}
