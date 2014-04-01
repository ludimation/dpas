using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Should take in screen shake commands for:
// - charge (type [enum: lightning, rain, meteor, platform, tsunami, etc.], amount [0.0–1.0], amplitude [float], slope [sqrt(x), inverted square -(x-1)^2+1], smoothing [float])
// - impact (type[enum: meteor, lightning, tsunami], amplitude[float], duration [float:seconds])

// Should know how to create default shake effects for all FX types

// Should queue all effects into arrays so that they can be applied in one step
// - charge shakes (should really only be one at any given point, but create an array just in case)
// - impact shakes

//[ExecuteInEditMode]

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

//*/
	// edit mode debugging bools (can be deleted or commented out later)
	public bool			chargeIncreaseNow		= false	;
	public bool			chargeDecreaseNow		= false	;
	public bool			impactNow				= false	;

	// declare top-level properties
	//====
	public Transform		CameraTransform			; 
	public float			maximumShakeAmplitude	= 1.0f; // in world units (to keep summation of amplitudes from shaking the camera too violently)
	private bool			cameraStill = true;
	private List<charge> 	chargeShakes;
	private List<impact> 	impactShakes;

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

	// struct constructors
	//====
	// TODO: Figure out a way to set defaults in constructor?
	public struct charge {
		public chargeType 	type 		;
		public float 		amount 		;
		public float 		targetAmount;
		public float 		amplitude 	;
		public tweenType	tween		;
		public float 		smoothing	;
		public float 		duration	;
		public float		life			;
		
			public charge(chargeType a1, float a2, float a3, float a4, tweenType a5, float a6, float a7, float a8){
//				if (a1 == null)
//					a1 = chargeType.CHARGE_LIGHTNING;
//				if (a2 == null)
//					a2 = 0.0f;
//				if (a3 == null)
//					a3 = 0.1f;
//				if (a4 == null)
//					a4 = 0.5f;
//				if (a5 == null)
//					a5 = tweenType.TWEEN_SQRT_EASEOUT_ON;
//				if (a6 == null)
//					a6 = 0.1f;
//				if (a7 == null)
//					a7 = 1000.0f;
				
				// try to use defaults instead? (wish I could declare these in a .h file :)
				//			chargeTypeDefault		
				//         	chargeAmountDefault		
				//         	chargeAplitudeDefault	
				//         	chargeTweenDefault		
				//         	chargeSmoothingDefault	
				//         	chargeDurationDefault	
				
				type 			= a1;
				amount 			= a2;
				targetAmount	= a3;
				amplitude		= a4;
				tween	 		= a5;
				smoothing		= a6;
				duration		= a7;
				life 			= a8;
			}
	}
	public struct impact {
		public impactType 	type 		;
		public float 		amplitude 	;
		public tweenType	tween		;
		public float 		duration	;
		public float		life			;

		public impact(impactType a1, float a2, tweenType a3, float a4, float a5){
//			if (a1 == null)
//				a1 = impactType.IMPACT_LIGHTNING;
//			if (a2 == null)
//				a2 = 1.0f;
//			if (a3 == null)
//				a3 = tweenType.TWEEN_SQ_EASEOUT_OFF;
//			if (a4 == null)
//				a4 = 0.25f;

			// try to use defaults instead? (wish I could declare these in a .h file :)
			//			impactTypeDefault		
			//        	impactAplitudeDefault	
			//         	impactTweenDefault		
			//         	impactDurationDefault	
			
			type 			= a1;
			amplitude		= a2;
			tween			= a3;
			duration		= a4;
			life 			= a5;
		}
	}
	//*/
	
	// Use this for initialization
	void Start () {
		// initialize properties

		// reference of main camera
		//		CameraTransform = Camera.main.transform; // TODO: doesn't seem to work in edit mode
		// try searching this object and its children for any cameras and grab the first one?
		// 		CameraTransform = GetComponentInChildren<Camera> ().transform; // doesn't seem to work either


		// array of charge structs
		chargeShakes = new List<charge>();
		// array of impact structs
		impactShakes = new List<impact>();
		//*/
	}

	void OnDestroy() {
		/*
		//cleanup
		chargeShakes.Clear ();
		impactShakes.Clear ();
		//*/
	}
		
	// Update is called once per frame
	void Update () {
		// check debugging flags
		if (chargeIncreaseNow) {
//			if (chargeShakes.Count < 1)
//				NewCharge ();
//
//
//			if (chargeShakes[0].amount < 1.0f)
//				chargeShakes[0].amount = chargeShakes[0].amount + 0.1f;
//
//			if (chargeShakes[0].amount > 1.0f)
//				chargeShakes[0].amount = 1.0f;

			chargeIncreaseNow		= false	;
		}
		if (chargeDecreaseNow) {
//			if (chargeShakes.Count > 0)
//			{
//				if (chargeShakes[0].amount > 0.0f)
//					chargeShakes[0].amount += 0.1f;
//
//				if (chargeShakes[0].amount < 0.0f)
//					chargeShakes[0].amount = 0.0f;
//			}

			chargeDecreaseNow		= false	;
		}
		if (impactNow) {
			NewImpact ();

			// CameraTransform.localPosition = CameraTransform.localPosition + ((Random.insideUnitSphere / 2.0f - CameraTransform.localPosition) * 0.5f) ;
			//			CameraTransform.rotation = CameraTransform.localPosition + ((Random.insideUnitSphere / 2.0f - CameraTransform.localPosition) * 0.5f) ;


			impactNow				= false	;
		}


		Vector3 	shakePosition	= new Vector3 ();
		Quaternion	shakeRotation	= new Quaternion ();
		float 		amplitude		= 0.0f;

		if(chargeShakes.Count != 0 || impactShakes.Count != 0){
			// cycle through impact and charge structs 
			if (chargeShakes.Count != 0)
			{
				foreach (charge element in chargeShakes) {
					// add to screen shake amplitude
					// update time-sensitive elements 		// use delta time for duration-specific ones
					// remove any zeroed out structs
				}
			}
			if (impactShakes.Count != 0)
			{
				for (int i = impactShakes.Count; i > 0; i--)
				{
					// add to screen shake amplitude
					float iAmp = impactShakes[i-1].amplitude * (impactShakes[i-1].life / impactShakes[i-1].duration);
					amplitude = amplitude + iAmp;
					// update time-sensitive elements 		// use delta time for duration-specific ones
					float lifeNew = impactShakes[i-1].life - Time.deltaTime;
					impactShakes[i-1] = new impact(
						impactShakes[i-1].type, 
						impactShakes[i-1].amplitude, 
						impactShakes[i-1].tween, 
						impactShakes[i-1].duration,
						lifeNew
						);
					// remove any zeroed out structs
					if (impactShakes[i-1].life <= 0.0f)
						impactShakes.RemoveAt(i-1);
				}
			}

			// might wanna use Mathf.PerlinNoise instead of random points
			// Mathf.PerlinNoise
			shakePosition = Random.insideUnitSphere * amplitude/2.0f;
			shakeRotation = Quaternion.Euler(shakePosition.x, shakePosition.y, shakePosition.z);

			CameraTransform.localPosition = shakePosition;
			CameraTransform.localRotation = shakeRotation;
		}
		else if (!cameraStill)
		{
			// reset position of camera if no shakes exist
			CameraTransform.localPosition = new Vector3(0,0,0);

			cameraStill = true;
		}
		//*/
	}

	void NewImpact () {
		
		// create new struct with arguments handed in (or defaults)
		impact impactTemp = new impact(
			impactTypeDefault,	
         	impactAplitudeDefault,
         	impactTweenDefault,
         	impactDurationDefault,
			impactDurationDefault
			);
		// add it to the array of impact structs
		impactShakes.Add(impactTemp);

		cameraStill = false;
		//*/
	}
	
	void NewCharge () {
		/*
		// create new struct with arguments handed in (or defaults)
		charge chargeTemp = new charge();
		// add it to the array of chage structs
		chargeShakes.Add(chargeTemp);

		//TODO: return a unique ID for updates to this charge effect

		cameraStill = false;
		//*/
	}

	void UpdateCharge () {
		// change the value by specifed incremant (-1.0f to 1.0f) of a particular charge with the specified ID
	}
}
