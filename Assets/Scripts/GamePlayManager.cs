using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour {

	// Constants

	private const int ANSWERS_TO_DISPLAY = 4;
	
	// Scene objects

	public Canvas gamePlayCanvas;
	public Button answerButtonPrefab;
	public Text timerText;

	public Canvas gameOverCanvas;
	public Text endGameText;
	public Image outcomeImage;

	public Color goodColor = Color.green;
	public Color badColor = Color.red;

	public float buttonFadeTime = .25f;


	// Private variables

	private List<Button> answerButtons;

	private int correctAnswers;
	private int incorrectAnswers;
	private List<Option> currentOptions;

	private float currentTime = 0.0f;
	private int lastTime = 0;
	private bool buttonFade;
	private float buttonFadeCD;

	// Common methods

	void Start() {
		SoundManager.Get().playMusic(SoundManager.musicNames.fastPaceMusic);
		this.correctAnswers = 0;
		this.incorrectAnswers = 0;
		this.answerButtons = new List<Button>();
		this.currentOptions = new List<Option>();

		GenerateButtons();

		startTimer();

	}

	void Update() {
		if (buttonFade) {
			buttonFadeCD -= Time.deltaTime;
			foreach(Button button in answerButtons) {
				Color fadeColor = button.colors.highlightedColor;
				fadeColor.a *= Time.deltaTime * -2.5f; // decrement so it takes .25s to fade out
				button.colors = SetColorBlock(button.colors, fadeColor);
			}
			if (buttonFadeCD <= 0.0f) {
				buttonFade = false;
				ResetButtons();
			}
		}
		else if (currentTime > 0.0f) {
			currentTime -= Time.deltaTime;
			timerText.text = currentTime.ToString("N0");
			if (int.Parse(timerText.text) != lastTime) {
				lastTime = int.Parse(timerText.text);
				if (lastTime < 10) {
					SoundManager.Get().playSoundEffect(SoundManager.SFXNames.timerUrgentSFX);
				} else {
					SoundManager.Get().playSoundEffect(SoundManager.SFXNames.timerNormalSFX);
				}
			}
			if (currentTime <= 0) {
				GameOver();
			}
		}
	}

	/*****************
	 * Generic
	 ****************/
	public void Reset() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		GameManager.Get().rememberPlayers = true;
		GameManager.Get().Reset();
		SceneManager.LoadScene("GameSetup");
	}

	public void Quit() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		SoundManager.instance.musicSource.GetComponent<AudioSource>().Stop();
		GameManager.Get().rememberPlayers = false;
		GameManager.Get().Quit();
		GameManager.ClearManager();
		SceneManager.LoadScene("Title");
	}

	public void ResetButtons() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.buttonTapSFX);
		foreach (Option op in currentOptions) {
			op.onScreen = false;
		}
		currentOptions.Clear();
		foreach (Button b in answerButtons) {
			Destroy(b.gameObject);
		}
		answerButtons.Clear();
		GenerateButtons();
	}


	// Private methods

	private void GenerateButtons() {
		var outsetImage = gamePlayCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "AnswerArea");
		int playerIndex = GameManager.Get().GetRandomUnfinishedPlayer();
		//Ensure at least one option is correct
		int correctButton = Random.Range(0, ANSWERS_TO_DISPLAY);
		//Ensure there is at least one unfinished player
		if (playerIndex != Option.DEFAULT_INDEX) {
			//Loop through answer list
			for (int i = 0; i < ANSWERS_TO_DISPLAY; i++) {
				Option temp = null;
				//If where correct button should be, select a correct option
				if (i == correctButton) {
					temp = GameManager.Get().GetRandomOptionForPlayer(playerIndex);	
				}
				// otherwise select a random option (can be more than one correct option!)
				else {
					temp = GameManager.Get().GetRandomAvailableOption();
				}
				temp.onScreen = true;
				currentOptions.Add(temp);
			}
			RectTransform rectTrans = answerButtonPrefab.GetComponent<RectTransform>();
			for (int i = 0; i < ANSWERS_TO_DISPLAY; i++) {
				var position = new Vector2(0, -75 - (i * rectTrans.rect.height));
				var newButton = Instantiate(answerButtonPrefab, position, Quaternion.identity);
				newButton.transform.SetParent(outsetImage.transform, false);
				newButton.GetComponentInChildren<Text>().text = currentOptions[i].text;
				AddListener(newButton, i);
				answerButtons.Add(newButton);
			}
		}
		else {
			GameOver();
			//ROUND HAS ENDED
		}

	}

	void AddListener(Button b, int value) {
		b.onClick.AddListener(() => ButtonPress(value));
	}

	public void Skip() {
		SoundManager.Get().playSoundEffect(SoundManager.SFXNames.wrongAnswerSFX);
		incorrectAnswers++;
		ResetButtons();
	}

	//BUTTON STUFF
	void ButtonPress(int button) {
		//IF the playerID is not zero, it is a correct option
		if (currentOptions[button].playerID >= 0) {
			SoundManager.Get().playSoundEffect(SoundManager.SFXNames.correctAnswerSFX);
			currentOptions[button].correctlyChosen = true;
			answerButtons[button].colors = SetColorBlock(answerButtons[button].colors, goodColor);
			correctAnswers++;
		//Otherwise they selected the wrong option
		} else {
			SoundManager.Get().playSoundEffect(SoundManager.SFXNames.wrongAnswerSFX);
			answerButtons[button].colors = SetColorBlock(answerButtons[button].colors, badColor);
			incorrectAnswers++;
		}
		//Make sure that the player can't select more options than one at a time. Setting interactible to false would also work
		foreach (Button b in answerButtons) {
			b.onClick.RemoveAllListeners();
		}
		buttonFade = true;
		buttonFadeCD = buttonFadeTime;
	}

	private void startTimer() {
		
		int highestDifficulty = 0;
		foreach (Player p in GameManager.Get().GetPlayers()) {
			highestDifficulty = (p.difficulty > highestDifficulty) ? p.difficulty : highestDifficulty;
		}
		highestDifficulty *= 6;
		highestDifficulty -= 3;
		currentTime = Mathf.Max((float)highestDifficulty, GameManager.MIN_PLAY_TIME); // ensure it can't be too short.
		timerText.text = highestDifficulty.ToString();
		lastTime = (int)currentTime;
		timerText.gameObject.SetActive(true);
	}

	private void GameOver() {
		currentTime = 0;
		lastTime = 0;
		SoundManager.Get().stopMusic();
		SoundManager.Get().stopSoundEffect();
		timerText.gameObject.SetActive(false);
		gamePlayCanvas.gameObject.SetActive(false);
		gameOverCanvas.gameObject.SetActive(true);
		string outputText = "";
		outputText += "Correct Scores: " + correctAnswers + ". Incorrect Answers: " + incorrectAnswers + "\n";
		float percentage = (float)correctAnswers / (float)GameManager.Get().NumCorrectOptions();
		percentage *= 100;
		//outputText += "Total: " + (int)percentage + "%\n"; this is a little confusing to display
		//Outcomes
		int outcome = 0;
		if (percentage >= 66) {
			outcome = 2;
		}
		else if (percentage >= 33) {
			outcome = 1;
		}
		if (outcome > 0) {
			SoundManager.Get().playSoundEffect(SoundManager.SFXNames.gameOverSuccessSFX);
		}
		else {
			SoundManager.Get().playSoundEffect(SoundManager.SFXNames.gameOverFailSFX);
		}

		foreach (Player p in GameManager.Get().GetPlayers()) {
			outputText += "Player " + p.name + "'s score: " + GameManager.Get().NumChosenCorrectAnswersFromPlayer(p.id)
				+ "/" + GameManager.Get().NumPossibleCorrectAnswersFromPlayer(p.id) + "\n";
		}
		outputText  += CharacterManager.GetCharacterOutcome(GameManager.Get().character, outcome);
		outcomeImage.sprite = CharacterManager.GetCharacterOutcomeImage(GameManager.Get().character, outcome);
		
		endGameText.text = outputText;
	}

	ColorBlock SetColorBlock (ColorBlock block, Color color) {
		block.highlightedColor = color;
		block.normalColor = color;
		block.pressedColor = color;
		return block;
	}
}
