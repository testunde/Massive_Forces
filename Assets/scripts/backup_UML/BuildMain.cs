using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts_o;

#pragma warning disable 0414 //"private field ... assigned but not used."

namespace Scripts_o {
	public class BuildMain : MonoBehaviour {
		private Vector3 coords;
		private CameraControl camCtrl;
		private GameObject Map;
		private Material previewMat;
		private List<Building> buildings=new List<Building>();	//all currently builded and alive buildings
		private Building currentBuild;
		private int targetBuilding=0;
		private State state=new State();
		
		public void startBuild(int nr){
			if(state.Equal(0))
				targetBuilding=nr;
		}
		public List<Building> getAliveBuildings(){	//return copy of buiildings list
			return new List<Building>(buildings);
		}
		
		void Start(){
			camCtrl=gameObject.GetComponent<CameraControl>();
			Map=GameObject.Find("Terrain");
			previewMat = (Material)Resources.Load("materials/buildingPreview", typeof(Material));
		}
		
		void Update(){
			coords=camCtrl.Pointer;
			//ATTENTION:	Make sure the switchcase leaves the last update routine after
			//				it has finishes NOT with 0. Use an code state instead!
			if(Input.GetKeyDown("f")&&state.Equal(0))
				startBuild(1);
			
			switch(state.Get()){
				case 0:{
					//start building process if building number is set
					if(targetBuilding>0){
						state.Set(1);
						goto case 1;
					}else{
						targetBuilding=0;
					}
					break;
				}case 1:{	//seperat case, so its possible to repeat this state immediately
					currentBuild=(new GameObject()).AddComponent<Building>();
					buildings.Add(currentBuild);
					currentBuild.Init(camCtrl,state,1);	//fraction ID 1 as player fraction
					currentBuild.createPreview(previewMat);
					state.Set(2);
					//set here the coords, so if shift and right-click is hold down it spawns at the mouse pointer
					currentBuild.setCoords(coords);
					break;
				}case 2:{
					//rotate if right click, else just follow the mouse pointer
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
						buildings.Remove(currentBuild);
						state.Set(99);	//set state to 99 as cancel code
					}
					break;
				}default:{	//mostly for state reset
					targetBuilding=0;
					state.Set(0);
					break;
				}
			}
		}
	}
}
