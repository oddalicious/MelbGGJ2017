using System.Collections.Generic;

public class Player {

	public const string DEFAULT_PLAYERNAME_DATABASE = "DefaultPlayerNames";
	public string name;
	public int id;
	public int difficulty = 3;

	public Player(int id, string name, int difficulty) {
		this.id = id;
		this.name = name;
		this.difficulty = difficulty;
	}

	public static List<string> GetDefaultPlayerNames() {
		List<string> output = new List<string>();
		string[] lines = Utilities.ReadLinesFromFile(DEFAULT_PLAYERNAME_DATABASE, '\n');
		foreach (string line in lines) {
			output.Add(line);
		}
		return output;
	}

}
