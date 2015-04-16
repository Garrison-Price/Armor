using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private Animator animator;
	private bool notFound = true; //Havent seen the player yet

	private GameObject tank;

	void Awake () {
		animator = GetComponent<Animator>();
	}

	void FixedUpdate () {
		if(notFound) {
			GetComponent<Rigidbody2D>().transform.Rotate(0,0,1f);
			RaycastHit2D hit = Physics2D.Raycast(((Vector2)GetComponent<Rigidbody2D>().transform.position) + ((Vector2)GetComponent<Rigidbody2D>().transform.up), Vector2.up,100f);
			if(hit.collider != null) {
				if(hit.collider.gameObject.name == "Tank") {
					tank = hit.collider.gameObject;
					notFound = false;
				}
			}
			animator.SetBool("Firing",false);
		} else {
			Vector2 LookAtPoint = new Vector2(tank.transform.position.x,tank.transform.position.y);
			float rotation = Mathf.Atan2(LookAtPoint.y-GetComponent<Rigidbody2D>().transform.position.y,LookAtPoint.x-GetComponent<Rigidbody2D>().transform.position.x);
			GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0,0,rotation*Mathf.Rad2Deg-90);
			animator.SetBool("Firing",true);
			//Spawn Bullets
		}
	}
}
