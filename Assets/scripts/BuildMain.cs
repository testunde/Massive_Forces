using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class BuildMain : MonoBehaviour {
		private Vector3 coords;
		private CameraControl camCtrl;
		private GameObject Map;
		private Material previewMat;
		public Building[] buildings=new Building[0];	//all currently builded and alive buildings
		private Building currentBuild;
		private int targetBuilding=0;
		private State state=new State();
		
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
		
		public void startBuild(int nr){
			targetBuilding=nr;
		}
		
		void Start(){
			camCtrl=gameObject.GetComponent<CameraControl>();
			Map=GameObject.Find("Terrain");
			previewMat = (Material)Resources.Load("materials/buildingPreview", typeof(Material));
		}
		
		void Update(){
			coords=camCtrl.Pointer;
			if(Input.GetKeyDown("f")
				startBuild(1);
			
			switch(state.Get()){
				case 0:{
					//start building process if building number is set
					if(targetBuilding>0)){
						state.Set(1);
						goto case 1;
					}
					break;
				}case 1:{	//seperat case, so its possible to repeat this state immediately
					increaseBuildingArray();
					buildings[buildings.Length-1]=(new GameObject()).AddComponent<Building>();
					currentBuild=buildings[buildings.Length-1];
					currentBuild.Init(camCtrl,state);
					currentBuild.createPreview(previewMat);
					currentBuild.setCoords(coords);
					state.Set(2);
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
							state.Set(1);
						else
							state.Set(55);	//set state to 55 as 'is builded' code
					}else if(Input.GetButtonDown("Cancel")){
						//abort building process when pressed esc or mouse2
						Destroy(currentBuild.getParent());
						decreaseBuildingArray();
						state.Set(99);	//set state to 99 as cancel code
					}
					break;
				}default:{	//mostly for state reset
					startBuild(0);
					state.Set(0);
					break;
				}
			}
		}
	}
}
