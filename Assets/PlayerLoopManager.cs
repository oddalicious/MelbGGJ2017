using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoopManager : MonoBehaviour {

	// Scene objects

	public Canvas passDeviceCanvas;
	public Text playerActionText;

	public Canvas optionsCanvas;
	public Text optionText;

	// Private variables

	private int playersTurn;

	private List<string> hexColours = new List<string> {"#f1c40f", "#2ecc71", "#3498db", "#9b59b6", "#e67e22", "#1abc9c", "#e74c3c"};

	//TODO: delete this once we read from file..
	private List<string> options = new List<string> {"Hug her!", "Do a cool dance", "Sleep", "Do some programming", "Watch TV"};


	// Common methods

	void Start () {
		playersTurn = 0;
		setPlayerActionText();
	}


	// Private methods

	private void setPlayerActionText() {
		playerActionText.text = string.Format("Pass device to {0}", GameManager.Get().GetPlayer(playersTurn));
	}


	// Button actions methods

	public void displayOptionsForUser() {
		passDeviceCanvas.gameObject.SetActive(false);
		optionsCanvas.gameObject.SetActive(true);

		//TODO: display more or less options depending on the player difficulty
		int maxOptions = options.Count;
		for (int i = 0; i < maxOptions; i++) {
			Vector2 position = new Vector2(0, 300 - (i * 100));

			var option = Instantiate(optionText, position, Quaternion.identity);

			//TODO: update the option text to be read from file...
			option.text = string.Format("<color={0}>{1}</color>", hexColours[i % hexColours.Count], options[i]);

			option.transform.SetParent(optionsCanvas.transform, false);
		}
	}
}
