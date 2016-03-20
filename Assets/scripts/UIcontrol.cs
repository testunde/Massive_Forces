using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts;

public class UIcontrol : MonoBehaviour {

	public Canvas buildingMenu;
	public Button building1;
	private ObjectControl objScript;
	
	// Use this for initialization
	void Start () 
	{
		buildingMenu = buildingMenu.GetComponent<Canvas> ();
		building1 = building1.GetComponent<Button> ();
		objScript = GameObject.Find("MainCamera").GetComponent<ObjectControl>();
	}
	
	public void building1_click ()
	{
		objScript.startBuild("IOb_testBuilding");
	}
	public void unit1_click ()
	{
		objScript.placeUnit("IOu_testUnit");		
	}
}
