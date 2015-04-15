using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour {
	public float fadeSpeed = 3f;

	private bool sceneStarting = false;

	void Awake() {
		sceneStarting = true;
		GetComponent<CanvasRenderer>().SetColor(Color.clear);
	}

	void Update() {
		if(sceneStarting) {
			StartScene();
		} else {
			EndScene();
		}
	}

	void FadeToClear() {
		GetComponent<CanvasRenderer>().SetColor(Color.Lerp(this.GetComponent<CanvasRenderer>().GetColor(), Color.clear, fadeSpeed * Time.deltaTime));
	}

	void FadeToBlack() {
		GetComponent<CanvasRenderer>().SetColor(Color.Lerp(this.GetComponent<CanvasRenderer>().GetColor(), Color.black, fadeSpeed * Time.deltaTime));
	}

	void StartScene() {
		FadeToBlack();

		if(GetComponent<CanvasRenderer>().GetColor().a > 0.95f) {
			GetComponent<CanvasRenderer>().SetColor(Color.black);
			float start = Time.time;
			while(Time.time-start>1.5f);
			sceneStarting = false;
		}
	}

	public void EndScene () {
		FadeToClear();
		if(GetComponent<CanvasRenderer>().GetColor().a < 0.01f) {
			Application.LoadLevel(1);
		}
	}
}