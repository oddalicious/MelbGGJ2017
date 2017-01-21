using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public string name;
	public int id;
	public int difficulty = 3;
	public int character = 0;

	public Player(int id, string name) {
		this.id = id;
		this.name = name;
	}

	public Player(int id, string name, int difficulty) {
		this.id = id;
		this.name = name;
		this.difficulty = difficulty;
	}
}
