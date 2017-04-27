using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour {
	
	public static GameManger instance = null;


	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private Text scoreTxt;
	[SerializeField] private Text scoreTxtOnScreen;
	public coinSpawn cs;

	private bool playerActive = false;
	private bool gameOver = false;
	private bool gameStarted = false;
	private int score = 0;

	public bool GameStarted {
		get {
			return gameStarted;
		}
	}

	public bool PlayerActive {
		get {
			return playerActive;
		}
	}

	public bool GameOver {
		get {
			return gameOver;
		}
	}

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
//		DontDestroyOnLoad (gameObject);
	
		Assert.IsNotNull (mainMenu);
		Assert.IsNotNull (gameOverScreen);
		Assert.IsNotNull (scoreTxt);
	}

	// Use this for initialization
	void Start () {
		scoreTxtOnScreen.text = "";
		cs = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<coinSpawn> ();
	}

	IEnumerator passTime() {
		
		yield return new WaitForSeconds(5);




	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayerCollided() {

		gameOver = true;
	}

	public void PlayerStartGame() {
		playerActive = true;


	}


	public void EnterGame() {
		
		mainMenu.SetActive (false);
		scoreTxtOnScreen.text = "Score: " + score;
		gameStarted = true;

	}

	public void scorePoint() {
		score++;
		scoreTxtOnScreen.text = "Score: " + score;

	}

	public void GameEnd() {
		gameOverScreen.SetActive (true);
		scoreTxtOnScreen.text = "";
		scoreTxt.text = "Score: " + score;


	
	}


	public void Restart() {
		
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

}