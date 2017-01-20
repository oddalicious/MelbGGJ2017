using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupManager : MonoBehaviour {

	public Canvas playerNumbers;
	public Canvas playerConfig;

	public List<GameObject> nameFields;

	public void IncrementPlayers(Text numText)
	{
		GameManager.Get().IncrementPlayers(numText);
	}
	public void DecrementPlayers(Text numText) {
		GameManager.Get().DecrementPlayers(numText);
	}

	public void MoveToPlayerConfig() {
		int toRemove = 0;
		if (playerNumbers)
			playerNumbers.gameObject.SetActive(false);
		if (playerConfig)
			playerConfig.gameObject.SetActive(true);
		switch (GameManager.Get().NumPlayers) {

			case 4: {
					break;
				}

			case 3: {
					toRemove = 1;
					break;
				}
				
			case 2: {
					toRemove = 2;
					break;
				}
				
			case 1: {
					toRemove = 3;
					break;
				}
				
			default: {
					Debug.Log("Invalid number of players");
					toRemove = 0;
					break;
				}
		}

		for (int i = 0; i < toRemove; i++) {
			nameFields[nameFields.Count - 1 - i].SetActive(false);
		}

		GameManager.Get().LoadNextState();
	}

	public void MoveToStoryMode() {
		for (int i = 0; i < nameFields.Count; i++) {
			if (nameFields[i].activeSelf) {
				Text t = nameFields[i].GetComponentInChildren<Text>();
				if (t) {
					GameManager.Get().SetPlayer(i, t.text);
				}
			}
		}
		GameManager.Get().LoadNextState();
	}

	void Start() {
		if (playerNumbers == null)
			Debug.Log("NO PLAYERNUMBERS CANVAS DETECTED!");
		if (playerConfig == null)
			Debug.Log("NO PLAYERCONFIG CANVAS DETECTED!");
		if (nameFields.Count == 0) {
			Debug.Log("NO NAMEFIELD ENTRIES IN LIST");
		}
	}
}
