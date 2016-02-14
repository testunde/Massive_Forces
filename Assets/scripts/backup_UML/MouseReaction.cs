using UnityEngine;
using System.Collections;
using Scripts_o;

namespace Scripts_o {
	//sets material if mousehover or selected
	//must be imported as component in the to its refered object!
	public class MouseReaction : MonoBehaviour {
		private CameraControl camCtrl;
		private GameObject Map;
		private int state=0;	//0-nothing; 1-hover; 2-selected; 3-selected&hover
		private bool freshSelected=false;	//to make it possible to inverse selection
		private State buildControllerState;
		private Material buildMat,hoverMat,selectMat,selectHoverMat;
		private Material[] MatList;
		
		public void Init(CameraControl cC,State s){
			camCtrl=cC;
			buildControllerState=s;
		}
		
		void OnMouseOver(){
			if(state==0){
				state=1;
			}else if(state==2||state==4){
				state=3;
			}
		}
		void OnMouseExit(){
			freshSelected=false;
			if(state==1){
				state=0;
			}else if(state==3){
				state=2;
			}
		}
		void OnMouseDown(){
			if(state==1){
				state=3;
				freshSelected=true;
				//CALL SELECT COMMAND
			}
		}
		void OnMouseUpAsButton(){
			freshSelected=false;
		}
		
		void Start(){
			Map=GameObject.Find("Terrain");
			buildMat=gameObject.GetComponent<Renderer>().material;	//to able to reverse to the old material
			hoverMat = (Material)Resources.Load("materials/mouseHover", typeof(Material));
			selectMat = (Material)Resources.Load("materials/mouseSelected", typeof(Material));
			selectHoverMat = (Material)Resources.Load("materials/mouseSelectedHover", typeof(Material));
			MatList=new Material[] {buildMat,hoverMat,selectMat,selectHoverMat};
		}
		void Update(){
			//deselect condisitons
			if(camCtrl.leftClick&& (state==2&& !(camCtrl.shiftPress||camCtrl.ctrlPress) || camCtrl.ctrlPress&&state==3&& !freshSelected)){
				state=0;
				//CALL DESELECT COMMAND
			}
			
			//set material. make shure the material list fits with the states here
			if(buildControllerState.Equal(0))
				gameObject.GetComponent<Renderer>().material=MatList[state];
		}
	}
}
