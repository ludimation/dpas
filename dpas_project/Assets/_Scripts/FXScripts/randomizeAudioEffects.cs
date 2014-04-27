using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
[RequireComponent(typeof(AudioEchoFilter))]
[RequireComponent(typeof(AudioDistortionFilter))]
[RequireComponent(typeof(AudioReverbFilter))]
[RequireComponent(typeof(AudioChorusFilter))]

public class randomizeAudioEffects : MonoBehaviour {

	public bool 					randomizeNow = false;
	public bool 					audPlayInEdit = false;
	public AudioSource 				audSrc; // pitch might be fun to play with //this variable is technically unecessary since you can automatically get this with inherited "audio" variable
	public AudioLowPassFilter 		audLowPassFilter;
	public AudioHighPassFilter 		audHighPassFilter;
	public AudioEchoFilter 			audEchoFilter; // delay of 10 seems pretty fun (and other shorter delays)
	public AudioDistortionFilter 	audDistortionFilter; // DistortionLevel needs to be combined with lowered volumes
	public AudioReverbFilter 		audReverbFilter; // interesting presets: Bathroom, StoneCorridor, Qarry, Drugged, Dizzy, Psychotic
	public AudioChorusFilter 		audChorusFilter; // Depth variable seems most effective

	private Behaviour[]				filters;
	private float					updateTime;

	// Use this for initialization
	void Start () {
		audSrc = audio;
		audLowPassFilter 	= GetComponent<AudioLowPassFilter 		>();
		audHighPassFilter	= GetComponent<AudioHighPassFilter 		>();
		audEchoFilter		= GetComponent<AudioEchoFilter 			>();
		audDistortionFilter	= GetComponent<AudioDistortionFilter	>();
		audReverbFilter 	= GetComponent<AudioReverbFilter 		>();
		audChorusFilter 	= GetComponent<AudioChorusFilter 		>();

		filters = new Behaviour[] {
			audLowPassFilter, 
			audHighPassFilter,
			audEchoFilter,
			audDistortionFilter,
			audReverbFilter,
   	      	audChorusFilter
		};

		randomizeFilters(true);

	}

	// Update is called once per frame
	void Update () {
		// randomize now for Edit mode
		if (randomizeNow)
		{
			Start ();

			// TODO: have this execute in edit mode only at base level of Update()
			if (audPlayInEdit)
			{
				randomizeFilters(true);
				audSrc.Play();
			}
			else
			{
				randomizeFilters(false);
				audSrc.Stop();
			}

			randomizeNow = false;
		}


		// randomize every time that the audSrc reaches zero 		// audSrc.clip.length might also be useful
		if (Time.time >= updateTime && audSrc.time <= 0.1f)
			randomizeFilters(true);
	}

	// Called whenever filters need to be randomized
	void randomizeFilters (bool randomize) {
		int j = Random.Range (0, filters.Length);
		int k = Random.Range (0, filters.Length);
		int l = Random.Range (0, filters.Length);
		int m = Random.Range (0, filters.Length);
		int n = Random.Range (0, filters.Length);
		int o = Random.Range (0, filters.Length);

		for (int i = 0; i < filters.Length; i++)
		{
			if (randomize && (i == j || i == k || i == l || i == m || i == n || i == o))
				filters[i].enabled = true;
			else
				filters[i].enabled = false;
		}

		updateTime = Time.time + (audSrc.clip.length * 0.9f);
	}

}