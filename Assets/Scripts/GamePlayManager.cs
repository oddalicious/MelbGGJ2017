using System.Collections;
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


	// Private variables

	private List<Button> answerButtons;

	private int correctAnswers;
	private int incorrectAnswers;
	private List<Option> currentOptions;

	private float currentTime = 0.0f;


	//TODO: delete this once we generate from the previous scenes the answers/incorrect answers..
	//private List<string> options = new List<string> {"Hug her!", "Do a cool dance", "Sleep", "Do some programming", "Watch TV"};


	// Common methods

	void Start() {
		
		this.correctAnswers = 0;
		this.incorrectAnswers = 0;
		this.answerButtons = new List<Button>();
		this.currentOptions = new List<Option>();
		//TODO: maybe display a counter initially + some text telling players to place the phone in the centre, then call this..
		GenerateButtons();

		startTimer();

	}

	void Update() {
		if (currentTime > 0.0f) {
			currentTime -= Time.deltaTime;
			timerText.text = currentTime.ToString("N0");
			if (currentTime <= 0) {
				timerText.gameObject.SetActive(false);
				gamePlayCanvas.gameObject.SetActive(false);
				//TODO: implement game over!
				gameOverCanvas.gameObject.SetActive(true);
			}
		}
	}

	/*****************
	 * Generic
	 ****************/
	public void Reset() {
		GameManager.Get().Reset();
		SceneManager.LoadScene("GameSetup");
	}

	public void Quit() {
		GameManager.Get().Quit();
		SceneManager.LoadScene("Title");
	}

	public void ResetButtons() {
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
		var outsetImage = gamePlayCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "OutsetImage");
		int playerIndex = GameManager.Get().GetRandomUnfinishedPlayer();
		//Ensure at least one option is correct
		int correctButton = Random.Range(0, ANSWERS_TO_DISPLAY);
		//Ensure there is at least one unfinished player
		if (playerIndex != Option.DEFAULT_INDEX) {
			//Loop through answer list
			for (int i = 0; i < ANSWERS_TO_DISPLAY; i++) {
				Option temp = null;
				//If correct button, select a correct answer
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
			for (int i = 0; i < ANSWERS_TO_DISPLAY; i++) {
				var position = new Vector2(0, 200 - (i * 100));
				var newButton = Instantiate(answerButtonPrefab, position, Quaternion.identity);
				newButton.transform.SetParent(outsetImage.transform, false);
				//TODO: actually update with one of the possible incorrect/correct answers
				newButton.GetComponentInChildren<Text>().text = currentOptions[i].text;
				if (currentOptions[i].playerID != Option.DEFAULT_INDEX) {
					newButton.GetComponentInChildren<Text>().color = Color.red;
				}
				answerButtons.Add(newButton);
			}
		}
		else {
			//ROUND HAS ENDED
		}

	}

	private void startTimer() {
		timerText.text = "30";
		currentTime = 30;
		timerText.gameObject.SetActive(true);
	}
}
