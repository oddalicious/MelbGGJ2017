using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;

public class GameManager {

	// Constants
	public const int MIN_PLAYERS = 2;
	public const int MAX_PLAYERS = 4;

	private static GameManager Instance = null;

	public enum GameState {
		PlayerSelect = 0,
		PlayerConfig = 1,
		Storyline = 2,
		PlayerLoop = 3,
		Gameplay = 4,
		Outcome = 5
	}

	public enum PlayerLoop {
		NextPlayer = 0,
		ReadOptions = 1
	}

	private int numberOfOptions = 3;

	public int NumberOfOptions
	{
		get
		{
			return numberOfOptions;
		}
	}

	//Make sure it can't be made by calling from another object.
	protected GameManager() {
		if (players == null)
			players = new List<string>();

		//TODO: remove this, its just for testing the PlayerLoop scene
		players.Add("playerOne");
		players.Add("playerTwo");	

		if (options == null)
			options = new List<Option>();
	}

	private List<string> players;
	private List<Option> options;
	private GameState gameState;

	public int NumPlayers
	{
		get
		{
			if (players != null)
				return players.Count;

			else
				return 0;
		}
	}

	public void AddPlayer(string name) {
		if (players != null)
			players.Add(name);
	}

	public void RemovePlayer() {
		if (players != null && players.Count > 0)
		players.RemoveAt(players.Count - 1);
	}

	public void LoadState(GameState newGameState) {
		if (gameState == GameState.Storyline && newGameState == GameState.PlayerLoop) 
			SceneManager.LoadScene("PlayerLoop");
		
		this.gameState = newGameState;
	}

	private void GeneratePlayerLists() {
		ShuffleOptions();

		//Loop through players
		int count = 0;
		for (int i = 0; i < players.Count; i++) {
			//loop through difficulty
			for (int j = 0; j < numberOfOptions; j++) {
				//Let it know which Player it has
				options[count].playerID = i;
				count++;
			}
		}
	}

	//Sets the ID back to the Default
	void ResetOptions() {
		for (int i = 0; i < options.Count; i++) {
			options[i].playerID = Option.DEFAULT_INDEX;
			options[i].correctlyChosen = false;
			options[i].onScreen = false;
		}
	}

	//Self Explanatory
	void ShuffleOptions() {
		for (int i = 0; i < options.Count; i++) {
			Option temp = options[i];
			int randomIndex = UnityEngine.Random.Range(i, options.Count);
			options[i] = options[randomIndex];
			options[randomIndex] = temp;
		}
	}

	public Option GetAvailableOption() {
		Option temp;
		ShuffleOptions();
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList();
		if (tempOptions.Count == 0) 
			temp = Option.GenerateEmptyOption();
		
		else 
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];
		
		return temp;
	}

	public Option GetCorrectAvailableOption() {
		Option temp;
		ShuffleOptions();
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen && n.id != Option.DEFAULT_INDEX)).ToList();

		if (tempOptions.Count == 0)
			temp = Option.GenerateEmptyOption();

		else
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];

		return temp;
	}

	public Option GetCorrectOptionForPlayer(int index, int playerID) {
		Option temp = Option.GenerateEmptyOption();
		int count = 0;
		foreach (Option option in options) {
			if (option.playerID == playerID) {
				if (count == index) {
					temp = option;
					break;
				}
				count++;
			}
		}

		return temp;
	}

	public List<Option>GetXAvailableOptions(int x) {
		List<Option> returnList = new List<Option>();
		ShuffleOptions();
		List<Option> tempList = options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList();

		for (int i = 0; i < x; i++)
			returnList.Add(tempList[i]);

		return returnList;
	}


	public static GameManager Get() {
		if (Instance == null) 
			Instance = new GameManager();
		
		return Instance;
	}

	public void Reset() {
		ResetOptions();
		gameState = GameState.Storyline;
	}

	public void SetupGame() {
		LoadOptionsFromText();
		GeneratePlayerLists();
	}

	void Quit() {

	}

	public string GetPlayer(int index) {
		if (index < players.Count) 
			return players[index];

		else {
			Debug.Log("index '" + index + "' out of bounds");
			return "";
		}
	}

	bool LoadOptionsFromText(string fileName = "Words") {
		// Handle any problems that might arise when reading the text
		try {
			string[] lines;
			TextAsset textAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
			if (textAsset) {

				lines = textAsset.text.Split("\n"[0]);

				int count = 0;
				if (lines.Length > 0) {
					for (int i = 0; i < lines.Length; i++) {
						options.Add(Option.GenerateOption(count, lines[i]));
						count++;
					}
				}
				return true;
			}
			return false;
		}
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e) {
			Console.WriteLine("{0}\n", e.Message);
			return false;
		}
	}
}