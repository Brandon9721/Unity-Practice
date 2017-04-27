using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ObjectMove {
	// reset z: 26  and start 57
	[SerializeField] Vector3 topPosition; // 
	[SerializeField] Vector3 bottomPosition; // 
	[SerializeField] float rockSpeed;




	// Use this for initialization
	public void Start () {
		StartCoroutine (Move (bottomPosition));
	}



	protected override void Update () {
				transform.Rotate (0, 45 * Time.deltaTime, 0);

	}
		

public IEnumerator Move(Vector3 target) {
		while(Mathf.Abs((target - transform.localPosition).y) > 1f) {
			Vector3 direction = target.y == topPosition.y ? Vector3.up : Vector3.down;
			transform.localPosition += direction * (rockSpeed * Time.deltaTime);

			yield return null;
		}			

		yield return new WaitForSeconds (Random.Range(0.1f, 1.5f));


		Vector3 newTarget = target.y == topPosition.y ? bottomPosition : topPosition;

		StartCoroutine (Move (newTarget));
	}
}
