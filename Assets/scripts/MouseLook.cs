using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Scripts.Control
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;

        public void Init(Transform camera)
        {
        }

        public void LookRotation(Transform camera,float downMax,float upMax)	//horizont is 0 degrees (euler angles)
        {
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
			
			float targetYRot=camera.eulerAngles.y+yRot*3.2f;
			float targetXRot=camera.eulerAngles.x-xRot;
			
			if(targetXRot<180){
				if(targetXRot>downMax){
					targetXRot=downMax;
				}else if(upMax<180&&targetXRot<upMax){
					targetXRot=upMax;
				}
			}else if(targetXRot>180&&targetXRot<upMax){
				targetXRot=upMax;
			}
			
            camera.eulerAngles = new Vector3(targetXRot,targetYRot,camera.eulerAngles.z);
        }

    }
}
