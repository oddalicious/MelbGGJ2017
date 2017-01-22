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
	private List<InputField> difficultyFields = new List<InputField>();
	public Button continueNameButton;

	public Canvas introStoryCanvas;
	public Text introStoryTitle;
	public Text introStoryText;
	public Image mainImage;


	// Prefabs

	public InputField playerNameInputField;
	public InputField playerDifficultyInputField;
	public Button decrementButton;
	public Button incrementButton;

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

	private void Update() {
		bool donewithNames = true;
		foreach (InputField field in nameFields) {
			if (field.text.Length < 2)
				donewithNames = false;
		}
		continueNameButton.interactable = donewithNames;
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
		Image nameArea = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "NameArea");
		Image difficultyArea = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "DifficultyArea");
		for (int i = 0; i < int.Parse(numberOfPlayersText.text); i++) {
			var newField = Instantiate(playerNameInputField, new Vector2(25, 480 - (i * 120)), Quaternion.identity);
			newField.transform.SetParent(nameArea.transform, false);			
			nameFields.Add(newField);
			//If the playerList is not null, The Game has to have been set to remember players.
			

			var newDifficulty = Instantiate(playerDifficultyInputField, new Vector2(0, 480 - (i * 120)), Quaternion.identity);
			var newDecrement = Instantiate(decrementButton, new Vector2(-100, 480 - (i * 120)), Quaternion.identity);
			AddDecrementButton(newDecrement, i);
			var newIncrement = Instantiate(incrementButton, new Vector2(100, 480 - (i * 120)), Quaternion.identity);
			AddIncrementButton(newIncrement, i);
			newIncrement.transform.SetParent(difficultyArea.transform, false);
			newDecrement.transform.SetParent(difficultyArea.transform, false);
			newDifficulty.transform.SetParent(difficultyArea.transform, false);
			newDifficulty.interactable = false;
			
			difficultyFields.Add(newDifficulty);

			if (tempPlayerList != null && i < tempPlayerList.Count) {
				newField.text = tempPlayerList[i].name;
				newDifficulty.text = tempPlayerList[i].difficulty.ToString();
			}
			else {
				newDifficulty.text = GameManager.DEFAULT_DIFFICULTY.ToString();
			}
			//HANDLE DIFFICULTY THINGS HERE!!!

		}
		GameManager.Get().ClearPlayers();
	}

	void AddIncrementButton(Button b, int index) {
		b.onClick.AddListener(() => IncrementDifficulty(index));
	}

	void AddDecrementButton(Button b, int index) {
		b.onClick.AddListener(() => DecrementDifficutly(index));
	}

	public void IncrementDifficulty(int id) {
		int tempdifficulty = 0;
		if (int.TryParse(difficultyFields[id].text, out tempdifficulty)) {
			tempdifficulty++;
			if (tempdifficulty <= GameManager.MAX_DIFFICULTY) {
				difficultyFields[id].text = tempdifficulty.ToString();
			}
		}
	}

	public void DecrementDifficutly(int id) {
		int tempdifficulty = 0;
		if (int.TryParse(difficultyFields[id].text, out tempdifficulty)) {
			tempdifficulty--;
			if (tempdifficulty >= GameManager.MIN_DIFFICULTY) {
				difficultyFields[id].text = tempdifficulty.ToString();
			}
		}
	}

	public void MoveToStoryMode() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		for (int i = 0; i < nameFields.Count; i++) {
			GameManager.Get().AddPlayer(nameFields[i].text);
			int tempDifficulty = 0;
			if (int.TryParse(difficultyFields[i].text, out tempDifficulty))
				GameManager.Get().GetPlayer(i).difficulty = tempDifficulty;
			else
				GameManager.Get().GetPlayer(i).difficulty = GameManager.DEFAULT_DIFFICULTY;

		}
		playerConfigCanvas.gameObject.SetActive(false);
		GameManager.Get().SetupGame();
		introStoryCanvas.gameObject.SetActive(true);
		introStoryTitle.text = CharacterManager.GetCharacterName(GameManager.Get().character);
		string[] story = CharacterManager.GetCharacterStory(GameManager.Get().character);
		string title = story[0];
		introStoryText.text = story[1];
		mainImage.sprite = CharacterManager.GetCharacterLogo(GameManager.Get().character);
		GameManager.Get().LoadState(GameManager.GameState.Storyline);
	}

	public void StartGame() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SoundManager.Get().stopMusic();
		GameManager.Get().LoadState(GameManager.GameState.PlayerLoop);
	}
}
