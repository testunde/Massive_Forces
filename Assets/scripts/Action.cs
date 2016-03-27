using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class Action {
		protected IngameObject obj=null;	//IngameObject which contains this Action
		protected Res resources;
		protected Produceline production;
		public string name;
		public float time=0f;
		public bool disabled=false;
		//public icon=...
		
		public Action(IngameObject obj){
			this.obj=obj;
			this.resources=Res.getInstance();
			this.production=Produceline.getInstance();
		}
		
		//put Action in Produceline when time>0
		public abstract void begin();
		
		//e.g. turn back resources
		public abstract void abort();
		
		//e.g. set modifiers or factors
		public abstract void finish();
	}
}
