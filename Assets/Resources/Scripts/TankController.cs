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

		Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,Camera.main.transform.position.z);
	}
}
