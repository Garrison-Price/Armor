using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	string previousScene = "MainMenu";
	private static SceneManager _sm;
	private Dictionary<string, UnityEngine.Events.UnityAction<int>> functionMap;
	
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
		LoadDictionary();
		LoadButtons();
	}

	void OnLevelWasLoaded() {
		LoadButtons();
	}

	public void LoadDictionary() {
		if(_sm.functionMap == null) {
			//We haven't created and loaded the dictionary yet, so lets do that.
			functionMap = new Dictionary<string, UnityEngine.Events.UnityAction<int>>();
			functionMap.Add("LevelSelectButton",LevelSelectButtonClicked);
			functionMap.Add("OptionsButton",OptionsButtonClicked);
			functionMap.Add("CreditsButton",CreditsButtonClicked);
			functionMap.Add("QuitButton",QuitButtonClicked);
			functionMap.Add("BackButton",BackButtonClicked);
		}
	}

	//Initializes all scene changing buttons and their on click methods.
	public void LoadButtons() {
		UnityEngine.UI.Button[] buttons = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.UI.Button)) as UnityEngine.UI.Button[];
		foreach(UnityEngine.UI.Button button in buttons) {
			button.onClick.RemoveAllListeners();
			if(_sm.functionMap.ContainsKey(button.name))
				UnityEditor.Events.UnityEventTools.AddIntPersistentListener(button.onClick, _sm.functionMap[button.name], 1);
		}
	}

	//Input is not used here but is required for Dictionary
	public void BackButtonClicked(int ambiguous) {
		//Go Back to the previous scene... this may need some work when returning to a game scene.
		Application.LoadLevel(previousScene);
	}

	//Button clicked to select and play a level
	public void SelectLevelButtonClicked(int level) {
		Application.LoadLevel(string.Concat("Level",level,"Scene"));
	}

	//Button clicked to go to the level selection menu
	//Input is not used here but is required for Dictionary
	public void LevelSelectButtonClicked(int ambiguous) {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("LevelSelectScene");
	}

	//Button clicked to open the options menu
	//Input is not used here but is required for Dictionary
	public void OptionsButtonClicked(int ambiguous) {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("OptionsScene");
	}

	//Button clicked to view the credits
	//Input is not used here but is required for Dictionary
	public void CreditsButtonClicked(int ambiguous) {
		previousScene = Application.loadedLevelName;
		Application.LoadLevel("CreditsScene");
	}

	//Button clicked to quit the game.
	//Input is not used here but is required for Dictionary
	public void QuitButtonClicked(int ambiguous) {
		Application.Quit();
	}
}
