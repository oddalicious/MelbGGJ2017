using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupManager : MonoBehaviour {

	public void IncrementPlayers(Text numText)
	{
		GameManager.Get().IncrementPlayers(numText);
	}
	public void DecrementPlayers(Text numText) {
		GameManager.Get().DecrementPlayers(numText);
	}

	public void MoveToPlayerConfig() {

	}
}
