using UnityEngine;
using System.Collections;

// Should take in screen shake commands for:
// - charge (type [enum: lightning, rain, meteor, platform, tsunami, etc.], amount [0.0–1.0], amplitude [float], slope [sqrt(x), inverted square -(x-1)^2+1], smoothing [float])
// - impact (type[enum: meteor, lightning, tsunami], amplitude[float], duration [float:seconds])

// Should know how to create default shake effects for all FX types

// Should queue all effects into arrays so that they can be applied in one step
// - charge shakes (should really only be one at any given point, but create an array just in case)
// - impact shakes

[ExecuteInEditMode]

public class screenShake : MonoBehaviour {

	// declare enums
	//====
	public enum chargeType {
		CHARGE_RAIN,
		CHARGE_LIGHTNING,
		CHARGE_METEOR, 
		CHARGE_PLATFORM,
		CHARGE_WAVE,
		CHARGE_TSUNAMI,
		CHARGE_COUNT
	}	
	public enum impactType {
		IMPACT_WIND,
		IMPACT_LIGHTNING,
		IMPACT_METEOR, 
		IMPACT_PLATFORM,
		IMPACT_WAVE,
		IMPACT_TSUNAMI,
		IMPACT_COUNT
	}
	public enum tweenType {
		TWEEN_LIN_ON,			// x
		TWEEN_LIN_OFF,			// -x + 1
		TWEEN_SQ_EASEOUT_ON, 	// x^2
		TWEEN_SQ_EASEOUT_OFF, 	// x^2 + 1
		TWEEN_SQ_EASEIN_ON,		// -(x-1)^2 + 1
		TWEEN_SQ_EASEIN_OFF,	// (x-1)^2
		TWEEN_SQRT_EASEOUT_ON, 	// -sqrt(-x+1) + 1
		TWEEN_SQRT_EASEOUT_OFF,	// sqrt(-x+1)
		TWEEN_SQRT_EASEIN_ON, 	// sqrt(x)
		TWEEN_SQRT_EASEIN_OFF,	// -sqrt(x) + 1
		TWEEN_COUNT
	}

	// struct constructors
	//====
	// TODO: Figure out a way to set defaults in constructor?
	public struct charge {
		public chargeType 	type 		;
		public float 		amount 		;
		public float 		amplitude 	;
		public tweenType	tween		;
		public float 		smoothing	;
		public float 		duration	;
	}
	public struct impact {
		public impactType 	type 		;// : impactType.IMPACT_LIGHTNING;
		public float 		amplitude 	;// : 1.0f; // any value (should be relatively lower for charge FX compared to Impact FX?)
		public tweenType	tween		;// : slop.SLOPE_SQRT; // might be a naming conflict
		public float 		duration	;// : 0.1f;
	}

	// edit mode debugging bools (can be deleted or commented out later)
	public bool			chargeIncreaseNow		= false	;
	public bool			chargeDecreaseNow		= false	;
	public bool			impactNow				= false	;

	// declare top-level properties
	//====
	public Transform	CameraTransform			; 
	public float		maximumShakeAmplitude	= 1.0f; // in world units (to keep summation of amplitudes from shaking the camera too violently)
	// TODO: array of charge structs
	// TODO: array of impact structs
	

	// charge shake defaults
	//====
	public chargeType	chargeTypeDefault		= chargeType.CHARGE_LIGHTNING;
	public float		chargeAmountDefault		= 0.1f; // 0.0f – 1.0f
	public float		chargeAplitudeDefault	= 0.5f; // any value (should be relatively lower for charge FX compared to Impact FX?)
	public tweenType 	chargeTweenDefault		= tweenType.TWEEN_SQRT_EASEOUT_ON;
	public float	 	chargeSmoothingDefault	= 0.1f; // amount of smoothig per update: 0.0 (no smoothing), 1.0 (infinite smoothing [will never reach target charge amount])
	public float		chargeDurationDefault	= 1000.0f; // in seconds (large for the purpose of cleanup just in case a charge is created but forgotten)

	// impact shake defaults
	//====
	public impactType	impactTypeDefault		= impactType.IMPACT_LIGHTNING;
	public float		impactAplitudeDefault	= 1.0f; // any value (should be relatively lower for charge FX compared to Impact FX?)
	public tweenType 	impactTweenDefault		= tweenType.TWEEN_SQ_EASEOUT_OFF;
	public float		impactDurationDefault	= 0.25f; // in seconds (most impact effects should be less than a second)

	// Use this for initialization
	void Start () {
		// initialize properties

		// reference of main camera
		//		CameraTransform = Camera.main.transform; // TODO: doesn't seem to work in edit mode
		// try searching this object and its children for any cameras and grab the first one?
		// 		CameraTransform = GetComponentInChildren<Camera> ().transform; // doesn't seem to work either

		// array of charge structs
		// array of impact structs
	}
	
	// Update is called once per frame
	void Update () {
		// check debugging flags

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
