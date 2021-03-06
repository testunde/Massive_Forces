using UnityEngine;
using System.Collections;
using Scripts;

namespace Database {
	public class IO_Database {
		public string name;
		public string type;
		public int maxHP;
		public int damage=0;
		public float maxSpeed=0f,accel=0f;
		public float buildTime;
		public float markerSize;
		public long[] costs;
		public string[,] actions;
		public int workerUnits=0;
		
		public IO_Database(){
			actions=new string[ActionMatrix.width,ActionMatrix.height];
		}
	}
}
