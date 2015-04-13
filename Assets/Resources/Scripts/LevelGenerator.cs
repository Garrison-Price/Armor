using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class LevelGenerator {
	private Texture2D levelTemplate;
	public Texture2D generatedLevel;
	private GameObject[] mediumTrees;
	private GameObject[] smallTrees;
	private GameObject[] rocks;
	private GameObject ground;
	private GameObject path;
	private int currentLevel;
	private int levelSize;
	public bool doneGenerating;

	public LevelGenerator(int size) {
		doneGenerating = false;
		levelSize = size;

		UnloadAssets ();

		Regex levelRegularExpression = new Regex("(Level)(\\d+)(Scene)",RegexOptions.IgnoreCase|RegexOptions.Singleline);
		Match match = levelRegularExpression.Match(Application.loadedLevelName);

		if(match.Success) {
			currentLevel = System.Int32.Parse(match.Groups[2].ToString());
			LoadAssets();
			GenerateLevelImage();
			BuildScene();
		} else {
			currentLevel = -1;
		}
	}

	private void LoadAssets() {
		mediumTrees = Resources.LoadAll("Sprites/Scenery/MediumTrees").Cast<GameObject>().ToArray();
		smallTrees = Resources.LoadAll("Sprites/Scenery/SmallTrees").Cast<GameObject>().ToArray();
		rocks = Resources.LoadAll("Sprites/Scenery/Rocks").Cast<GameObject>().ToArray();
		ground = Resources.Load("Sprites/Scenery/Ground") as GameObject;
		path = Resources.Load("Sprites/Scenery/Path") as GameObject;
		levelTemplate = Resources.Load("Art/Levels/Level"+currentLevel+"Path") as Texture2D;
	}

	private void UnloadAssets() {
		mediumTrees = null;
		smallTrees = null;
		rocks = null;
		ground = null;
		path = null;
		Resources.UnloadUnusedAssets();
	}

	private void GenerateLevelImage() {
		generatedLevel = new Texture2D(levelSize, levelSize);
		float increment = .018f * 256 / levelSize; //I like this density at 256 pixels so lets just make the density the same for every size
		float xoff = 0.0f;
		Color[] pixels = new Color[levelSize * levelSize];
		for(int x = 0; x < levelSize; x++)
		{
			xoff += increment;
			float yoff = 0.0f;
			for(int y = 0; y < levelSize; y++)
			{
				yoff += increment;
				float noise = Mathf.PerlinNoise(xoff + currentLevel * 15,yoff + currentLevel * 15); //Need to offset for each level otherwise it would be the same noise
				noise = levelTemplate.GetPixel((int)Mathf.Floor(x * 32.0f / levelSize),(int)Mathf.Floor(y * 32.0f / levelSize)).grayscale - noise;
				noise = Mathf.Min(1.0f,noise);
				if(noise > .6f)			//This ended up looking good for a threshold
					noise = .85f;
				else if(noise <= .6f)
					noise = 0.0f;
				pixels[x + y * levelSize] = new Color(noise,noise,noise);
			}
		}
		generatedLevel.SetPixels(pixels);
		generatedLevel.Apply();
	}

	private void BuildScene() {

	}
}
