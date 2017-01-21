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
	private float currentTime = 0.0f;

	private List<string> hexColours = new List<string> {"#f1c40f", "#2ecc71", "#3498db", "#9b59b6", "#e67e22", "#1abc9c", "#e74c3c"};

	// Common methods

	void Start () {
		visibleOptions = new List<GameObject>();
		GameManager.Get().SetupGame();
		playersTurn = 0;
		setPlayerActionText();
	}

	void Update() {
		if (currentTime > 0.0f) {
			currentTime -= Time.deltaTime;
			timerText.text = currentTime.ToString("N0");
			if (currentTime <= 0) {
				timerText.gameObject.SetActive(false);
				LoadNextPlayer();
			}
		}
	}

	// Private methods

	private void setPlayerActionText() {
		playerActionText.text = string.Format("Pass device to {0}", GameManager.Get().GetPlayer(playersTurn));
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
			//LOAD NEXT SCENE
		}
	}

	// Button actions methods

	public void displayOptionsForUser() {
		passDeviceCanvas.gameObject.SetActive(false);
		optionsCanvas.gameObject.SetActive(true);

		Image insetImage = optionsCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "InsetImage");

		int maxOptions = GameManager.Get().NumberOfOptions;
		for (int i = 0; i < maxOptions + 1; i++) {
			Vector2 position = new Vector2(100, 200 - (i * 100));
			var option = Instantiate(optionText, position, Quaternion.identity);

			option.text = string.Format("<color={0}>{1}</color>", hexColours[i % hexColours.Count], GameManager.Get().GetCorrectOptionForPlayer(i, playersTurn).text);

			option.transform.SetParent(insetImage.transform, false);
			visibleOptions.Add(option.gameObject);
			currentTime = timeToView;
			timerText.gameObject.SetActive(true);
		}

	}
}
