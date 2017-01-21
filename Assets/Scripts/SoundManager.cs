using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Private variables
	private Dictionary<string, string> musicList;
	private Dictionary<string, string> soundEffectsList;

	// Public variables

	public static SoundManager instance = null;

	public GameObject musicSource;
	public GameObject soundEffectsSource;


	// Enums

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

	void Awake () {
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}


	// Private methods
	private void setupMusic() {
		musicList = new Dictionary<string, string>();

		musicList[musicNames.elevatorMusic.ToString()] = "01";
		musicList[musicNames.upbeatMusic.ToString()] = "02";
		musicList[musicNames.grimMusic.ToString()] = "03";
		musicList[musicNames.fastPaceMusic.ToString()] = "04";
	}

	private void setupSoundEffects() {
		soundEffectsList = new Dictionary<string, string>();

		soundEffectsList[SFXNames.incrementOrDecrementSFX.ToString()] = "01incOrDecPlayerCount";
		soundEffectsList[SFXNames.correctAnswerSFX.ToString()] = "02correct";
		soundEffectsList[SFXNames.gameOverFailSFX.ToString()] = "03gameOverFail";
		soundEffectsList[SFXNames.gameOverSuccessSFX.ToString()] = "04gameOverSuccess";
		soundEffectsList[SFXNames.timerUrgentSFX.ToString()] = "05timerUrgent";
		soundEffectsList[SFXNames.buttonTapSFX.ToString()] = "06buttonTap";
		soundEffectsList[SFXNames.timerNormalSFX.ToString()] = "07timerNormal";
		soundEffectsList[SFXNames.wrongAnswerSFX.ToString()] = "08wrong";
	}

	// Public methods
	public static SoundManager Get() {
		if (instance == null)
			instance = new SoundManager();

		return instance;
	}

	public void playMusic(musicNames musicName) {
		Debug.Log(string.Format("here, music name is {0}", musicName));
		if (GameManager.Get().musicEnabled) {
			AudioSource source = musicSource.GetComponentInChildren<AudioSource>();

			var music = Resources.Load(string.Format("Music/Music/{0}", musicList[musicName.ToString()]), typeof(AudioClip)) as AudioClip;
			source.clip = music;

			source.Play();
		}
	}

	public bool ToggleMusicSource() {
		AudioSource source = musicSource.GetComponentInChildren<AudioSource>();
		return source.enabled = !source.enabled;
	}

	public bool ToggleSoundEffectSource() {
		AudioSource source = soundEffectsSource.GetComponentInChildren<AudioSource>();
		return source.enabled = !source.enabled;
	}

	public bool getSoundEffectEnabled() {
		return soundEffectsSource.GetComponentInChildren<AudioSource>().enabled;
	}

	public bool getMusicEnabled() {
		return musicSource.GetComponentInChildren<AudioSource>().enabled;
	}

	public void playSoundEffect(SFXNames sfxName) {
		if (GameManager.Get().soundEffectsEnabled) {
			AudioSource source = soundEffectsSource.GetComponentInChildren<AudioSource>();

			var music = Resources.Load(string.Format("Music/SFX/{0}", soundEffectsList[sfxName.ToString()]), typeof(AudioClip)) as AudioClip;
			source.clip = music;

			source.Play();
		}
	}

	public void stopMusic() {
		AudioSource source = musicSource.GetComponentInChildren<AudioSource>();
		source.Stop();
	}

	public void stopSoundEffect() {
		AudioSource source = soundEffectsSource.GetComponentInChildren<AudioSource>();
		source.Stop();
	}

}
