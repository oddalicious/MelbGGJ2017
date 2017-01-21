using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SetupManager : MonoBehaviour {


	// Scene objects

	public Canvas playerNumbersCanvas;
	public Text numberOfPlayersText;

	public Canvas playerConfigCanvas;
	private List<InputField> nameFields = new List<InputField>(); //these are generated

	public Canvas introStoryCanvas;


	// Prefabs

	public InputField playerNameInputField;



	// Common methods

	void Start() {
		if (playerNumbersCanvas == null)
			Debug.Log("NO PLAYERNUMBERS CANVAS DETECTED!");
		if (playerConfigCanvas == null)
			Debug.Log("NO PLAYERCONFIG CANVAS DETECTED!");
	}


	// Button actions methods

	public void IncrementPlayers(Text numText) {
		int numberOfPlayers = int.Parse(numberOfPlayersText.text);
		if (numberOfPlayers < GameManager.MAX_PLAYERS) {
			numberOfPlayers++;
			numberOfPlayersText.text = numberOfPlayers.ToString();
		}
	}
	public void DecrementPlayers(Text numText) {
		int numberOfPlayers = int.Parse(numberOfPlayersText.text);
		if (numberOfPlayers > GameManager.MIN_PLAYERS) {
			numberOfPlayers--;
			numberOfPlayersText.text = numberOfPlayers.ToString();
		}

	}


	// Game State management methods

	public void MoveToPlayerConfig() {
		if (playerNumbersCanvas) 
			playerNumbersCanvas.gameObject.SetActive(false);
		if (playerConfigCanvas)
			playerConfigCanvas.gameObject.SetActive(true);

		GameManager.Get().LoadState(GameManager.GameState.PlayerConfig);

		for (int i = 0; i < int.Parse(numberOfPlayersText.text); i++) {
			Image insetImage = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "InsetImage");
			var newField = Instantiate(playerNameInputField, new Vector2(insetImage.transform.position.x, 80 - (i * 75)), Quaternion.identity);
			newField.transform.SetParent(insetImage.transform, false);
			nameFields.Add(newField);
		}
	}

	public void MoveToStoryMode() {
		foreach (InputField playerName in nameFields) 
			GameManager.Get().AddPlayer(playerName.text);

		playerConfigCanvas.gameObject.SetActive(false);
		introStoryCanvas.gameObject.SetActive(true);

		GameManager.Get().LoadState(GameManager.GameState.Storyline);
	}

	public void StartGame() {
		GameManager.Get().LoadState(GameManager.GameState.PlayerLoop);
	}
}
