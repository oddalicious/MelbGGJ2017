using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CHARACTERS {
	Boris = 1,
	Fashionista = 2,
	//RighteousShadow = 3,
	//BearTrap = 3,
}

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
		string characterData = ReadLineFromCharacterStory(STORY_LIST_FILE, index);
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
				sprite = Resources.Load("images/BorisLogo", typeof(Sprite)) as Sprite;
				
				break;
			case 2:
				sprite = Resources.Load("images/FashionistaLogo", typeof(Sprite)) as Sprite;
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
				tempSprite = Resources.Load("images/BorisOutcome"+ (outcome + 1), typeof(Sprite)) as Sprite;
				break;
			case 2:
				tempSprite = Resources.Load("images/FashionistaOutcome" + (outcome + 1), typeof(Sprite)) as Sprite;
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
		string[] outcomes = ReadLinesFromCharacterFileAndSplit(OUTCOME_LIST_FILE, character);
		if (outcomeIndex < outcomes.Length) {
			outcome = outcomes[outcomeIndex];
		}
		return outcome;
	}

	private static string ReadLineFromCharacterStory(string filename, int character) {
		string lines =  "" ;
		try {
			TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
			if (textAsset) {
				lines = textAsset.text.Split("\n"[0])[character-1];
			}
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}
		return lines;
	}

	private static string[] ReadLinesFromCharacterFileAndSplit(string filename, int character) {
		string[] lines = { "" };
		try {
			TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
			if (textAsset) {
				lines = textAsset.text.Split("\n"[0]);
				if (MAX_CHARACTERS - 1 < lines.Length) {
					lines = lines[character - 1].Split("|"[0]);
				}
			}
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}

		return lines;
	}
}
