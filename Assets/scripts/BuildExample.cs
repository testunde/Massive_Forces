using UnityEngine;
using System.Collections;
using Scripts;

public class BuildExample : MonoBehaviour {
	private Vector3 coords;
	public CameraControl camCtrl;
	public GameObject Map;
	public Material previewMat;
	private Building[] buildings=new Building[0];	//so it's easier to get all building references
	private Building currentBuild;
	private int state=0;
	
	void increaseBuildingArray(){
		Building[] tempBuildings=new Building[buildings.Length+1];
		for(int i=0;i<buildings.Length;i++)
			tempBuildings[i]=buildings[i];
		buildings=tempBuildings;
	}
	void decreaseBuildingArray(){
		Building[] tempBuildings=new Building[buildings.Length-1];
		for(int i=0;i<tempBuildings.Length;i++)
			tempBuildings[i]=buildings[i];
		buildings=tempBuildings;
	}
	
	void Start(){
		camCtrl=gameObject.GetComponent<CameraControl>();
		Map=GameObject.Find("Terrain");
	}
	
	void Update(){
		coords=camCtrl.Pointer;
		switch(state){
			case 0:{
				//start building process
				if(Input.GetKeyDown("f")){
					state=1;
					goto case 1;
				}
				break;
			}case 1:{	//seperat case, so its possible to repeat this state immediately
				increaseBuildingArray();
				buildings[buildings.Length-1]=(new GameObject()).AddComponent<Building>();
				currentBuild=buildings[buildings.Length-1];
				currentBuild.Init(camCtrl);
				currentBuild.createPreview(previewMat);
				currentBuild.setCoords(coords);
				state=2;
				break;
			}case 2:{
				if(Input.GetButton("Fire2"))
					currentBuild.rotateTo(coords);
				else
					currentBuild.setCoords(coords);
				
				if(Input.GetButtonDown("Fire1")){
					currentBuild.createBuilding();
					//repeat if shift is hold while clicked
					if(Input.GetKey(KeyCode.LeftShift))
						state=1;
					else
						state=3;
				}else if(Input.GetButtonDown("Cancel")){
					//abort building process when pressed esc or mouse2
					Destroy(currentBuild.getParent());
					decreaseBuildingArray();
					state=0;
				}
				break;
			}default:{
				state=0;
				break;
			}
		}
	}
}
