using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Takes in screen shake commands for:
// - impact (type[enum], amplitude[float], tween [enum], duration [float:seconds])
// - charge (type [enum], amount [0.0–1.0], amplitude [float], tween [enum], smoothing [float])

// TODO:
// - Should know how to create default shake effects for all FX types
// - should be able to increment and decrement charge shake


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
	
	// edit mode debugging bools (can be deleted or commented out later)
	public bool			chargeIncreaseNow		= false	;
	public bool			chargeDecreaseNow		= false	;
	public bool			impactNow				= false	;

	// declare top-level properties
	//====
	public Transform		CameraTransform			; 
	public float			maximumShakeAmplitude	= 5.0f; // in world units (to keep summation of amplitudes from shaking the camera too violently)
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
		private chargeType 	typeVal 		;
		private float 		amountVal 		;
		private float 		targetAmountVal	;
		private float 		amplitudeVal 	;
		private tweenType	tweenVal		;
		private float 		smoothingVal	;
		private float 		durationVal		;
		private float		lifeVal			;
		
		public charge(chargeType a1, float a2, float a3, float a4, tweenType a5, float a6, float a7, float a8) // 
		{
			// TODO: use defaults? (wish I could declare these in a .h file :)
			//			chargeTypeDefault		
			//         	chargeAmountDefault		
			//         	chargeAplitudeDefault	
			//         	chargeTweenDefault		
			//         	chargeSmoothingDefault	
			//         	chargeDurationDefault	
			
			typeVal 			= a1;
			amountVal 			= a2; // 0.0f
			targetAmountVal		= a3;
			amplitudeVal		= a4;
			tweenVal	 		= a5;
			smoothingVal		= a6;
			durationVal			= a7;
			lifeVal 			= a8; // a7
		}
		public chargeType type
		{
			get
			{
				return typeVal;
			}
			set
			{
				typeVal = value;
			}
		}
		public float amount
		{
			get
			{
				return amountVal;
			}
			set
			{ 
				amountVal = Mathf.Max (Mathf.Min (value, 1.0f), 0.0f); // clamp values to be from 0.0f to 1.0f
			}
		}
		public float targetAmount
		{
			get
			{
				return targetAmountVal;
			}
			set
			{ 
				targetAmountVal = Mathf.Max (Mathf.Min (value, 1.0f), 0.0f); // clamp values to be from 0.0f to 1.0f
			}
		}
		public float amplitude
		{
			get
			{
				return amplitudeVal;
			}
			set
			{
				amplitudeVal = value;
			}
		}
		public tweenType tween
		{
			get
			{
				return tweenVal;
			}
			set
			{
				tweenVal = value;
			}
		}
		public float smoothing
		{
			get
			{
				return smoothingVal;
			}
			set
			{ 
				smoothingVal = Mathf.Max (Mathf.Min (value, 1.0f), 0.0f); // clamp values to be from 0.0f to 1.0f
			}
		}
		public float duration
		{
			get
			{
				return durationVal;
			}
			set
			{
				durationVal = value;
			}
		}
		public float life
		{
			get
			{
				return lifeVal;
			}
			set
			{
				lifeVal = Mathf.Max (Mathf.Min (value, durationVal), 0.0f);
			}
		}
	}
	public struct impact {
		private impactType 	typeVal 		;
		private float 		amplitudeVal 	;
		private tweenType	tweenVal		;
		private float 		durationVal		;
		private float		lifeVal			;

		public impact(impactType a1, float a2, tweenType a3, float a4, float a5){ // 

			// TODO: use defaults ? (wish I could declare these in a .h file :)
			//			impactTypeDefault		
			//        	impactAplitudeDefault	
			//         	impactTweenDefault		
			//         	impactDurationDefault	
			
			typeVal			= a1;
			amplitudeVal	= a2;
			tweenVal		= a3;
			durationVal		= a4;
			lifeVal			= a5; // a4
		}
		public impactType type
		{
			get
			{
				return typeVal;
			}
			set
			{
				typeVal = value;
			}
		}
		public float amplitude
		{
			get
			{
				return amplitudeVal;
			}
			set
			{
				amplitudeVal = value;
			}
		}
		public tweenType tween
		{
			get
			{
				return tweenVal;
			}
			set
			{
				tweenVal = value;
			}
		}
		public float duration
		{
			get
			{
				return durationVal;
			}
			set
			{
				durationVal = value;
			}
		}
		public float life
		{
			get
			{
				return lifeVal;
			}
			set
			{
				lifeVal = Mathf.Max (Mathf.Min (value, durationVal), 0.0f);
			}
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
	}

	void OnDestroy() {
		//cleanup
		chargeShakes.Clear ();
		impactShakes.Clear ();
	}
		
	// Update is called once per frame
	void Update () {
		// check debugging flags
		if (chargeIncreaseNow) {
			if (chargeShakes.Count < 1)
				NewCharge ();

			float newTargetAmount = chargeShakes[0].targetAmount;
			newTargetAmount = Mathf.Min (1.0f, newTargetAmount + 0.1f);
			//   chargeShakes[0].targetAmount = newTargetAmount; // TODO: this set function refuses to work
			// So this is the work-around
			chargeShakes[0] = new charge (
				chargeShakes[0].type		,	
				chargeShakes[0].amount		,
				newTargetAmount				,
				chargeShakes[0].amplitude	,
				chargeShakes[0].tween		,
				chargeShakes[0].smoothing	,
				chargeShakes[0].duration	,
				chargeShakes[0].life
				);

			chargeIncreaseNow = false;
		}
		if (chargeDecreaseNow) {
			if (chargeShakes.Count > 0)
			{
				float newTargetAmount = chargeShakes[0].targetAmount;
				newTargetAmount = Mathf.Max (0.0f, newTargetAmount - 0.1f);

				//	chargeShakes[0].targetAmount = newTargetAmount; // TODO: this set function refuses to work
				// So this is the work-around
				chargeShakes[0] = new charge (
					chargeShakes[0].type		,
					chargeShakes[0].amount		,
					newTargetAmount				,
					chargeShakes[0].amplitude	,
					chargeShakes[0].tween		,
					chargeShakes[0].smoothing	,
					chargeShakes[0].duration	,
					chargeShakes[0].life
					);
			}

			chargeDecreaseNow = false;
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
					for (int i = chargeShakes.Count; i > 0; i--)
					{
						// add to screen shake amplitude // TODO: use other tweens here (currently linear)
						float iAmp = chargeShakes[i-1].amplitude * chargeShakes[i-1].amount;
						amplitude = amplitude + iAmp;
						// update time-sensitive elements 		// use delta time for duration-specific ones
						float lifeNew = chargeShakes[i-1].life - Time.deltaTime;
						float amountNew = chargeShakes[i-1].amount + (
							(chargeShakes[i-1].targetAmount - chargeShakes[i-1].amount)
							* chargeShakes[i-1].smoothing
							);
						// remove any zeroed out structs
						if (lifeNew <= 0.0f)
							chargeShakes.RemoveAt(i-1);
						else
						{
							//	impactShakes[i-1].life = lifeNew; // TODO: this set method refuses to work
							// So this is a work-around
							chargeShakes[i-1] = new charge (
								chargeShakes[i-1].type			,
								amountNew						,
								chargeShakes[i-1].targetAmount	,
								chargeShakes[i-1].amplitude		,
								chargeShakes[i-1].tween			,
								chargeShakes[i-1].smoothing		,
								chargeShakes[i-1].duration		,
								lifeNew
								);
						}
					}
				}
			}
			if (impactShakes.Count != 0)
			{
				for (int i = impactShakes.Count; i > 0; i--)
				{
					// add to screen shake amplitude // TODO: use other tweens here (currently linear)
					float iAmp = impactShakes[i-1].amplitude * (impactShakes[i-1].life / impactShakes[i-1].duration);
					amplitude = amplitude + iAmp;
					// update time-sensitive elements 		// use delta time for duration-specific ones
					float lifeNew = impactShakes[i-1].life - Time.deltaTime;
					// remove any zeroed out structs
					if (lifeNew <= 0.0f)
						impactShakes.RemoveAt(i-1);
					else
					{
					 	//	impactShakes[i-1].life = lifeNew; // TODO: this set method refuses to work
						// So this is a work-around
						impactShakes[i-1] = new impact (
							impactShakes[i-1].type		,	
							impactShakes[i-1].amplitude	,
							impactShakes[i-1].tween		,
							impactShakes[i-1].duration	,
							lifeNew
							);
					}
				}
			}

			// clamp amplitude at maximum
			amplitude = Mathf.Min(maximumShakeAmplitude, amplitude);

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
	}

	void NewImpact () {
		
		// create new struct with arguments handed in (or defaults)
		impact impactTemp = new impact(
			impactTypeDefault		,	
         	impactAplitudeDefault	,
         	impactTweenDefault		,
         	impactDurationDefault	,
			impactDurationDefault
			);
		// add it to the array of impact structs
		impactShakes.Add(impactTemp);

		cameraStill = false;
	}
	
	void NewCharge () {
		// create new struct with arguments handed in (or defaults)
		charge chargeTemp = new charge(
			chargeTypeDefault		,
			0.0f					,
       		chargeAmountDefault		,
         	chargeAplitudeDefault	,
         	chargeTweenDefault		,
         	chargeSmoothingDefault	,
         	chargeDurationDefault	,
			chargeDurationDefault
			);
		// add it to the array of chage structs
		chargeShakes.Add(chargeTemp);

		//TODO: return a unique ID for updates to this charge effect

		cameraStill = false;
	}

	void UpdateCharge () {
		// change the value by specifed incremant (-1.0f to 1.0f) of a particular charge with the specified ID
	}
}
