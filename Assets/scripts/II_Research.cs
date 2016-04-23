using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class II_Research : IngameItem {
		public IO_Building obj;
		public float testUnitHP=0f;
		
		public II_Research() : base(){
			this.name="II_Research";
		}
		
		//set modifiers/factors
		public abstract void perform();
	}
}
