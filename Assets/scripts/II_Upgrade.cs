using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class II_Upgrade : IngameItem{
		public IO_Building obj;
		
		public II_Upgrade() : base(){
			this.name="II_Upgrade";
		}
		
		//set upgrade values
		public abstract void perform();
	}
}
