using UnityEngine;

public class CharacterManager {
	public const int MAX_CHARACTERS = 2;
	private static string OUTCOME_LIST_FILE = "Outcomes";
	private static string STORY_LIST_FILE = "Stories";
	public static string GetCharacterName(int index)
	{
		switch (index) {
			case 0:
				return "Generic Name, Don't use this";
			case 1:
				return "Boris";
			case 2:
				return "Fashionista";
			//case 3:
			//	return "Righteous Shadow";
			//case 4:
			//	return "BearTrap";
			default:
				return "How did you get here?";
		}
	}

	public static string[] GetCharacterStory(int index) {
		string characterData = Utilities.ReadLinesFromFile(STORY_LIST_FILE, '\n')[index-1];
		string[] output = SplitIntoStory(characterData);
		return output;
	}

	private static string[] SplitIntoStory(string data) {
		string[] output = { "", "" };
		string[] tempOutput = data.Split('|');
		output[0] = tempOutput[0]; //Ensure the Title is set
		//loop through and ensure data is set
		for (int i = 1; i < tempOutput.Length; i++) {
			output[1] += tempOutput[i] + '\n';
		}

		return output;
	}

	public static Sprite GetCharacterLogo(int characterIndex) {
		Sprite sprite = new Sprite();
		switch (characterIndex) {
			case 0:
				Debug.Log("You've selected an invalid character for Filepath: " + characterIndex);
				break;
			case 1:
				sprite = Resources.Load("images/UI/BorisLogo", typeof(Sprite)) as Sprite;
				
				break;
			case 2:
				sprite = Resources.Load("images/UI/FashionistaLogo", typeof(Sprite)) as Sprite;
				break;
			default:

				break;
		}
		return sprite;
	}

	public static Sprite GetCharacterOutcomeImage(int characterIndex, int outcome) {
		Sprite tempSprite = new Sprite();
		switch (characterIndex) {
			case 0:
				Debug.Log("You've selected an invalid character for Filepath: " + characterIndex);
				break;
			case 1:
				tempSprite = Resources.Load("images/UI/BorisOutcome"+ (outcome + 1), typeof(Sprite)) as Sprite;
				break;
			case 2:
				tempSprite = Resources.Load("images/UI/FashionistaOutcome" + (outcome + 1), typeof(Sprite)) as Sprite;
				break;
			default:

				break;
		}
		return tempSprite;
	}

	public static string GetCharacterOptionsFilepath(int index) {
		switch (index) {
			case 0:
				return "General";
			case 1:
				return "Boris";
			case 2:
				return "Fashionista";
			//case 3:
			//	return "RighteousShadow";
			//case 4:
			//	return "Beartrap";
			default: {
				Debug.Log("You've selected an invalid character for Filepath: " + index);
				return "Not sure what you did here";
			}
			
		}
	}

	public static int GetRandomCharacter() {
		return UnityEngine.Random.Range(1, MAX_CHARACTERS + 1);
	}

	public static string GetCharacterOutcome(int character, int outcomeIndex) {
		string outcome = "INVALID OUTCOME";
		string[] outcomes = Utilities.ReadLinesFromCharacterFileAndSplit(OUTCOME_LIST_FILE, character);
		if (outcomeIndex < outcomes.Length) {
			outcome = outcomes[outcomeIndex];
		}
		return outcome;
	}

	public static string[] GetCharacterOptionStringsFromTextFile(int characterIndex) {
		return Utilities.ReadLinesFromFile(CharacterManager.GetCharacterOptionsFilepath(characterIndex), '\n');
	}
}
