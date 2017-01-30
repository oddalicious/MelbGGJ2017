using UnityEngine;

public class SplashScreenScript : MonoBehaviour {

	private float currentTime = 4.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentTime -= Time.deltaTime;
		if (currentTime <= 0) {
			GameManager.Get().LoadMainMenu();
		}
	}
}
