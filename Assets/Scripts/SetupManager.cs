using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SetupManager : MonoBehaviour {


	// Scene objects

	public Canvas playerNumbersCanvas;
	public Text numberOfPlayersText;
	public Canvas playerConfigCanvas;
	public Button continueNameButton;
	public Canvas introStoryCanvas;
	public Text introStoryTitle;
	public Text introStoryText;
	public Image mainImage;

	private List<InputField> nameFields = new List<InputField>(); //these are generated
	private List<InputField> difficultyFields = new List<InputField>();
	private List<string> defaultPlayerNames = new List<string>();


	// Prefabs

	public InputField playerNameInputField;
	public GameObject playerDifficultyArea;

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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.ClearManager();
			SceneManager.LoadScene("Title");
		}
		bool playersCompletedNameFill = true;
		foreach (InputField field in nameFields) {
			if (field.text.Length < 2)
				playersCompletedNameFill = false;
		}
		continueNameButton.interactable = playersCompletedNameFill;
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

		defaultPlayerNames = Player.GetDefaultPlayerNames();
		Utilities.Shuffle<string>(defaultPlayerNames);


		Image nameArea = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "NameArea");
		Image difficultyArea = playerConfigCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "DifficultyArea");
		//For each player, add a Name Field, Difficulty Field, and Increment/Decrement buttons.
		RectTransform rectTrans = playerNameInputField.GetComponent<RectTransform>();
		for (int i = 0; i < int.Parse(numberOfPlayersText.text); i++) {
			var newNameField = Instantiate(playerNameInputField, new Vector2(0, -40 - (rectTrans.rect.height * i)), Quaternion.identity);
			newNameField.transform.SetParent(nameArea.transform, false);	
			newNameField.text = defaultPlayerNames[i];		
			nameFields.Add(newNameField);

			var newPlayerDifficultyArea = Instantiate(playerDifficultyArea, new Vector2(0, -40 - (rectTrans.rect.height * i)), Quaternion.identity);
			newPlayerDifficultyArea.transform.SetParent(difficultyArea.transform, false);
			var inputField = newPlayerDifficultyArea.gameObject.GetComponentInChildren<InputField>();
			inputField.interactable = false;
			difficultyFields.Add(inputField);

			var buttons = newPlayerDifficultyArea.gameObject.GetComponentsInChildren<Button>();

			foreach (Button button in buttons) {
				if (button.name == "ButtonIncrement") {
					AddIncrementButton(button, i);
				} else if (button.name == "ButtonDecrement") {
					AddDecrementButton(button, i);
				}
			}

			if (tempPlayerList != null && i < tempPlayerList.Count) {
				newNameField.text = tempPlayerList[i].name;
				inputField.text = tempPlayerList[i].difficulty.ToString();
			} else {
				inputField.text = GameManager.DEFAULT_DIFFICULTY.ToString();
			}
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
		SoundManager.instance.playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		int tempdifficulty = 0;
		if (int.TryParse(difficultyFields[id].text, out tempdifficulty)) {
			tempdifficulty++;
			if (tempdifficulty <= GameManager.MAX_DIFFICULTY) {
				difficultyFields[id].text = tempdifficulty.ToString();
			}
		}
	}

	public void DecrementDifficutly(int id) {
		SoundManager.instance.playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
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
		// Loop through the players and add them to the game. Set difficulty and names based upon their respective fields
		for (int i = 0; i < nameFields.Count; i++) {
			GameManager.Get().AddPlayer(nameFields[i].text);
			int tempDifficulty = 0;
			// If the difficulty can't be parsed to an int somehow, give it the default difficulty.
			if (int.TryParse(difficultyFields[i].text, out tempDifficulty))
				GameManager.Get().GetPlayer(i).difficulty = tempDifficulty;
			else
				GameManager.Get().GetPlayer(i).difficulty = GameManager.DEFAULT_DIFFICULTY;

		}
		playerConfigCanvas.gameObject.SetActive(false);
		GameManager.Get().SetupGame();
		introStoryCanvas.gameObject.SetActive(true);

		//Populate Story data
		introStoryTitle.text = CharacterManager.GetCharacterName(GameManager.Get().character);
		string[] story = CharacterManager.GetCharacterStory(GameManager.Get().character);

		// IMPLEMENT THIS IF WE HAVE A TITLE
		//titleText.text title = story[0];
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
