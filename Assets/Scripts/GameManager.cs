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

	//***************************************************************
	//* Constants 
	//*************************************************************/

	public const int MIN_PLAYERS = 2;
	public const int MAX_PLAYERS = 4;
	public const int DEFAULT_DIFFICULTY = 3;
	public const int MAX_DIFFICULTY = 7;
	public const int MIN_DIFFICULTY = 3;

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

	//***************************************************************
	//* Variables
	//*************************************************************/

	// Public
	public bool soundEffectsEnabled = true;
	public bool musicEnabled = true;
	public int character = 0;
	public bool rememberPlayers = false;

	// Private
	private static GameManager Instance = null;
	private List<Player> players;
	private List<Option> options;
	private GameState gameState;
	

	//***************************************************************
	//* Accessors
	//*************************************************************/

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

	// Constructor - Protected so it can only construct itself.
	protected GameManager() {
		if (players == null)
			players = new List<Player>();

		if (options == null)
			options = new List<Option>();
	}

	//***************************************************************
	//* Public Functions
	//*************************************************************/

	//**********************
	//* Player Data
	//*********************/

	public void AddPlayer(string name, int difficulty = 3) {
		if (players != null)
			players.Add(new Player (players.Count, name, difficulty));
	}

	public void RemovePlayer() {
		if (players != null && players.Count > 0)
		players.RemoveAt(players.Count - 1);
	}

	public int GetPlayerDifficuty(int id) {
		int difficulty = 3;
		foreach (Player p in players) {
			if (p.id == id)
				return p.difficulty;
		}
		return difficulty;
	}

	public string GetPlayerName(int id) {
		if (id < players.Count) {
			foreach (Player p in players) {
				if (p.id == id)
					return p.name;
			}
		}
		else
			Debug.Log("index '" + id + "' out of bounds");
		return "";
	}

	public Player GetPlayer(int id) {
		if (id < players.Count) {
			foreach (Player p in players) {
				if (p.id == id)
					return p;
			}
		}
		else
			Debug.Log("index '" + id + "' out of bounds");
		return null;
	}

	public List<Player> GetPlayers() {
		return players;
	}

	public void ClearPlayers() {
		for (int i = 0; i < players.Count; i++) {
			players[i] = null;
		}
		players.Clear();
	}

	public void ShufflePlayers() {
		Shuffle<Player>(players);
	}

	//**********************
	//* Generic
	//*********************/

	public void LoadMainMenu() {
		SceneManager.LoadScene("Title");
	}

	public void LoadState(GameState newGameState) {	
		if (gameState == GameState.Storyline && newGameState == GameState.PlayerLoop) 
			SceneManager.LoadScene("PlayerLoop");
		else if (gameState == GameState.PlayerLoop && newGameState == GameState.Gameplay) {
			SceneManager.LoadScene("Game");
		}
		this.gameState = newGameState;

	}

	public static GameManager Get() {
		if (Instance == null)
			Instance = new GameManager();

		return Instance;
	}

	public void Reset() {
		ClearOptions();
		gameState = GameState.Storyline;
		if (!rememberPlayers)
			ClearPlayers();
	}

	public void SetupGame() {
		character = CharacterManager.GetRandomCharacter();
		bool[] characterFlag = new bool[CharacterManager.MAX_CHARACTERS];
		for (int i = 0; i < CharacterManager.MAX_CHARACTERS; i++)
			characterFlag[i] = false;
		ShufflePlayers();
		// Ensure generic answers are loaded
		LoadOptionsForPlayer(0);
		// Loop through players
		LoadOptionsForPlayer(character);

		GeneratePlayerLists();
		
	}

	public void Quit() {
		ClearPlayers();
		ClearOptions();
		gameState = GameState.PlayerSelect;
	}

	//**********************
	//* Options
	//*********************/


	public int NumChosenCorrectAnswersFromPlayer(int playerID) {
		return options.Where(n => (n.correctlyChosen && n.playerID == playerID)).ToList().Count;
	}

	public int NumPossibleCorrectAnswersFromPlayer(int playerID) {
		return options.Where(n => (n.playerID == playerID)).ToList().Count;
	}

	public Option GetRandomAvailableOption() {
		Option temp;
		Shuffle<Option>(options);
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList();
		if (tempOptions.Count == 0)
			temp = Option.GenerateEmptyOption();

		else
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];

		return temp;
	}

	public Option GetRandomCorrectAvailableOption() {
		Option temp;
		Shuffle<Option>(options);
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen && n.id != Option.DEFAULT_INDEX)).ToList();

		if (tempOptions.Count == 0)
			temp = Option.GenerateEmptyOption();

		else
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];

		return temp;
	}

	public Option GetOptionForPlayerAtIndex(int index, int playerID) {
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

	public Option GetRandomOptionForPlayer(int playerID) {
		Option temp = Option.GenerateEmptyOption();
		List<Option> tempList = options.Where(n => (!n.correctlyChosen && !n.onScreen && n.playerID == playerID)).ToList();
		if (tempList.Count > 0) {
			Shuffle<Option>(tempList);
			temp = tempList[0];
		}
		return temp;
	}

	public int GetNumberAvailableOptionsForPlayer(int playerID) {
		return options.Where(n => (!n.correctlyChosen && !n.onScreen && n.playerID == playerID)).ToList().Count;
	}

	public int GetNumberAvailableOptions() {
		return options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList().Count;
	}

	public int GetRandomUnfinishedPlayer() {
		List<Player> tempList = new List<Player>();
		foreach (Player p in players) {
			tempList.Add(p);
		}
		Shuffle<Player>(tempList);
		int randomIndex = 0;
		randomIndex = UnityEngine.Random.Range(0, tempList.Count - 1);
		int playerIndex = tempList[randomIndex].id;
		int count = GetNumberAvailableOptionsForPlayer(playerIndex);
		if (count == 0) {
			tempList.RemoveAt(randomIndex);

			do {
				try {
					randomIndex = UnityEngine.Random.Range(0, tempList.Count - 1);
					playerIndex = tempList[randomIndex].id;
					count = GetNumberAvailableOptionsForPlayer(playerIndex);
					tempList.RemoveAt(randomIndex);
				}
				catch (Exception e) {
					Debug.Log("INVALID INDEX WHEN GRABBING RANDOM PLAYER. \n" + e.ToString());
					return Option.DEFAULT_INDEX;
				}
			} while (tempList.Count > 0 && count == 0);
			
		}

		return (count > 0) ? playerIndex : Option.DEFAULT_INDEX;
	}

	//***************************************************************
	//* Private Functions
	//*************************************************************/

	private void GeneratePlayerLists() {
		Shuffle<Option>(options);

		//Loop through players
		foreach (Player player in players) {
			List<Option> tempList = options.Where(n => (n.playerID == Option.DEFAULT_INDEX)).ToList();
			//loop through difficulty
			for (int j = 0; j < player.difficulty; j++) {
				//Let it know which Player it has
				tempList[j].playerID = player.id;
			}
		}
	}

	private void ResetOptions() {
		for (int i = 0; i < options.Count; i++) {
			options[i].Reset();
		}
	}

	private void ClearOptions() {
		for (int i = 0; i < options.Count; i++) {
			options[i] = null;
		}
		options.Clear();
	}

	private void LoadOptionsForPlayer(int characterIndex) {
		// Handle any problems that might arise when reading the text
		string[] lines = ReadLinesFromTextFile(characterIndex);
		if (lines.Length > 0) {
			for (int i = 0; i < lines.Length; i++) {
				Option temp = Option.GenerateOption(options.Count, lines[i]);
				options.Add(temp);
				}
		}
	}

	private string[] ReadLinesFromTextFile(int characterIndex) {
		string[] lines = { "" };
		string fileName = CharacterManager.GetCharacterOptionsFilepath(characterIndex);
		try {
			TextAsset textAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
			if (textAsset) {
				lines = textAsset.text.Split("\n"[0]);
			}
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}

		return lines;
	}

	private void Shuffle<T>(List<T> list) {
		for (int i = 0; i < list.Count; i++) {
			T temp = list[i];
			int randomIndex = UnityEngine.Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}
}