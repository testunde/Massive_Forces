using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class Action {
		protected IngameObject obj=null;	//IngameObject which contains this Action
		protected Res resources;
		protected Produceline production=null;
		protected ObjectControl objCtrl;
		public string name;
		public float time=0f;
		public long[] costs;
		public bool disabled=false;
		
		public Action(){
			this.resources=Res.getInstance();
			this.costs=new long[]{0,0,0,0};
			this.objCtrl=GameObject.Find("MainCamera").GetComponent<ObjectControl>();
		}
		
		public virtual void setObject(IngameObject obj){
			this.obj=obj;
			if(obj is IO_Building)
				this.production=((IO_Building)obj).production;
		}
		
		//put Action in Produceline when time>0
		public abstract void begin();
		
		//e.g. create Unity objects
		public virtual bool create(IngameItem item){
			return false;
		}
		
		//e.g. turn back resources
		public abstract void abort(IngameItem item);
		
		//e.g. set modifiers or factors
		public abstract void finish(IngameItem item);
	}
}
