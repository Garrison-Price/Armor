using UnityEngine;
using System.Collections;

public class WinCondition : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Tank") {
			Application.LoadLevel("WinScene");
		} 
	}
}
