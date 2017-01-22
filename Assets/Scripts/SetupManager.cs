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
	public Text introStoryTitle;
	public Text introStoryText;
	public Image mainImage;


	// Prefabs

	public InputField playerNameInputField;

	private List<Player> tempPlayerList;



	// Common methods

	void Start() {
		if (playerNumbersCanvas == null)
			Debug.Log("NO PLAYERNUMBERS CANVAS DETECTED!");
		if (playerConfigCanvas == null)
			Debug.Log("NO PLAYERCONFIG CANVAS DETECTED!");
		if (GameManager.Get().rememberPlayers) {
			numberOfPlayersText.text = GameManager.Get().NumPlayers.ToString();
			tempPlayerList = GameManager.Get().GetPlayers();
		}
	}


	// Button actions methods

	public void IncrementPlayers(Text numText) {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.incrementOrDecrementSFX);
		int numberOfPlayers = int.Parse(numberOfPlayersText.text);
		if (numberOfPlayers < GameManager.MAX_PLAYERS) {
			numberOfPlayers++;
			numberOfPlayersText.text = numberOfPlayers.ToString();
		}
	}
	public void DecrementPlayers(Text numText) {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.incrementOrDecrementSFX);
		int numberOfPlayers = int.Parse(numberOfPlayersText.text);
		if (numberOfPlayers > GameManager.MIN_PLAYERS) {
			numberOfPlayers--;
			numberOfPlayersText.text = numberOfPlayers.ToString();
		}

	}


	// Game State management methods

	public void MoveToPlayerConfig() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		if (playerNumbersCanvas) 
			playerNumbersCanvas.gameObject.SetActive(false);
		if (playerConfigCanvas)
			playerConfigCanvas.gameObject.SetActive(true);

		GameManager.Get().LoadState(GameManager.GameState.PlayerConfig);

		Image bottomArea = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "BottomArea");
		for (int i = 0; i < int.Parse(numberOfPlayersText.text); i++) {
			var newField = Instantiate(playerNameInputField, new Vector2(0, 625 - (i * 120)), Quaternion.identity);
			newField.transform.SetParent(bottomArea.transform, false);			
			nameFields.Add(newField);
			//If the playerList is not null, The Game has to have been set to remember players.
			if (tempPlayerList != null && i < tempPlayerList.Count) {
				newField.text = tempPlayerList[i].name;
			}
		}
		GameManager.Get().ClearPlayers();
	}

	public void MoveToStoryMode() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		foreach (InputField playerName in nameFields) 
			GameManager.Get().AddPlayer(playerName.text);
		playerConfigCanvas.gameObject.SetActive(false);
		GameManager.Get().SetupGame();
		introStoryCanvas.gameObject.SetActive(true);
		introStoryTitle.text = CharacterManager.GetCharacterName(GameManager.Get().character);
		string[] story = CharacterManager.GetCharacterStory(GameManager.Get().character);
		string title = story[0];
		introStoryText.text = story[1];
		mainImage.sprite = CharacterManager.GetCharacterLogo(GameManager.Get().character,1,1);
		GameManager.Get().LoadState(GameManager.GameState.Storyline);
	}

	public void StartGame() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SoundManager.Get().stopMusic();
		GameManager.Get().LoadState(GameManager.GameState.PlayerLoop);
	}
}
