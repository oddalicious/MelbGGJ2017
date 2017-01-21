using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Private variables
	private Dictionary<string, string> musicList;
	private Dictionary<string, string> soundEffectsList;


	// Common methods
	void Start() {
		setupMusic();
		setupSoundEffects();
	}


	// Private methods
	private void setupMusic() {
		musicList = new Dictionary<string, string>();

		musicList["elevatorMusic"] = "01.mp3";
		musicList["upbeatMusic"] = "02.mp3";
		musicList["grimMusic"] = "03.mp3";
		musicList["fastPaceMusic"] = "04.mp3";
	}

	private void setupSoundEffects() {
		soundEffectsList = new Dictionary<string, string>();

		soundEffectsList["incrementOrDecrementSFX"] = "01incOrDecPlayerCount.ogg";
		soundEffectsList["correctAnswerSFX"] = "02correct.ogg";
		soundEffectsList["gameOverFailSFX"] = "03gameOverFail.ogg";
		soundEffectsList["gameOverSuccessSFX"] = "04gameOverSuccess.ogg";
		soundEffectsList["timerUrgentSFX"] = "05timerUrgent.ogg";
		soundEffectsList["buttonTapSFX"] = "06buttonTap.ogg";
		soundEffectsList["timerNormalSFX"] = "07timerNormal.ogg";
		soundEffectsList["wrongAnswerSFX"] = "08wrong.ogg";
	}

}
