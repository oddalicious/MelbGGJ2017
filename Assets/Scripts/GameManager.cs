using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

	private int difficulty = 3;

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

	private int currentTurn = 0;
	private List<string> players;
	private List<Option> options;
	private GameState gameState;
	private PlayerLoop playState;

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
		players.Add(name);
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
			for (int j = 0; j < difficulty; j++) {
				//Let it know which Player it has
				options[count].playerID = i;
				count++;
			}
		}
	}

	void IteratePlayerLoop() {
		if (currentTurn + 1 < players.Count) 
			currentTurn += 1;
		
		else 
			currentTurn = 0;
		
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

	public Option GenerateUnselectedOption() {
		Option temp;
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList();
		if (tempOptions.Count > 0) 
			temp = Option.GenerateEmptyOption();
		
		else 
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];
		

		return temp;
	}

	public static GameManager Get() {
		if (Instance == null) 
			Instance = new GameManager();
		
		return Instance;
	}

	public void Reset() {
		ResetOptions();
		gameState = GameState.Storyline;
		currentTurn = 0;
		playState = PlayerLoop.NextPlayer;
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
}
