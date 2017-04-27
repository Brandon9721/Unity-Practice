using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {
	[SerializeField] private float jumpForce = 100f;
	[SerializeField] private AudioClip sfxJump;
	[SerializeField] private AudioClip sfxDeath;
	[SerializeField] private AudioClip Coin;

//	coinMove cm = new coinMove ();
	Animator anim;
	private Rigidbody rigidBody;
	private bool jump = false;
	private AudioSource audioSource;

	void Awake() {
//		Assert.IsNotNull (Coin);
		Assert.IsNotNull (sfxDeath);
		Assert.IsNotNull (sfxJump);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManger.instance.GameOver && GameManger.instance.GameStarted) {
			if(Input.GetMouseButtonDown (0) && rigidBody.position.y < - 1.1) {
				GameManger.instance.PlayerStartGame ();
				anim.Play ("Jump");
				audioSource.PlayOneShot (sfxJump);
				rigidBody.useGravity = true;
				jump = true;
			}
		}
	}

	void FixedUpdate () {
		if(jump == true) {
			jump = false;
			rigidBody.velocity = new Vector2 (0, 0);
			rigidBody.AddForce (new Vector2 (0, jumpForce), ForceMode.Impulse);
		}

	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "obstacle") {
			rigidBody.AddForce (new Vector3 (0, 0, -70), ForceMode.Impulse);
			rigidBody.detectCollisions = false;
			audioSource.PlayOneShot (sfxDeath);


			GameManger.instance.PlayerCollided ();
			GameManger.instance.GameEnd ();
		}
	}


	void OnTriggerEnter(Collider other) {
		other.gameObject.SetActive (false);
		audioSource.PlayOneShot (Coin);
		GameManger.instance.scorePoint ();
	}



}
