using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinSpawn : MonoBehaviour {

	public GameObject coin;
	private float randomTime;
	private float randomHeight;
	[SerializeField] private int pooledAmount = 15;
	public List<GameObject> coins;

//	public bool coinSpawnEnable;

	// Use this for initialization
	void Start () {
		coins = new List<GameObject>();



		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(coin, transform.position, Quaternion.Euler(180,180,0));

			obj.SetActive(false);
			coins.Add(obj);
		}
		StartCoroutine (passTime ());
	}

	// Update is called once per frame
	void Spawn () {
		for(int i = 0; i < coins.Count; i++) {
			if(!coins[i].activeInHierarchy && GameManger.instance.PlayerActive) {
				coins[i].transform.position = new Vector3(2,randomHeight,70);
				coins[i].transform.rotation = Quaternion.Euler(180,180,90);
				coins[i].SetActive (true);

				break;
			}
		}
	}

//	public void Despawn() {
//		coins.RemoveRange(0,15);
//	
//		}

	IEnumerator passTime() {
		randomTime = Random.Range (0.1f, 10f);
		randomHeight = Random.Range (-14f, 0f);
		if (randomTime >= 9) {
			yield return new WaitForSeconds (randomTime - 4);
			for (int i = 0; i < 10; i++) {
				Spawn ();
				yield return new WaitForSeconds (0.4f);
			}
		} else if (randomTime < 9 && randomTime > 7) {
			yield return new WaitForSeconds (randomTime - 3);
			for (int i = 0; i < 5; i++) {
				Spawn ();
				yield return new WaitForSeconds (0.4f);
			} 
		} else if (randomTime < 7 && randomTime > 4) {
			yield return new WaitForSeconds (randomTime - 2);
			for (int i = 0; i < 3; i++) {
				Spawn ();
				yield return new WaitForSeconds (0.5f);
			} 
		} else {
				yield return new WaitForSeconds(randomTime);
				Spawn();
			}

		StartCoroutine (passTime ());
	}
}
