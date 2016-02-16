using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts;

public class UIcontrol : MonoBehaviour {

	public Canvas buildingMenu;
	public Button building1;
	private ObjectControl ObjScript;
	
	// Use this for initialization
	void Start () 
	{
		buildingMenu = buildingMenu.GetComponent<Canvas> ();
		building1 = building1.GetComponent<Button> ();
		ObjScript = GameObject.Find("Main Camera").GetComponent<ObjectControl>();
	}
	
	public void building1_click ()
	{
		ObjScript.startBuild("IOb_testBuilding");
	}
}
