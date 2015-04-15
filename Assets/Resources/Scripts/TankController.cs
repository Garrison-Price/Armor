using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	public static float tankRotationConstant = 25.0f;
	public static float tankSpeedConstant = 1.4f;
	public static float turretRotationConstant = 0.5f;
	void FixedUpdate () {
		float tankBaseRot = -1 * Input.GetAxis ("HorizontalTankBase");
		float tankBaseThrottle = -1 * Input.GetAxis ("VerticalTankBase");
		float tankTurretRot = -1 * Input.GetAxis ("HorizontalTankTurret");

		//Debug.Log("BaseRot: "+tankBaseRot+" - BaseThrottle: "+tankBaseThrottle+" - TurretRot: "+tankTurretRot);
		GetComponent<Rigidbody2D>().velocity = new Vector2(tankBaseThrottle * tankSpeedConstant * Mathf.Cos((GetComponent<Rigidbody2D>().rotation + 90) * Mathf.Deg2Rad),tankBaseThrottle * tankSpeedConstant * Mathf.Sin((GetComponent<Rigidbody2D>().rotation + 90) * Mathf.Deg2Rad));
		GetComponent<Rigidbody2D>().angularVelocity = tankBaseRot * tankRotationConstant;

		transform.Find("TankTurret").transform.Rotate(0,0,tankTurretRot * turretRotationConstant);

		Vector3 newCameraPosition = new Vector3(transform.position.x,transform.position.y,Camera.main.transform.position.z);

		Vector2 screeBottomLeftWorldPosition = new Vector2(newCameraPosition.x, newCameraPosition.y) - new Vector2(Camera.main.GetComponent<Camera>().orthographicSize * Screen.width/Screen.height, Camera.main.GetComponent<Camera>().orthographicSize);
		Vector2 screeTopRightWorldPosition = new Vector2(newCameraPosition.x, newCameraPosition.y) + new Vector2(Camera.main.GetComponent<Camera>().orthographicSize * Screen.width/Screen.height, Camera.main.GetComponent<Camera>().orthographicSize);

		if(screeBottomLeftWorldPosition.x <= 0) {
			newCameraPosition.x = Camera.main.GetComponent<Camera>().orthographicSize * Screen.width/Screen.height;
		} else if(screeTopRightWorldPosition.x >= 64) {
			newCameraPosition.x = 64 - Camera.main.GetComponent<Camera>().orthographicSize * Screen.width/Screen.height;
		}
		if(screeBottomLeftWorldPosition.y <= 0) {
			newCameraPosition.y = Camera.main.GetComponent<Camera>().orthographicSize;
		} else if(screeTopRightWorldPosition.y >= 64) {
			newCameraPosition.y = 64 - Camera.main.GetComponent<Camera>().orthographicSize;
		} 

		Camera.main.transform.position = newCameraPosition;
	}
}
