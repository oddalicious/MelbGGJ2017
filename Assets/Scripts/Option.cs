public class Option {
	public static int DEFAULT_INDEX = -99;
	public string text;
	public bool selected = false;
	public int playerID = DEFAULT_INDEX;

	public static Option GenerateEmptyOption() {
		Option temp = new Option();
		temp.text = "---";
		temp.selected = false;
		temp.playerID = DEFAULT_INDEX;
		return temp;
	}
}
