using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IO_Unit : IngameObject {
		
		public IO_Unit() : base(){
			this.type="IO_Unit";
			standardActions[3,2]="Acom_SetMeetPoint";
		}
		
		public virtual void createUnit(){
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="united" || child.name=="Marker" || child.name=="collider"){
					child.SetActive(true);
				}else{
					child.SetActive(false);
				}
			}
			HP=maxHP;
			Debug.Log(type+" with ID "+ID+" finished!");
			actionBeh.enabled=true;
		}
		
		public virtual bool moveTo(Vector3 coords){
			rotateTo(coords);
			if(Vector3.Distance(coords,model.transform.position)>.5f){
				float distance=maxSpeed*inputMod.originalFixedDeltaTime;
				Vector3 oldPos=model.transform.position;
				model.transform.Translate(0f,0f,distance);	//forwards
				//get new terrain height
				Vector3 tooFarPos=inputMod.getCoordsAtXZ(model.transform.position);
				//finally set position
				model.transform.position=inputMod.getCoordsAtXZ(oldPos+Vector3.ClampMagnitude(tooFarPos-oldPos,distance));
				return true;
			}
			return false;
		}
	}
}
