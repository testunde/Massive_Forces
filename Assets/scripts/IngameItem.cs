using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IngameItem {
		protected static FractionControl frCtrl=FractionControl.getInstance();
		public string name;
		public float buildTime=0,timeRemaining;
		public long[] costs=new long[frCtrl.RSC_a];
		public Action createdBy=null;
		//public icon=...
		
		public IngameItem(){
		}
	}
}
