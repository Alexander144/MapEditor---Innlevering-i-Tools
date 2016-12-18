using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour {

		public Vector2 currentPosition;
		public GameObject Tile;

	IEnumerator Start() {
		LoadMapEditorData.LoadMapEditorData obj = new LoadMapEditorData.LoadMapEditorData();
		obj.GetDataFromSQL();
		Tile.transform.position = new Vector3(-1,-1,-1);

		float x = 0.319f;
		float y = 0.319f;
		int tempx = 0;
		int tempy = 0;
		Vector3[] position = new Vector3[3];
		position[0] = new Vector3(0,0,0);
		position[1] = new Vector3(100, 0, 0);
		position[2] = new Vector3(0, 100, 0);

		for (int i = 0; i < obj.Id.Count; i++)
		{

			WWW www = new WWW(obj.Path[i]);
			yield return www;
			string imageExsist = www.error;
			if (imageExsist == null)
			{
				Tile.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.0f, 0.0f));
			}
			else
			{
				char[] image = obj.Path[i].ToCharArray();
				
				Array.Reverse(image);
				List<char> finalImage = new List<char>();
				//AddCharToList is there for to drop .png and .jpg, beacuse unity do not use it
				bool AddCharToList = false;
				for (int j = 0; j < image.Length; j++)
				{
					if (image[j].Equals('/'))
					{
						break;
					}
					
					if (AddCharToList == true) {
						finalImage.Add(image[j]);
					}
					if (image[j].Equals('.'))
					{
						AddCharToList = true;
					}
				}

				finalImage.Reverse();
				string finalImageToString = "";
				foreach (char t in finalImage)
				{
					finalImageToString += t;
				}

				Tile.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + finalImageToString);
			}
		
				
			if (obj.PositionX[i] != tempx)
			{
				x += 0.319f;
			}
			
			if (obj.PositionY[i] != tempy)
			{
				y += 0.319f;
			}
			

			currentPosition = new Vector2(y,x);
			tempx = obj.PositionX[i];
			tempy = obj.PositionY[i];

			Instantiate(Tile, currentPosition, new Quaternion());
			if (obj.PositionY[i] == 9)
			{
				y = 0;
			}
		}
	}
}
