using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public enum gameStatus {
	next, play, gameover, win
}

public class GameManager : Singleton<GameManager> { // GameManger class uses generic singleton class
	
	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl; 
	[SerializeField]
	private Text totalEscapedLbl;
	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private Enemy[] enemies;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private Text playBtnLbl;
	[SerializeField]
	private Button playBtn;
	[SerializeField]
	private Image winBanner;

	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int enemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;
	private AudioSource audioSource;

	public bool gameStarted = false;

	public List<Enemy> EnemyList = new List<Enemy>();

	const float spawnDelay = 0.75f;

	public int TotalEscaped {
		get {
			return totalEscaped;
		}
		set {
			totalEscaped = value;
		}
	}

	public int RoundEscaped {
		get {
			return roundEscaped;
		}
		set {
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}
		set {
			totalKilled = value;
		}
	}

	public int TotalMoney {
		get {
			return totalMoney;
		}
		set {
			totalMoney = value;
			totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	public AudioSource AudioSource {
		get {
			return audioSource;
		}
	}


	// Use this for initialization
	void Start () {
		playBtn.gameObject.SetActive(false);
		winBanner.gameObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		showMenu();
	}

	void Update() {
		handleEscape();
	}



	IEnumerator spawn() {
		// int spawnNum = 0;
		// if(waveNumber <= 4) {
		// 	spawnNum = 1;
		// } else if(waveNumber <=7) {
		// 	spawnNum = 2;
		// } else {
		// 	spawnNum = 3;
		// }
		if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
			for(int i = 0; i < enemiesPerSpawn; i++) {
				if(EnemyList.Count < totalEnemies) {
					Enemy newEnemy = Instantiate(enemies[(int)(Random.Range(0, (enemiesToSpawn + 1)))]) as Enemy;
					newEnemy.transform.position = spawnPoint.transform.position;
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
		
	}


	public void RegisterEnemy(Enemy enemy) {
		EnemyList.Add(enemy);
	}

	public void UnregisterEnemy(Enemy enemy) {
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void DestroyAllEnemies() {
		foreach(Enemy enemy in EnemyList) {
			Destroy(enemy.gameObject);
		}

		EnemyList.Clear();
	}

	public void addMoney(int amount) {
		TotalMoney += amount;
	}

	public void subtractMoney(int amount) {
		TotalMoney -= amount;
	}

	public void isWaveOver() {
		totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + "5";
		if((RoundEscaped + TotalKilled) == totalEnemies) {
			if (waveNumber <= enemies.Length && enemiesToSpawn != 2) {
				enemiesToSpawn = waveNumber;
			}
			setCurrentGameState();
			showMenu();
		}
	}

	public void setCurrentGameState() {
		if(TotalEscaped >= 5) {
			currentState = gameStatus.gameover;
			gameStarted = false;
			showMenu();
		} else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0) {
			currentState = gameStatus.play;
			gameStarted = true;
		} else if (waveNumber + 1 >= totalWaves) {
			currentState = gameStatus.win;
			gameStarted = false;
		} else {
			currentState = gameStatus.next;
			gameStarted = true;
		}
	}

	public void showMenu() {
		switch (currentState) {
			case gameStatus.gameover:
				playBtnLbl.text = "Play Again!";
				AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
				break;
			case gameStatus.next:
				playBtnLbl.text = "Next Wave";
			
				break;
			case gameStatus.play:
				playBtnLbl.text = "Play";
				break;
			case gameStatus.win:
				playBtnLbl.text = "Play";
				winBanner.gameObject.SetActive(true);
				break;
		}
		playBtn.gameObject.SetActive(true);
	}

	public void playBtnPressed() {
		switch(currentState) {
			case gameStatus.next:
				waveNumber += 1;
				totalEnemies += waveNumber;
				totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + "5";
				gameStarted = true;
				break;
			default:
				gameStarted = true;
				totalEnemies = 3;
				TotalEscaped = 0;
				enemiesToSpawn = 0;
				TotalMoney = 10;
				TowerManager.Instance.DestroyAllTowers();
				TowerManager.Instance.RenameTagsBuildSites();
				// enemiesToSpawn = 0;
				waveNumber = 0;
				totalMoneyLbl.text = TotalMoney.ToString();
				totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + "5";
				winBanner.gameObject.SetActive(false);
				audioSource.PlayOneShot(SoundManager.Instance.NewGame);
				break;
		}
		DestroyAllEnemies();
		TotalKilled = 0;
		RoundEscaped = 0;
		if(waveNumber + 1 == totalWaves)
			currentWaveLbl.text = "Wave " + (waveNumber + 1) + " - Final Wave!";
		else
			currentWaveLbl.text = "Wave " + (waveNumber + 1);
		StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
	}

	private void handleEscape() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerBtnPressed = null;
		}
	}


}
