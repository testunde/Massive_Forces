using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IO_Neutral : IngameObject {
		
		public IO_Neutral() : base(){
			this.type="IO_Neutral";
			fraction=0;
			//costs must be negativ (e.g. for Trees when cut down)
		}
	}
}
