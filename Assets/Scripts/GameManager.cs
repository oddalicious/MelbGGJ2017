using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static int PROTOTYPE_MAX_PLAYERS = 4;

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
		Loading = 0,
		ReadOptions = 1
	}

	private int difficulty = 3;

	//Make sure it can't be made by calling new.
	protected GameManager() { }

	private int currentTurn = 0;
	private List<string> players;
	private List<Option> options;
	private GameState gameState;
	private PlayerLoop playState;

	public int NumPlayers
	{
		get
		{
			return players.Count;
		}
	}

	void Awake() {
		if (Instance == null)
			Instance = this;

		else if (Instance != this)
			Destroy(gameObject);

		if (players == null)
			players = new List<string>();

		if (players.Count == 0)
			players.Add("");

		if (options == null)
			options = new List<Option>();

		//Set this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	public void SetPlayer(int id, string input) {
		if (id < players.Count)
		players[id] = input;
		else {
			Debug.Log("HOW DID YOU MANAGE TO CHANGE A PLAYER THAT DOESN'T EXIST!??");
		}
	}

	public void SetNumPlayers(int count) {
		players.Clear();
		for (int i = 0; i < count; i++)
			players.Add("");
	}

	public void LoadNextState() {
		switch (gameState) {
			case GameState.PlayerSelect:
				gameState = GameState.PlayerConfig;
				break;
			case GameState.PlayerConfig:
				gameState = GameState.Storyline;
				break;
			case GameState.Storyline:
				gameState = GameState.PlayerLoop;
				break;
			case GameState.PlayerLoop:
				gameState = GameState.Gameplay;
				break;
			case GameState.Gameplay:
				gameState = GameState.Outcome;
				break;
			case GameState.Outcome:
				gameState = GameState.PlayerSelect;
				break;
		}
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
		if (currentTurn + 1 < players.Count) {
			currentTurn += 1;
		}
		else {
			currentTurn = 0;
			LoadNextState();
		}
	}

	void ResetOptions() {
		for (int i = 0; i < options.Count; i++) {
			options[i].playerID = Option.DEFAULT_INDEX;
		}
	}

	void ShuffleOptions() {
		for (int i = 0; i < options.Count; i++) {
			Option temp = options[i];
			int randomIndex = Random.Range(i, options.Count);
			options[i] = options[randomIndex];
			options[randomIndex] = temp;
		}
	}

	public Option GenerateUnselectedOption() {
		Option temp;
		List<Option> tempOptions = options.Where(n => !n.selected).ToList();
		if (tempOptions.Count > 0) {
			temp = Option.GenerateEmptyOption();
		}
		else {
			temp = tempOptions[Random.Range(0, tempOptions.Count - 1)];
		}

		return temp;
	}

	public void ContinueSetPlayers(int players) {
		LoadNextState();
	}

	public void ContinuePlayerConfig() {
		GeneratePlayerLists();
		LoadNextState();
	}

	public void ContinueStoryline() {

	}

	public void ContinuePassNextPlayer() {

	}

	public void ContinueReadOptions() {

	}

	public void IncrementPlayers(Text numText) {
		if (players.Count < PROTOTYPE_MAX_PLAYERS) {
			players.Add("");
			if (numText)
				numText.text = players.Count.ToString();
		}
		else {
			Debug.Log("Hit max player cap");
		}
	}
	
	public void DecrementPlayers(Text numText) {
		if (players.Count > 1) {
			players.RemoveAt(players.Count - 1);
			if (numText)
				numText.text = players.Count.ToString();
		}
		else {
			Debug.Log("Can't go below one player");
		}
	}

	public static GameManager Get() {
		if (Instance == null) {
			Instance = Camera.main.gameObject.AddComponent<GameManager>();
		}
		return Instance;
	}

	public string GetPlayer(int index) {
		if (index < players.Count) {
			return players[index];
		}
		else {
			Debug.Log("index '" + index + "' out of bounds");
			return "";
		}
	}
}
