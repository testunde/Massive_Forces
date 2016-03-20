using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IO_Neutral : IngameObject {
		
		public IO_Neutral() : base(){
			fraction=0;
			//costs must be negativ (e.g. for Trees when cut down)
		}
	}
}
