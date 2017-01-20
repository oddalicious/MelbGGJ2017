using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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

		if (options == null)
			options = new List<Option>();

		//Set this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	public void AddPlayer(string name) {
		players.Add(name);
	}

	public void LoadState(GameState newGameState) {
		if (gameState == GameState.Storyline && newGameState == GameState.PlayerLoop) {
			SceneManager.LoadScene("PlayerLoop");
		}
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
		if (currentTurn + 1 < players.Count) {
			currentTurn += 1;
		}
		else {
			currentTurn = 0;
			//LoadNextState();
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

//	public void ContinueSetPlayers(int players) {
//		LoadNextState();
//	}
//
//	public void ContinuePlayerConfig() {
//		GeneratePlayerLists();
//		LoadNextState();
//	}

	public void ContinueStoryline() {

	}

	public void ContinuePassNextPlayer() {

	}

	public void ContinueReadOptions() {

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
		} else {
			Debug.Log("index '" + index + "' out of bounds");
			return "";
		}
	}
}
