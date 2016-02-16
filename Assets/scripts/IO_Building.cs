using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IO_Building : IngameObject {
		
		public IO_Building() : base(){
		}
		
		public virtual void setPreview(){
			//disable all non-preview objects
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name!="preview")
					child.SetActive(false);
			}
		}
		
		public virtual void abortBuild(){
			deleteModel();
		}
		
		public virtual void build(int frac){
			//disable preview; enable all building objects
			//or: invert all enables
			fraction=frac;
			timeRemaining=buildTime;
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				child.SetActive(!child.activeSelf);
			}
		}
	}
}
