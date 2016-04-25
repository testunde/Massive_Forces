using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Upgrade : Action {
		
		public A_Upgrade() : base(){
			this.name="A_Upgrade";
		}
		
		public abstract override void begin();
		
		public abstract override void abort(IngameItem item);
		
		public abstract override void finish(IngameItem item);
	}
}
