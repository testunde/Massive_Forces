using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Blank : Action {
		
		public A_Blank(IngameObject obj) : base(obj){
			this.name="A_Blank";
		}
		
		public override void begin(){
			
		}
		
		public override void abort(){
			
		}
		
		public override void finish(){
			
		}
	}
}
