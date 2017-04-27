using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour {

	[SerializeField] private float objectSpeed = 1;
	[SerializeField] protected float resetPosition = -13f;
	[SerializeField] protected float startPosition = 178.3f;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		

		if(GameManger.instance.GameOver != true && GameManger.instance.PlayerActive) {
			transform.Translate(Vector3.back * (objectSpeed * Time.deltaTime));

			if(transform.localPosition.z <= resetPosition) {
				Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, startPosition);
				transform.position = newPosition;
				Destroy (GameObject.Find ("firstPlat"));
			}
		}


	}
}
 