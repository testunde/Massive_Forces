using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts;

public class UIcontrol : MonoBehaviour {

	public Canvas buildingMenu;
	public Button building1;
	private BuildMain BuildingScript;
	
	// Use this for initialization
	void Start () 
	{
		buildingMenu = buildingMenu.GetComponent<Canvas> ();
		building1 = building1.GetComponent<Button> ();
		BuildingScript = GameObject.Find("Main Camera").GetComponent<BuildMain>();
	}
	
	public void building1_click ()
	{
		BuildingScript.startBuild(1);
	}
}
