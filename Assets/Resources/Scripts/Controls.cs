using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Controls : MonoBehaviour {
	public Image wasd;
	public Image arrowCtrl;

	private bool ctrl = false;
	private bool arrow = false;

	private bool move = false;

	void Awake() {
		arrowCtrl.color = Color.clear;
	}
	
	void FixedUpdate () {
		if(Mathf.Abs(Input.GetAxis ("HorizontalTankBase")) > 0 || Mathf.Abs(Input.GetAxis ("VerticalTankBase")) > 0) {
			wasd.color = Color.clear;
			arrowCtrl.color = Color.white;
			move = true;
		} else if(Mathf.Abs (Input.GetAxis ("HorizontalTankTurret")) > 0 && move) {
			if(ctrl) 
				arrowCtrl.color = Color.clear;
			arrow = true;
		} else if(Input.GetButton("FireTankShell") && move) {
			if(arrow) 
				arrowCtrl.color = Color.clear;
			ctrl = true;
		}

		if(move && ctrl && arrow)
			Destroy(this.gameObject);
	}
}
