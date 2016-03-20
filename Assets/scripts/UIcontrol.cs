using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts;

public class UIcontrol : MonoBehaviour {

	private Canvas buildingMenu;
	private Button building1,unit1;
	private ObjectControl objScript;
	
	// Use this for initialization
	void Start () 
	{
		buildingMenu = findUIElement("BuildingMenu",gameObject).GetComponent<Canvas> ();
		
		building1 = findUIElement("Building 1",buildingMenu.gameObject).GetComponent<Button> ();
		building1.onClick.AddListener(delegate { building1_click(); });
		
		unit1 = findUIElement("Unit 1",buildingMenu.gameObject).GetComponent<Button> ();
		unit1.onClick.AddListener(delegate { unit1_click(); });
		
		objScript = GameObject.Find("MainCamera").GetComponent<ObjectControl>();
	}
	
	void Update ()
	{
	
	}
	
	public void building1_click ()
	{
		objScript.startBuild("IOb_testBuilding");
	}
	public void unit1_click ()
	{
		objScript.placeUnit("IOu_testUnit");		
	}
	
	private GameObject findUIElement (string objName,GameObject parent)
	{
		GameObject element=null;
		for (int i=0;i<parent.transform.childCount;i++)
		{
			if(parent.transform.GetChild(i).name.Equals(objName))
			{
				element=parent.transform.GetChild(i).gameObject;
				break;
			}
		}
		return element;
	}
}