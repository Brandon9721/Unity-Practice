using UnityEngine;

public class Enemy : MonoBehaviour {

	
	[SerializeField]
	private Transform exitPoint;
	[SerializeField]
	private Transform[] waypoints;
	[SerializeField]
	private float navigationUpdate;
	[SerializeField]
	private int healthPoints;
	[SerializeField]
	private int rewardAmt;

	private bool isDead = false;
	private int target = 0;
	private Transform enemy;
	private Collider2D enemyCollider;
	private Animator anim;
	private float navigationTime = 0;

	public bool IsDead {
		get {
			return isDead;
		}
	}

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
		enemyCollider = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();
		GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(waypoints != null && !isDead) {
			navigationTime += Time.deltaTime;
			if(navigationTime > navigationUpdate) {
				if(target < waypoints.Length) {
					enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
				} else {
					enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
				}
				navigationTime = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other != null || other.gameObject != null) {
			if(other.tag == "Checkpoint") {
				target += 1;
			}
			else if(other.tag == "Finish") {
				GameManager.Instance.RoundEscaped += 1;
				GameManager.Instance.TotalEscaped += 1;
				GameManager.Instance.UnregisterEnemy(this);
				GameManager.Instance.setCurrentGameState();
				GameManager.Instance.isWaveOver();
			} else if(other.tag == "Projectile") {
				Projectile newP = other.gameObject.GetComponent<Projectile>();
				if(newP != null) {
					enemyHit(newP.AttackStrength);
					Destroy(newP);
				}
					
					
				Destroy(other.gameObject);
				// Destroy(newP);

			}
		}
		
	}

	public void enemyHit(int hitpoints) {
		if(healthPoints - hitpoints > 0) {
			healthPoints -= hitpoints;
			anim.Play("Hurt");
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
			
		} else {
			die();
			anim.SetTrigger("didDie");
			
		}
	}

	public void die() {
		isDead = true;
		enemyCollider.enabled = false;
		GameManager.Instance.TotalKilled += 1;
		GameManager.Instance.addMoney(rewardAmt);
		GameManager.Instance.isWaveOver();
		GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);

	}
}
