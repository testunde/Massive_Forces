using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IO_Unit : IngameObject {
		
		public IO_Unit() : base(){
		}
		
		public virtual void moveTo(Vector3 coords){
			//set velocity of rigidbody of model
			//later: a* algorithm
		}
		
		public virtual void createUnit(){
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="united" || child.name=="Marker"){
					child.SetActive(true);
				}else{
					child.SetActive(false);
				}
			}
		}
	}
}
