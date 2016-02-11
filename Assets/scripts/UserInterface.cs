using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	private Rect Pos;
	
	// Use this for initialization
	void OnGUI () {
	//	GUI.Box ( Rect (0,0,100,100), "Text hier" );
		GUI.Box(new Rect( 20, 20, 200, 100), "Ein UnserInterface wird folgen");
	}
	
	void Update () {
		OnGUI();
	}
}
