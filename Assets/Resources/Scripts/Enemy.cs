using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private Animator animator;
	private bool notFound = true; //Havent seen the player yet

	private GameObject tank;

	private AudioClip scream;

	private bool dead = false;

	void Awake () {
		animator = GetComponent<Animator>();
		scream = Resources.Load("Sounds/Hl2_Rebel-Ragdoll485-573931361") as AudioClip;
	}

	void FixedUpdate () {
		if(notFound && !dead) {
			GetComponent<Rigidbody2D>().transform.Rotate(0,0,1f);
			RaycastHit2D hit = Physics2D.Raycast(((Vector2)GetComponent<Rigidbody2D>().transform.position) + ((Vector2)GetComponent<Rigidbody2D>().transform.up), (Vector2)GetComponent<Rigidbody2D>().transform.up,100f);
			if(hit.collider != null) {
				if(hit.collider.gameObject.name == "Tank") {
					tank = hit.collider.gameObject;
					notFound = false;
				}
			}
			animator.SetBool("Firing",false);
		} else if(!dead) {
			Vector2 LookAtPoint = new Vector2(tank.transform.position.x,tank.transform.position.y);
			float rotation = Mathf.Atan2(LookAtPoint.y-GetComponent<Rigidbody2D>().transform.position.y,LookAtPoint.x-GetComponent<Rigidbody2D>().transform.position.x);
			GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0,0,rotation*Mathf.Rad2Deg-90);
			animator.SetBool("Firing",true);
			//Spawn Bullets
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Shell") {
			animator.SetFloat("Health",0);
			GetComponent<Rigidbody2D>().simulated = false;
			dead = true;
		} else if(coll.gameObject.name == "Tank") {
			animator.SetFloat("Health",0);
			GetComponent<Rigidbody2D>().simulated = false;
			float volume = PlayerPrefs.GetFloat("SoundVolume");
			int mute = PlayerPrefs.GetInt("SoundMute");
			AudioSource.PlayClipAtPoint(scream,GetComponent<Rigidbody2D>().transform.position,volume*(mute^1));
			dead = true;
		}
	}
}
