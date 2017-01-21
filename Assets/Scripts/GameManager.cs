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
			players = new List<Player>();

		//TODO: remove this, its just for testing the PlayerLoop scene
		AddPlayer("playerOne");
		AddPlayer("playerTwo");	

		if (options == null)
			options = new List<Option>();
	}

	private List<Player> players;
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
			players.Add(new Player (players.Count, name));
	}

	public void RemovePlayer() {
		if (players != null && players.Count > 0)
		players.RemoveAt(players.Count - 1);
	}

	public void LoadState(GameState newGameState) {	
		if (gameState == GameState.Storyline && newGameState == GameState.PlayerLoop) 
			SceneManager.LoadScene("PlayerLoop");
		else if (gameState == GameState.PlayerLoop) {
			SceneManager.LoadScene("Game");
		}
		this.gameState = newGameState;

	}

	private void GeneratePlayerLists() {
		ShuffleList(options);

		//Loop through players
		int count = 0;
		for (int i = 0; i < players.Count; i++) {
			//loop through difficulty
			for (int j = 0; j < numberOfOptions; j++) {
				//Let it know which Player it has
				options[count].playerID = players[i].id;
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

	public Option GetRandomAvailableOption() {
		Option temp;
		ShuffleList(options);
		List<Option> tempOptions = options.Where(n => (!n.correctlyChosen && !n.onScreen)).ToList();
		if (tempOptions.Count == 0) 
			temp = Option.GenerateEmptyOption();
		
		else 
			temp = tempOptions[UnityEngine.Random.Range(0, tempOptions.Count - 1)];
		
		return temp;
	}

	public Option GetRandomCorrectAvailableOption() {
		Option temp;
		ShuffleList(options);
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
			ShuffleList(tempList);
			temp = tempList[0];
		}
		return temp;
	}

	public int GetNumberAvailableOptionsForPlayer(int playerID) {
		return options.Where(n => (!n.correctlyChosen && !n.onScreen && n.playerID == playerID)).ToList().Count;
	}

	public int GetRandomUnfinishedPlayer() {
		int playerIndex = UnityEngine.Random.Range(0, players.Count);
		int count = GetNumberAvailableOptionsForPlayer(playerIndex);

		return (count > 0) ? playerIndex : Option.DEFAULT_INDEX;
	}

	public List<Option>GetXAvailableOptions(int x) {
		List<Option> returnList = new List<Option>();
		ShuffleList(options);
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

	public string GetPlayerName(int index) {
		if (index < players.Count) 
			return players[index].name;

		else {
			Debug.Log("index '" + index + "' out of bounds");
			return "";
		}
	}

	public Player GetPlayer(int id) {
		if (id < players.Count) {
			foreach (Player p in players) {
				if (p.id == id)
					return p;
			}
		}
		return null;
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

	void ShuffleList(List<Option> list) {
		for (int i = 0; i < list.Count; i++) {
			Option temp = list[i];
			int randomIndex = UnityEngine.Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}
}