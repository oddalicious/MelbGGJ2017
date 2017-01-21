using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerLoopManager : MonoBehaviour {

	// Scene objects

	public Canvas passDeviceCanvas;
	public Text playerActionText;

	public Canvas optionsCanvas;
	public Text optionText;
	public Text timerText;
	public float timeToView = 5.0f;
	List<GameObject> visibleOptions;
	// Private variables

	private int playersTurn;
	private float currentTime = 0f;
	private int lastTime = 0;

	private List<string> hexColours = new List<string> {"#f1c40f", "#2ecc71", "#3498db", "#9b59b6", "#e67e22", "#1abc9c", "#e74c3c"};

	// Common methods

	void Start () {
		SoundManager.Get().playMusic(SoundManager.musicNames.upbeatMusic);
		visibleOptions = new List<GameObject>();
		
		GameManager.Get().LoadState(GameManager.GameState.PlayerLoop);
		playersTurn = 0;
		setPlayerActionText();
	}

	void Update() {
		if (currentTime > 0.0f) {
			currentTime -= Time.deltaTime;
			timerText.text = currentTime.ToString("N0");
			if (int.Parse(timerText.text) != lastTime) {
				lastTime = int.Parse(timerText.text);
				SoundManager.Get().playSoundEffect(SoundManager.SFXNames.timerNormalSFX);
			}
			if (currentTime <= 0) {
				timerText.gameObject.SetActive(false);
				LoadNextPlayer();
			}
		}
	}

	// Private methods

	private void setPlayerActionText() {
		playerActionText.text = string.Format("Pass device to {0}", GameManager.Get().GetPlayerName(playersTurn));
	}

	private void ShowPlayerLoadScreen() {

		foreach (GameObject option in visibleOptions) 
			Destroy(option);
		visibleOptions.Clear();

		passDeviceCanvas.gameObject.SetActive(true);
		optionsCanvas.gameObject.SetActive(false);
		setPlayerActionText();
	}

	private void LoadNextPlayer() {
		playersTurn++;
		if (playersTurn < GameManager.Get().NumPlayers) {
			ShowPlayerLoadScreen();
		}
		else {
			SoundManager.Get().stopMusic();
			GameManager.Get().LoadState(GameManager.GameState.Gameplay);
		}
	}

	// Button actions methods

	public void displayOptionsForUser() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		passDeviceCanvas.gameObject.SetActive(false);
		optionsCanvas.gameObject.SetActive(true);

		Image insetImage = optionsCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "InsetImage");

		int maxOptions = GameManager.Get().GetPlayerDifficuty(playersTurn);
		for (int i = 0; i < maxOptions; i++) {
			Vector2 position = new Vector2(100, 200 - (i * 100));
			var option = Instantiate(optionText, position, Quaternion.identity);
			option.text = string.Format("<color={0}>{1}</color>", hexColours[i % hexColours.Count], GameManager.Get().GetOptionForPlayerAtIndex(i, playersTurn).text);

			option.transform.SetParent(insetImage.transform, false);
			visibleOptions.Add(option.gameObject);
			currentTime = timeToView;
			lastTime = (int)timeToView;
			timerText.gameObject.SetActive(true);
		}

	}
}
