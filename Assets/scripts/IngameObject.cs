using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IngameObject {
		protected static int resAmount=Res.getInstance().a;
		protected static int IDflow=1;
		public int ID;
		public string name;
		public string type;
		public GameObject model;
		public int HP,maxHP;
		public float buildTime,timeRemaining;
		public int fraction;
		public double[] costs=new double[resAmount];
		public ActionMatrix actions=new ActionMatrix();
		protected MinimapProjection minimap=new MinimapProjection();
		public SelectReact selectReact;
		
		public IngameObject(){
			ID=IDflow;
			IDflow++;
		}
		
		public virtual void initAfterModel(){
			selectReact=model.AddComponent<SelectReact>();
			selectReact.connectedObject=this;
		}
		
		public virtual void setCoords(Vector3 coords){
			model.transform.position=coords;
		}
		
		public virtual void rotateTo(Vector3 coords){
			Vector3 targetRot=new Vector3(coords.x,model.transform.position.y,coords.z);
			model.transform.LookAt(targetRot,Vector3.up);
		}
		
		public virtual void deleteModel(){
			MonoBehaviour.Destroy(model);
		}
	}
}
