using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CHARACTERS {
	Boris = 1,
	Fashionista = 2,
	RighteousShadow = 3
}


public class CharacterManager {
	public static string GetCharacterName(int index)
	{
		switch (index) {
			case 0:
				return "Generic Name, Don't use this";
			case 1:
				return "Boris";
			case 2:
				return "Fashionista";
			case 3:
				return "Righteous Shadow";
			default:
				return "How did you get here?";
		}
	}

	public static Texture2D GetCharacterLogo(int index) {
		Texture2D temp = null;
		return temp;
	}

	public static string GetCharacterFilepath(int index) {
		switch (index) {
			case 0:
				return "Words";
			case 1:
				return "Boris";
			case 2:
				return "Fashionista";
			case 3:
				return "RighteousShadow";
			default: {
				Debug.Log("You've selected an invalid Character for Filepath");
				return "Not sure what you did here";
			}
			
		}
	}
	public static string GetCharacterNegativeFiePath(int index) {
		switch (index) {
			case 0:
				return "BadBoris";
			case 1:
				return "BadFashionista";
			case 2:
				return "BadRighteousShadow";
			default:
				return "";
		}
	}
}
}
