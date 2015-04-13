using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
	private List<GameObject> placedObjects;

	private static float MEDIUM_TREE_RANGE = 0.60f;
	private static float SMALL_TREE_RANGE = 0.85f;

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
		placedObjects = new List<GameObject>();
	}

	private void UnloadAssets() {
		mediumTrees = null;
		smallTrees = null;
		rocks = null;
		ground = null;
		path = null;
		placedObjects = null;
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
				//noise = levelTemplate.GetPixel((int)Mathf.Floor(x * 32.0f / levelSize),(int)Mathf.Floor(y * 32.0f / levelSize)).grayscale - noise;
				//noise = Mathf.Min(1.0f,noise);
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
		for(float x = 0.0f; x < 4; x+=1.68f) {
			for(float y = 0.0f; y < levelSize; y+=1.68f) {
				//Place a ground tile, grass if not a path, path if it is.
				//Start object placement
				PlaceObjects(x,y);
			}
		}
	}

	private void PlaceObjects(float x, float y) {
		//Check to see if this is a spot we can place items at
		int xpixel = (int)(x/1.68f); //x mapped to our generated level texture
		int ypixel = (int)(y/1.68f); //y mapped to the generated level texture

		if(generatedLevel.GetPixel(xpixel,ypixel).grayscale < .5f && levelTemplate.GetPixel((int)Mathf.Floor(xpixel * 32.0f / levelSize),(int)Mathf.Floor(ypixel * 32.0f / levelSize)).grayscale < .5f) {
			//We can place objects here as it is not a path and it is a part of the forest
			float theta = 0.0f;
			GameObject newSceneryObject;
			GameObject previousObject = null;
			while(theta < 360) {
				//Randomly pick an object for use
				float r = Random.value;
				if(r < MEDIUM_TREE_RANGE)
					newSceneryObject = (GameObject) GameObject.Instantiate(mediumTrees[(int)(Random.value * mediumTrees.Length)]);
				else if(r > MEDIUM_TREE_RANGE && r < SMALL_TREE_RANGE) 
					newSceneryObject = (GameObject) GameObject.Instantiate(smallTrees[(int)(Random.value * smallTrees.Length)]);
				else
					newSceneryObject = (GameObject) GameObject.Instantiate(rocks[(int)(Random.value * rocks.Length)]);

				if(previousObject == null) {
					xpixel = (int)(newSceneryObject.transform.position.x/1.68f);
					ypixel = (int)(newSceneryObject.transform.position.y/1.68f);
					if(xpixel > 0 && xpixel < levelSize && ypixel > 0 && ypixel < levelSize && generatedLevel.GetPixel(xpixel,ypixel).grayscale < .5f && levelTemplate.GetPixel((int)Mathf.Floor(xpixel * 32.0f / levelSize),(int)Mathf.Floor(ypixel * 32.0f / levelSize)).grayscale < .5f) {
						foreach(GameObject go in placedObjects)
							if(newSceneryObject.GetComponent<CircleCollider2D>().IsTouching(go.GetComponent<CircleCollider2D>()))
								return;
					}
					else
						return;
					newSceneryObject.transform.position.Set(x,y,0);
					placedObjects.Add(newSceneryObject);
					previousObject = newSceneryObject;
				}
				else {
					bool placed = false;
					while(theta < 360 && !placed) {
						newSceneryObject.transform.position = new Vector3(previousObject.transform.position.x + Mathf.Cos(Mathf.Deg2Rad * theta) * (previousObject.GetComponent<CircleCollider2D>().radius + newSceneryObject.GetComponent<CircleCollider2D>().radius), previousObject.transform.position.y + Mathf.Sin(Mathf.Deg2Rad * theta) * (previousObject.GetComponent<CircleCollider2D>().radius + newSceneryObject.GetComponent<CircleCollider2D>().radius), 0);
						placed = true;
						xpixel = (int)(newSceneryObject.transform.position.x/1.68f);
						ypixel = (int)(newSceneryObject.transform.position.y/1.68f);
						if(xpixel > 0 && xpixel < levelSize && ypixel > 0 && ypixel < levelSize && generatedLevel.GetPixel(xpixel,ypixel).grayscale < .5f && levelTemplate.GetPixel((int)Mathf.Floor(xpixel * 32.0f / levelSize),(int)Mathf.Floor(ypixel * 32.0f / levelSize)).grayscale < .5f) {
							foreach(GameObject go in placedObjects)
								if(newSceneryObject.GetComponent<CircleCollider2D>().IsTouching(go.GetComponent<CircleCollider2D>()))
									placed = false;
						}
						else
							placed = false;
						theta+=5;
					}
					if(!placed)
						Object.Destroy(newSceneryObject);
					else {
						theta = 0;
						placedObjects.Add(newSceneryObject);
						previousObject = newSceneryObject;
					}
				}
			}
		}
	}
}
