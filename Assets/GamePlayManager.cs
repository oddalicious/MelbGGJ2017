using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

	private float currentTime = 0.0f;


	//TODO: delete this once we generate from the previous scenes the answers/incorrect answers..
	private List<string> options = new List<string> {"Hug her!", "Do a cool dance", "Sleep", "Do some programming", "Watch TV"};


	// Common methods

	void Start() {
		this.correctAnswers = 0;
		this.incorrectAnswers = 0;
		this.answerButtons = new List<Button>();

		//TODO: maybe display a counter initially + some text telling players to place the phone in the centre, then call this..
		generateButtons();

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


	// Private methods

	private void generateButtons() {
		var outsetImage = gamePlayCanvas.GetComponentsInChildren<Image>().FirstOrDefault(img => img.name == "OutsetImage");

		for (int i = 0; i < ANSWERS_TO_DISPLAY; i++) {
			var position = new Vector2(0, 200 - (i * 100));
			var newButton = Instantiate(answerButtonPrefab, position, Quaternion.identity);
			newButton.transform.SetParent(outsetImage.transform, false);
			//TODO: actually update with one of the possible incorrect/correct answers
			newButton.GetComponentInChildren<Text>().text = options[i];

			answerButtons.Add(newButton);
		}
	}

	private void startTimer() {
		timerText.text = "30";
		currentTime = 30;
		timerText.gameObject.SetActive(true);
	}


}
