using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IO_Unit : IngameObject {
		
		public IO_Unit() : base(){
			standardActions[3,2]="Acom_Move";
		}
		
		public virtual void createUnit(int frac){
			fraction=frac;
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="united" || child.name=="Marker" || child.name=="collider"){
					child.SetActive(true);
				}else{
					child.SetActive(false);
				}
			}
			Debug.Log(type+" with ID "+ID+" finished!");
			actionBeh.enabled=true;
		}
		
		public virtual void moveTo(Vector3 coords){
			//set velocity of rigidbody of model
			//later: a* algorithm
		}
		
		public override void setTargetPos(Vector3 coords){
			base.setTargetPos(coords);
			//SET A-STAR ROUTE!
		}
	}
}
