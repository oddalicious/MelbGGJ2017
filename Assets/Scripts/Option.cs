﻿using System.Collections.Generic;

public class Option {
	public static int DEFAULT_INDEX = -99;

	public int id;
	public string text;
	public bool correctlyChosen = false;
	public bool onScreen = false;
	public int playerID = DEFAULT_INDEX;
	public List<int> positiveCharacters;

	public static Option GenerateEmptyOption() {
		Option temp = new Option(0);
		temp.text = "---";
		return temp;
	}

	public static Option GenerateOption(int index, string text) {
		Option temp = new Option(index);
		temp.text = text;
		return temp;
	}

	public bool isPositiveToCharacter(int inputIndex) {
		foreach (int characterIndex in positiveCharacters) {
			if (characterIndex == inputIndex)
				return true;
		}
		return false;
	}

	public Option(int id) {
		this.id = id;
		playerID = DEFAULT_INDEX;
		correctlyChosen = false;
		onScreen = false;
		positiveCharacters = new List<int>();
	}

	//public override bool Equals(object obj) {
	//	// If paramater is null or false
	//	if (obj == null) 
	//		return false;
	//	
	//
	//	//If parameter cannot e cast to a Option return false
	//	Option o = obj as Option;
	//	if (o == null) 
	//		return false;
	//	
	//	return (o.id == this.id);
	//}
}
