using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Research : Action {
		
		public A_Research(IngameObject obj) : base(obj){
			this.name="A_Research";
		}
		
		public abstract override void begin();
		
		public abstract override void abort();
		
		public abstract override void finish();
	}
}
