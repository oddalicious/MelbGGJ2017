using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {

	public static string[] ReadLinesFromFile(string fileName, char splitOperator) {
		string[] lines = { "" };
		try {
			TextAsset textAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
			if (textAsset) {
				lines = textAsset.text.Split(splitOperator);
			}
		}
		catch (Exception e) {
			Debug.Log(e.ToString());
		}
		return lines;
	}

	public static void Shuffle<T>(List<T> list) {
		for (int i = 0; i < list.Count; i++) {
			T temp = list[i];
			int randomIndex = UnityEngine.Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}

	public static string[] ReadLinesFromCharacterFileAndSplit(string filename, int character) {
		string[] lines = { "" };
		try {
			TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
			if (textAsset) {
				lines = textAsset.text.Split("\n"[0]);
				if (CharacterManager.MAX_CHARACTERS - 1 < lines.Length) {
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