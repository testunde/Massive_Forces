using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IO_Unit : IngameObject {
		
		public IO_Unit() : base(){
		}
		
		public virtual void moveTo(Vector3 coords){
			//set velocity of rigidbody of model
			//later: a* algorithm
		}
	}
}
