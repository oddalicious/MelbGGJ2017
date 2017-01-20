using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoopManager : MonoBehaviour {

	// Scene objects

	public Canvas passDeviceCanvas;
	public Text playerActionText;


	// Private variables

	private int playersTurn;


	// Common methods

	void Start () {
		playersTurn = 0;
		setPlayerActionText();
	}


	// Private methods

	private void setPlayerActionText() {
		playerActionText.text = string.Format("Pass device to {0}", GameManager.Get().GetPlayer(playersTurn));
	}


	//
}
