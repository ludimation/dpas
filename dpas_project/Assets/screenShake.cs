using UnityEngine;
using System.Collections;

// Should take in screen shake commands for:
// - charge (type [enum: lightning, rain, meteor, platform, tsunami, etc.], amount [0.0–1.0], amplitude [float], slope [sqrt(x), inverted square -(x-1)^2+1], smoothing [float])
// - impact (type[enum: meteor, lightning, tsunami], amplitude[float], duration [float:seconds])

// Should know how to create default shake effects for all FX types

// Should queue all effects into arrays so that they can be applied in one step
// - charge shakes (should really only be one at any given point, but create an array just in case)
// - impact shakes

enum slope {
	SLOPE_LIN_ON,			// x
	SLOPE_LIN_OFF,			// -x + 1
	SLOPE_SQ_EASEOUT_ON, 	// x^2
	SLOPE_SQ_EASEOUT_OFF, 	// x^2 + 1
	SLOPE_SQ_EASEIN_ON,		// -(x-1)^2 + 1
	SLOPE_SQ_EASEIN_OFF,	// (x-1)^2
	SLOPE_SQRT_EASEOUT_ON, 	// -sqrt(-x+1) + 1
	SLOPE_SQRT_EASEOUT_OFF,	// sqrt(-x+1)
	SLOPE_SQRT_EASEIN_ON, 	// sqrt(x)
	SLOPE_SQRT_EASEIN_OFF,	// -sqrt(x) + 1
	SLOPE_COUNT
}

enum chargeType {
	CHARGE_RAIN,
	CHARGE_LIGHTNING,
	CHARGE_METEOR, 
	CHARGE_PLATFORM,
	CHARGE_WAVE,
	CHARGE_TSUNAMI,
	CHARGE_COUNT
}

enum impactType {
	IMPACT_WIND,
	IMPACT_LIGHTNING,
	IMPACT_METEOR, 
	IMPACT_PLATFORM,
	IMPACT_WAVE,
	IMPACT_TSUNAMI,
	IMPACT_COUNT
}

// TODO: Figure out a way to set defaults in constructor?

public struct charge {
	public string 	type 		;// : chargeType.CHARGE_LIGHTNING;
	public float 	amount 		;// : 0.5f; // 0.0f – 1.0f
	public float 	amplitude 	;// : 1.0f; // any value (should be relatively lower for charge FX compared to Impact FX?)
	public float 	slope 		;// : slope.SLOPE_SQRTIN; // might be a naming conflict
	public float 	smoothing	;// : 0.1f;
	public float 	duration	;// : 1000.0f; // seconds (might want to put this in here anyway so they self-destruct after a while if forgotten
}

public struct impact {
	public string 	type 		;// : impactType.IMPACT_LIGHTNING;
	public float 	amplitude 	;// : 1.0f; // any value (should be relatively lower for charge FX compared to Impact FX?)
	public float 	slope		;// : slop.SLOPE_SQRT; // might be a naming conflict
	public float 	duration	;// : 0.1f;
}

public class screenShake : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// initialize properties
		// - array of charge structs
		// - array of impact structs
	}
	
	// Update is called once per frame
	void Update () {
		// cycle through impact and charge structs to calculate screen shake
		// use delta time for duration-specific ones
		// reset position of camera if none exist
	}

	void NewImpact () {
		// create new struct with arguments handed in (or defaults)
		// add it to the array of impact structs
	}
	
	void NewCharge () {
		// create new struct with arguments handed in (or defaults)
		// add it to the array of chage structs
		// return a unique ID for updates to this charge effect
	}

	void UpdateCharge () {
		// should change the value (0.0f to 1.0f) of a particular charge with the specified ID
	}
}
