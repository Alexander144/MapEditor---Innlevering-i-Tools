using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadMapEditorData.LoadMapEditorData obj = new LoadMapEditorData.LoadMapEditorData();
		print("Hell");
		print("THis" + obj.GetDataFromSQL());
	}
	

}
