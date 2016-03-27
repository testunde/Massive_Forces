using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Command : Action {
		
		public A_Command(IngameObject obj) : base(obj){
			this.name="A_Command";
		}
		
		public abstract override void begin();
		
		public abstract override void abort();
		
		public abstract override void finish();
	}
}
