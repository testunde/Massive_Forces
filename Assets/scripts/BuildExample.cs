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

	// GameObject createPreview(){	//create cube for building preview
		// GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		// cube.layer=2;
		// cube.GetComponent<Renderer>().material=previewMat;
		// return cube;
	// }
	// GameObject createBuilding(Vector3 pos){	//create cube
		// GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		// cube.name="BuildingExample";
		// cube.transform.position=pos;
		// buildMat=cube.GetComponent<Renderer>().material;	//to able to reverse to the old material
		// return cube;
	// }
	
	void increaseBuildingArray(){
		Building[] tempBuildings=new Building[buildings.Length+1];
		for(int i=0;i<buildings.Length;i++){
			tempBuildings[i]=buildings[i];
		}
		buildings=tempBuildings;
	}
	void decreaseBuildingArray(){
		Building[] tempBuildings=new Building[buildings.Length-1];
		for(int i=0;i<tempBuildings.Length;i++){
			tempBuildings[i]=buildings[i];
		}
		buildings=tempBuildings;
	}
	
	void Start(){
		camCtrl=gameObject.GetComponent<CameraControl>();
		Map=GameObject.Find("Terrain");
	}
	
	void FixedUpdate(){
		switch(state){
			case 0:{
				if(Input.GetKey("f")){
					increaseBuildingArray();
					buildings[buildings.Length-1]=(new GameObject()).AddComponent<Building>();
					currentBuild=buildings[buildings.Length-1];
					currentBuild.Init(camCtrl);
					currentBuild.createPreview(previewMat);
					state=1;
				}
				break;
			}case 1:{
				currentBuild.setPreview(coords);
				if(Input.GetButton("Fire1")){
					currentBuild.createBuilding(coords);
					state=2;
				}else if(Input.GetButton("Cancel")||Input.GetButton("Fire2")){
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
	
	void Update(){
		coords=camCtrl.Pointer;
	}
}
