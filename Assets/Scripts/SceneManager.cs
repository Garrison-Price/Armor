using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	string previousScene = "MainMenu";
	private static SceneManager _sm;

	void Awake() {
		if(_sm == null) {
			//First instance of the scene manager so make this the only one
			_sm = this;
			//Make the scene manager persist in all scenes
			DontDestroyOnLoad(_sm.gameObject);
		} else {
			//Not the first instance
			if(_sm != this) //I am not the scene manager, destroy me
				Destroy(this.gameObject);
		}
	}

	//Button clicked to select and play a level
	public void SelectLevelButtonClicked(int level) {
		Application.LoadLevel(string.Concat("Level",level,"Scene"));
	}

	//Button clicked to go to the level selection menu
	public void LevelSelectButtonClicked() {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("LevelSelectScene");
	}

	//Button clicked to open the options menu
	public void OptionsButtonClicked() {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("OptionsScene");
	}

	//Button clicked to view the credits
	public void CreditsButtonClicked() {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("CreditsScene");
	}

	//Button clicked to quit the game.
	public void QuitButtonClicked() {
		Application.Quit();
	}
}
