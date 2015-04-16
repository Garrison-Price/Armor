using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LevelImageManager : MonoBehaviour {
	private Image[] levelImages;

	public Sprite notCompleted;
	public Sprite completed;

	void Awake () {
		Regex levelRegularExpression = new Regex("(Level)(\\d+)(Image)",RegexOptions.IgnoreCase|RegexOptions.Singleline);
		levelImages = Resources.FindObjectsOfTypeAll(typeof(Image)) as Image[];
		foreach(Image image in levelImages) {
			Match match = levelRegularExpression.Match(image.name);
			if(match.Success) {
				if(PlayerPrefs.GetInt(string.Concat("Level",match.Groups[2].ToString(),"Completed"),0) == 0)
					image.sprite = notCompleted;
				else
					image.sprite = completed;
			}
		}
	}
}
