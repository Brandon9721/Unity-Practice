using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinHide : MonoBehaviour {
	
	void OnEnable() {
		Invoke ("Destroy", 9f);
	}

	void Destroy() {
		
			gameObject.SetActive (false);

	}

	void OnDisable() {
		CancelInvoke ();
	}
}
