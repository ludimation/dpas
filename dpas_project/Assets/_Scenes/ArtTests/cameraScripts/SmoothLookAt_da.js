var target : Transform;
var xOffset = 0.0;
var yOffset = 0.0;
var zOffset = 0.0;
var damping = 6.0;
var smooth = true;

@script AddComponentMenu("Camera-Control/Smooth Look At")

//This file has been modified from the standard asset "SmoothLookAt" to take into account an offset so that it doesn't default to poining at the character's feet.


function LateUpdate () {
	if (target) {
		var postion = target.position + Vector3(xOffset, yOffset, zOffset);
		
		if (smooth)
		{
			// Look at and dampen the rotation
			var rotation = Quaternion.LookRotation(postion - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}
		else
		{
			// Just lookat
		    transform.LookAt(postion);
		}
	}
}

function Start () {
	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;
}