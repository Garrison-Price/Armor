using UnityEngine;
using System.Collections;

public class TankShell : MonoBehaviour {
	private bool fired = false;

	public AudioClip shellExplode;

	void Awake () {
		GetComponent<Rigidbody2D>().transform.Translate(-0.5f * GetComponent<Rigidbody2D>().transform.up,Space.World);
		GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,-2000));
		fired=true;
	}

	void FixedUpdate() {
		if(GetComponent<Rigidbody2D>().velocity.magnitude < 4 && fired)
			Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy")
			ExplodeShell ();
	}

	private void ExplodeShell() {
		float volume = PlayerPrefs.GetFloat("SoundVolume");
		int mute = PlayerPrefs.GetInt("SoundMute");
		AudioSource.PlayClipAtPoint(shellExplode,GetComponent<Rigidbody2D>().transform.position,volume*(mute^1));
		Destroy(this.gameObject);
	}
}
