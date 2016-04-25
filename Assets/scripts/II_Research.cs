using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class II_Research : IngameItem {
		
		public II_Research() : base(){
			this.type="II_Research";
			this.name="II_Research";
		}
		
		//set modifiers/factors
		public abstract void perform(int frac);
		
		public abstract bool isDone();
	}
}
