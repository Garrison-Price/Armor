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

	private static float MEDIUM_TREE_RANGE = 0.40f;
	private static float SMALL_TREE_RANGE = 0.95f;

	public LevelGenerator(int size, int level) {
		doneGenerating = false;
		levelSize = size;

		UnloadAssets ();

		currentLevel = level;
		LoadAssets();
		GenerateLevelImage();
	}

	private void LoadAssets() {
		mediumTrees = Resources.LoadAll("Sprites/Scenery/MediumTrees").Cast<GameObject>().ToArray();
		smallTrees = Resources.LoadAll("Sprites/Scenery/SmallTrees").Cast<GameObject>().ToArray();
		rocks = Resources.LoadAll("Sprites/Scenery/Rocks").Cast<GameObject>().ToArray();
		ground = Resources.Load("Sprites/Ground") as GameObject;
		path = Resources.Load("Sprites/Path") as GameObject;
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
					noise = 1.0f;
				else if(noise <= .6f)
					noise = 0.0f;
				pixels[x + y * levelSize] = new Color(noise,noise,noise);
			}
		}
		generatedLevel.SetPixels(pixels);
		generatedLevel.Apply();
	}

	public void BuildScene() {
		GameObject newSceneryObject;
		GameObject groundObject;
		bool placed;

		for(int x = 0; x < levelSize+1; x++) {
			for(int y = 0; y < levelSize+1; y++) {
				if(levelTemplate.GetPixel((int)Mathf.Floor(x * 32.0f / levelSize),(int)Mathf.Floor(y * 32.0f / levelSize)).r > .5f) {
					//Grass
					groundObject = (GameObject) GameObject.Instantiate(ground);
				} else {
					//Path
					groundObject = (GameObject) GameObject.Instantiate(path);
				}
				groundObject.transform.position = new Vector3(x,y,0);
			}
		}
		int depth = 0;
		for(float x = 0.0f; x < levelSize; x+=.5f) {
			for(float y = levelSize; y >= 0.0f; y-=.5f) {
				depth++;
				//Place a ground tile, grass if not a path, path if it is.
				//Start object placement
				//PlaceObjects(x,y);
				int xpixel = (int)(x);///1.68f); //x mapped to our generated level texture
				int ypixel = (int)(y);///1.68f); //y mapped to the generated level texture
				if(generatedLevel.GetPixel(xpixel,ypixel).r < .5f && levelTemplate.GetPixel((int)Mathf.Floor(xpixel * 32.0f / levelSize),(int)Mathf.Floor(ypixel * 32.0f / levelSize)).r > .5f) {
					float r = Random.value;
					placed = true;
					if(r < MEDIUM_TREE_RANGE) {
						newSceneryObject = (GameObject) GameObject.Instantiate(mediumTrees[(int)(Random.value * mediumTrees.Length)]);
					}
					else if(r > MEDIUM_TREE_RANGE && r < SMALL_TREE_RANGE) {
						newSceneryObject = (GameObject) GameObject.Instantiate(smallTrees[(int)(Random.value * smallTrees.Length)]);
					}
					else {
						newSceneryObject = (GameObject) GameObject.Instantiate(rocks[(int)(Random.value * rocks.Length)]);
					}
					foreach(GameObject go in placedObjects) {
						if(Mathf.Pow(newSceneryObject.GetComponent<CircleCollider2D>().radius + go.GetComponent<CircleCollider2D>().radius,2) >= Mathf.Pow(x-go.transform.position.x,2)+Mathf.Pow(y-go.transform.position.y,2)) {
							placed = false;
							break;
						}
					}
					if(!placed) {
						Object.Destroy(newSceneryObject);
					}
					else {
						newSceneryObject.GetComponent<SpriteRenderer>().sortingOrder = depth;
						newSceneryObject.transform.position = new Vector3(x,y,0);
						placedObjects.Add(newSceneryObject);
					}
				}
			}
		}
	}
}
