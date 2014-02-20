using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public class KinectManager : MonoBehaviour
{
	// Public Bool to determine how many players there are. Default of one user.
	public bool TwoUsers = false;
	
	// Public Bool to determine if the sensor is used in near mode.
	public bool NearMode = false;
	
	// Public Bool to determine whether to receive and compute the user map
	public bool ComputeUserMap = false;
	
	// Public Bool to determine whether to receive and compute the color map
	public bool ComputeColorMap = false;
	
	// Public Bool to determine whether to display user map on the GUI
	public bool DisplayUserMap = false;
	
	// Public Bool to determine whether to display color map on the GUI
	public bool DisplayColorMap = false;
	
	// Bools to keep track of who is currently calibrated.
	bool Player1Calibrated = false;
	bool Player2Calibrated = false;
	
	bool AllPlayersCalibrated = false;
	
	// Values to track which ID (assigned by the Kinect) is player 1 and player 2.
	uint Player1ID;
	uint Player2ID;
	
	// Lists of GameObjects that will be controlled by which player.
	public List<GameObject> Player1Avatars;
	public List<GameObject> Player2Avatars;
	
	// Lists of AvatarControllers that will let the models get updated.
	private List<AvatarController> Player1Controllers;
	private List<AvatarController> Player2Controllers;
	
	// Variables needed to track the users.
	private KinectWrapper.UserDelegate NewUser;
	private KinectWrapper.UserDelegate CalibrationStarted;
	private KinectWrapper.UserDelegate CalibrationFailed;
    private KinectWrapper.UserDelegate CalibrationSuccess;
    private KinectWrapper.UserDelegate UserLost;

	private KinectWrapper.SkeletonJointTransformation jointTransform;
	private KinectWrapper.SkeletonJointPosition jointPosition;
	private KinectWrapper.SkeletonJointOrientation jointOrientation;
	
	// User/Depth map variables
	private Texture2D usersLblTex;
	private Color[] usersMapColors;
	private Rect usersMapRect;
	private int usersMapSize;
	private short[] usersLabelMap;
	private short[] usersDepthMap;
	private float[] usersHistogramMap;
	
	// Color map variables
	private Texture2D usersClrTex;
	private Color32[] usersClrColors;
	private Rect usersClrRect;
	private int usersClrSize;
	private byte[] usersColorMap;
	
	// List of all users
	private List<uint> allUsers;
	
	// GUI Text to show messages.
	private GameObject CalibrationText;
	
	// Bool to keep track of whether OpenNI has been initialized
	private bool KinectInitialized = false; 
	
	// The single instance of KinectManager
	private static KinectManager instance;

	
	// returns the single KinectManager instance
    public static KinectManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	// checks if Kinect is initialized and ready to use. If not, there was an error during Kinect-sensor initialization
	public static bool IsKinectInitialized()
	{
		return instance != null ? instance.KinectInitialized : false;
	}
	
	// this function is used internally by AvatarController
	public static bool IsCalibrationNeeded()
	{
		return true;
	}
	
	// returns the depth image/users histogram texture,if ComputeUserMap is true
    public Texture2D GetUsersLblTex()
    { 
		return usersLblTex;
	}
	
	// returns the color image texture,if ComputeColorMap is true
    public Texture2D GetUsersClrTex()
    { 
		return usersClrTex;
	}
	
	// returns true if at least one user is currently detected by the sensor
	public bool IsUserDetected()
	{
		return KinectInitialized && (allUsers.Count > 0);
	}
	
	// returns the UserID of Player1, or 0 if no Player1 is detected
	public uint GetPlayer1ID()
	{
		return Player1ID;
	}
	
	// returns the UserID of Player2, or 0 if no Player2 is detected
	public uint GetPlayer2ID()
	{
		return Player2ID;
	}
	
	// returns true if the User is calibrated and ready to use
	public bool IsPlayerCalibrated(uint UserId)
	{
		if(UserId == Player1ID)
			return Player1Calibrated;
		else if(UserId == Player2ID)
			return Player2Calibrated;
		
		return false;
	}
	
	// returns the User position, relative to the Kinect-sensor, in meters
	public Vector3 GetUserPosition(uint UserId)
	{
		if(KinectWrapper.GetJointPosition(UserId, (int)KinectWrapper.SkeletonJoint.HIPS, ref jointPosition))
		{
			return new Vector3(jointPosition.x * 0.001f, jointPosition.y * 0.001f, jointPosition.z * 0.001f);
		}
		
		return Vector3.zero;
	}
	
	// returns the User rotation, relative to the Kinect-sensor
	public Quaternion GetUserOrientation(uint UserId, bool flip)
	{
		if(KinectWrapper.GetJointOrientation(UserId, (int)KinectWrapper.SkeletonJoint.HIPS, ref jointOrientation))
		{
			Quaternion quat = ConvertMatrixToQuat(jointOrientation, (int)KinectWrapper.SkeletonJoint.HIPS, flip);
			return quat;
		}
		
		return Quaternion.identity;
	}
	
	// returns true if the given joint's position is being tracked
	public bool IsJointPositionTracked(uint UserId, int joint)
	{
		float fConfidence = KinectWrapper.GetJointPositionConfidence(UserId, joint);
		return fConfidence > 0.5;
	}
	
	// returns true if the given joint's orientation is being tracked
	public bool IsJointOrientationTracked(uint UserId, int joint)
	{
		float fConfidence = KinectWrapper.GetJointOrientationConfidence(UserId, joint);
		return fConfidence > 0.5;
	}
	
	// returns the joint position of the specified user, relative to the Kinect-sensor, in meters
	public Vector3 GetJointPosition(uint UserId, int joint)
	{
		if(KinectWrapper.GetJointPosition(UserId, joint, ref jointPosition))
		{
			return new Vector3(jointPosition.x * 0.001f, jointPosition.y * 0.001f, jointPosition.z * 0.001f);
		}

		return Vector3.zero;
	}
	
	// returns the joint rotation of the specified user, relative to the Kinect-sensor
	public Quaternion GetJointOrientation(uint UserId, int joint, bool flip)
	{
		if(KinectWrapper.GetJointOrientation(UserId, joint, ref jointOrientation))
		{
			Quaternion quat = ConvertMatrixToQuat(jointOrientation, joint, flip);
			return quat;
		}

		return Quaternion.identity;
	}
	
	// removes the currently detected kinect users, allowing a new detection/calibration process to start
	public void ClearKinectUsers()
	{
		if(!KinectInitialized)
			return;

		// remove current users
		for(int i = allUsers.Count - 1; i >= 0; i--)
		{
			uint userId = allUsers[i];
			OnUserLost(userId);
		}
		
		//ResetFilters();
	}

	
	//----------------------------------- end of public functions --------------------------------------//

	void Awake() 
	{
		CalibrationText = GameObject.Find("CalibrationText");
		
		try
		{
			if(KinectWrapper.CheckOpenNIPresence())
			{
				// reload the same level
				Application.LoadLevel(Application.loadedLevel);
			}
		} 
		catch (Exception ex) 
		{
			Debug.LogError(ex.ToString());
			if(CalibrationText != null)
				CalibrationText.guiText.text = ex.Message;
		}
		
	}

	void Start()
	{
		try 
		{
			// Initialize the OpenNI/NiTE wrapper
			int rc = KinectWrapper.Init(ComputeUserMap, ComputeColorMap);
	        if (rc != 0)
	        {
	            throw new Exception(String.Format("Error initing OpenNI: {0}", Marshal.PtrToStringAnsi(KinectWrapper.GetLastErrorString())));
	        }
			
			if(ComputeUserMap)
			{
		        // Initialize depth & label map related stuff
		        usersMapSize = KinectWrapper.GetDepthWidth() * KinectWrapper.GetDepthHeight();
		        usersLblTex = new Texture2D(KinectWrapper.GetDepthWidth(), KinectWrapper.GetDepthHeight());
		        usersMapColors = new Color[usersMapSize];
		        usersMapRect = new Rect(Screen.width, Screen.height - usersLblTex.height / 2, -usersLblTex.width / 2, usersLblTex.height / 2);
		        usersLabelMap = new short[usersMapSize];
		        usersDepthMap = new short[usersMapSize];
		        usersHistogramMap = new float[8192];
			}
	
			if(ComputeColorMap)
			{
		        // Initialize color map related stuff
		        usersClrSize = KinectWrapper.GetColorWidth() * KinectWrapper.GetColorHeight();
		        usersClrTex = new Texture2D(KinectWrapper.GetColorWidth(), KinectWrapper.GetColorHeight());
		        usersClrColors = new Color32[usersClrSize];
		        usersClrRect = new Rect(Screen.width, Screen.height - usersClrTex.height / 2, -usersClrTex.width / 2, usersClrTex.height / 2);
				
				if(ComputeUserMap)
					usersMapRect.x -= usersClrTex.width / 2;

				usersColorMap = new byte[usersClrSize * 3];
			}
	
	        // Initialize user list to contain ALL users.
	        allUsers = new List<uint>();
	        
	        // Initialize user callbacks.
	        NewUser = new KinectWrapper.UserDelegate(OnNewUser);
	        CalibrationStarted = new KinectWrapper.UserDelegate(OnCalibrationStarted);
	        CalibrationFailed = new KinectWrapper.UserDelegate(OnCalibrationFailed);
	        CalibrationSuccess = new KinectWrapper.UserDelegate(OnCalibrationSuccess);
	        UserLost = new KinectWrapper.UserDelegate(OnUserLost);
			
			// Pull the AvatarController from each of the players Avatars.
			Player1Controllers = new List<AvatarController>();
			Player2Controllers = new List<AvatarController>();
			
			// Add each of the avatars' controllers into a list for each player.
			foreach(GameObject avatar in Player1Avatars)
			{
				Player1Controllers.Add(avatar.GetComponent<AvatarController>());
			}
			
			foreach(GameObject avatar in Player2Avatars)
			{
				Player2Controllers.Add(avatar.GetComponent<AvatarController>());
			}
			
			// GUI Text.
			if(CalibrationText != null)
			{
				CalibrationText.guiText.text = "WAITING FOR USERS";
			}
			
	        // Start looking for users.
	        KinectWrapper.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, UserLost);
			Debug.Log("Waiting for users to calibrate");
			
			// Set the default smoothing for the Kinect.
			KinectWrapper.SetSkeletonSmoothing(0.7f);
			
			instance = this;
			KinectInitialized = true;
		} 
		catch(DllNotFoundException ex)
		{
			Debug.LogError(ex.ToString());
			if(CalibrationText != null)
				CalibrationText.guiText.text = "Please check the OpenNI and NITE installations.";
		}
		catch (Exception ex) 
		{
			Debug.LogError(ex.ToString());
			if(CalibrationText != null)
				CalibrationText.guiText.text = ex.Message;
		}
	}
	
	void Update()
	{
		if(KinectInitialized)
		{
	        // Update to the next frame.
			KinectWrapper.Update();
	
	        // If the players aren't all calibrated yet, draw the user map.
			if(ComputeUserMap)
			{
	        	UpdateUserMap();
			}
			
			if(ComputeColorMap)
			{
	        	UpdateColorMap();
			}
			
			// Update player 1's models if he/she is calibrated and the model is active.
			if(Player1Calibrated)
			{
				foreach (AvatarController controller in Player1Controllers)
				{
					//if(controller.Active)
					{
						controller.UpdateAvatar(Player1ID, NearMode);
					}
				}
			}
			
			// Update player 2's models if he/she is calibrated and the model is active.
			if(Player2Calibrated)
			{
				foreach (AvatarController controller in Player2Controllers)
				{
					//if(controller.Active)
					{
						controller.UpdateAvatar(Player2ID, NearMode);
					}
				}
			}
		}
		
		// Kill the program with ESC.
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	// Make sure to kill the Kinect on quitting.
	void OnApplicationQuit()
	{
		if(KinectInitialized)
		{
			// Shutdown OpenNI
			KinectWrapper.Shutdown();
			instance = null;
		}
	}
	
	// Draw the Histogram Map on the GUI.
    void OnGUI()
    {
		if(KinectInitialized)
		{
	        if(ComputeUserMap && ((allUsers.Count == 0) || DisplayUserMap))
	        {
	            GUI.DrawTexture(usersMapRect, usersLblTex);
	        }
			
	        if(ComputeColorMap && ((allUsers.Count == 0) || DisplayColorMap))
	        {
	            GUI.DrawTexture(usersClrRect, usersClrTex);
	        }
		}
    }
	
	// Update / draw the User Map
    void UpdateUserMap()
    {
		IntPtr pLabelMap = KinectWrapper.GetUsersLabelMap();
		IntPtr pDepthMap = KinectWrapper.GetUsersDepthMap();
		
		if(pLabelMap == IntPtr.Zero || pDepthMap == IntPtr.Zero)
			return;
		
        // copy over the maps
        Marshal.Copy(pLabelMap, usersLabelMap, 0, usersMapSize);
        Marshal.Copy(pDepthMap, usersDepthMap, 0, usersMapSize);

        // Flip the texture as we convert label map to color array
        int flipIndex, i;
        int numOfPoints = 0;
		Array.Clear(usersHistogramMap, 0, usersHistogramMap.Length);

        // Calculate cumulative histogram for depth
        for (i = 0; i < usersMapSize; i++)
        {
            // Only calculate for depth that contains users
            if (usersLabelMap[i] != 0)
            {
                usersHistogramMap[usersDepthMap[i]]++;
                numOfPoints++;
            }
        }
		
        if (numOfPoints > 0)
        {
            for (i = 1; i < usersHistogramMap.Length; i++)
	        {   
		        usersHistogramMap[i] += usersHistogramMap[i-1];
	        }
            for (i = 0; i < usersHistogramMap.Length; i++)
	        {
                usersHistogramMap[i] = 1.0f - (usersHistogramMap[i] / numOfPoints);
	        }
        }

        // Create the actual users texture based on label map and depth histogram
        for (i = 0; i < usersMapSize; i++)
        {
            flipIndex = usersMapSize - i - 1;
			
            if (usersLabelMap[i] == 0)
            {
                usersMapColors[flipIndex] = Color.clear;
            }
            else
            {
                // Create a blending color based on the depth histogram
				float histVal = usersHistogramMap[usersDepthMap[i]];
                Color c = new Color(histVal, histVal, histVal, 0.9f);
				
                switch (usersLabelMap[i] % 4)
                {
                    case 0:
                        usersMapColors[flipIndex] = Color.red * c;
                        break;
                    case 1:
                        usersMapColors[flipIndex] = Color.green * c;
                        break;
                    case 2:
                        usersMapColors[flipIndex] = Color.blue * c;
                        break;
                    case 3:
                        usersMapColors[flipIndex] = Color.magenta * c;
                        break;
                }
            }
        }
		
		// Draw it!
        usersLblTex.SetPixels(usersMapColors);
        usersLblTex.Apply();
    }

	// Update / draw the User Map
    void UpdateColorMap()
    {
		IntPtr pColorMap = KinectWrapper.GetUsersColorMap();
		if(pColorMap == IntPtr.Zero)
			return;
		
        // copy over the map
        Marshal.Copy(pColorMap, usersColorMap, 0, usersClrSize * 3);

        // Flip the texture as we convert color map to color array
        int index = 0, flipIndex;

        // Create the actual users texture based on label map and depth histogram
        for (int i = 0; i < usersClrSize; i++)
        {
            flipIndex = usersClrSize - i - 1;
			
			usersClrColors[flipIndex].r = usersColorMap[index];
			usersClrColors[flipIndex].g = usersColorMap[index + 1];
			usersClrColors[flipIndex].b = usersColorMap[index + 2];
			usersClrColors[flipIndex].a = 230;
			
			index += 3;
        }
		
		// Draw it!
        usersClrTex.SetPixels32(usersClrColors);
        usersClrTex.Apply();
    }

//	// Add model to player list.
//	void AddAvatar(GameObject avatar, List<GameObject> whichPlayerList)
//	{
//		whichPlayerList.Add(avatar);
//	}
//	
//	// Remove model from player list.
//	void RemoveAvatar(GameObject avatar, List<GameObject> whichPlayerList)
//	{
//		whichPlayerList.Remove(avatar);
//	}
	
//	// Functions that let you recalibrate either player 1 or player 2.
//	void RecalibratePlayer1()
//	{
//		OnUserLost(Player1ID);
//	}
//	
//	void RecalibratePlayer2()
//	{
//		OnUserLost(Player2ID);
//	}
	
	// When a new user enters, add it to the list.
	void OnNewUser(uint UserId)
    {
        Debug.Log(String.Format("[{0}] New user", UserId));
        //allUsers.Add(UserId);
    }   
	
	// Print out when the user begins calibration.
    void OnCalibrationStarted(uint UserId)
    {
		Debug.Log(String.Format("[{0}] Calibration started", UserId));
		
		if(CalibrationText != null)
		{
			CalibrationText.guiText.text = "CALIBRATING...\nPLEASE HOLD STILL";
		}
    }
	
	// Alert us when the calibration fails.
    void OnCalibrationFailed(uint UserId)
    {
        Debug.Log(String.Format("[{0}] Calibration failed", UserId));
		
		if(CalibrationText != null)
		{
			CalibrationText.guiText.text = "WAITING FOR USERS";
		}
    }
	
	// If a user successfully calibrates, assign him/her to player 1 or 2.
    void OnCalibrationSuccess(uint UserId)
    {
        Debug.Log(String.Format("[{0}] Calibration success", UserId));
		
		// If player 1 hasn't been calibrated, assign that UserID to it.
		if(!Player1Calibrated)
		{
			// Check to make sure we don't accidentally assign player 2 to player 1.
			if (!allUsers.Contains(UserId))
			{
				Player1Calibrated = true;
				Player1ID = UserId;
				
				allUsers.Add(UserId);
				
				foreach(AvatarController controller in Player1Controllers)
				{
					controller.SuccessfulCalibration(UserId);
				}
				
				// If we're not using 2 users, we're all calibrated.
				//if(!TwoUsers)
				{
					AllPlayersCalibrated = !TwoUsers ? allUsers.Count >= 1 : allUsers.Count >= 2; // true;
				}
			}
		}
		else if(TwoUsers && !Player2Calibrated)
		{
			if (!allUsers.Contains(UserId))
			{
				Player2Calibrated = true;
				Player2ID = UserId;
				
				allUsers.Add(UserId);
				
				foreach(AvatarController controller in Player2Controllers)
				{
					controller.SuccessfulCalibration(UserId);
				}
				
				// All users are calibrated!
				AllPlayersCalibrated = !TwoUsers ? allUsers.Count >= 1 : allUsers.Count >= 2; // true;
			}
		}
		
		// If all users are calibrated, stop trying to find them.
		if(AllPlayersCalibrated)
		{
			Debug.Log("All players calibrated.");
			
			if(CalibrationText != null)
			{
				CalibrationText.guiText.text = "";
			}
			
			KinectWrapper.StopLookingForUsers();
		}
    }
	
	// If a user walks out of the kinects all-seeing eye, try to reassign them! Or, assign a new user to player 1.
    void OnUserLost(uint UserId)
    {
        Debug.Log(String.Format("[{0}] User lost", UserId));
		
		// If we lose player 1...
		if(UserId == Player1ID)
		{
			// Null out the ID and reset all the models associated with that ID.
			Player1ID = 0;
			Player1Calibrated = false;
			
			foreach(AvatarController controller in Player1Controllers)
			{
				controller.RotateToCalibrationPose(UserId, IsCalibrationNeeded());
			}
			
			// Try to replace that user!
			Debug.Log("Starting looking for users");
			KinectWrapper.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, UserLost);
	
			if(CalibrationText != null)
			{
				CalibrationText.guiText.text = "WAITING FOR USERS";
			}
		}
		
		// If we lose player 2...
		if(UserId == Player2ID)
		{
			// Null out the ID and reset all the models associated with that ID.
			Player2ID = 0;
			Player2Calibrated = false;
			
			foreach(AvatarController controller in Player2Controllers)
			{
				controller.RotateToCalibrationPose(UserId, IsCalibrationNeeded());
			}
			
			// Try to replace that user!
			Debug.Log("Starting looking for users");
			KinectWrapper.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, UserLost);
	
			if(CalibrationText != null)
			{
				CalibrationText.guiText.text = "WAITING FOR USERS";
			}
		}

        // remove from global users list
        allUsers.Remove(UserId);
		AllPlayersCalibrated = !TwoUsers ? allUsers.Count >= 1 : allUsers.Count >= 2; // false;
	}
	
	// convert the matrix to quaternion, taking care of the mirroring
	private Quaternion ConvertMatrixToQuat(KinectWrapper.SkeletonJointOrientation ori, int joint, bool flip)
	{
		Matrix4x4 mat = Matrix4x4.identity;
		
		Quaternion quat = new Quaternion(ori.x, ori.y, ori.z, ori.w);
		mat.SetTRS(Vector3.zero, quat, Vector3.one);

		Vector3 vZ = mat.GetColumn(2);
		Vector3 vY = mat.GetColumn(1);
		
		if(!flip)
		{
			vZ.y = -vZ.y;
			vY.x = -vY.x;
			vY.z = -vY.z;
		}
		else
		{
			vZ.x = -vZ.x;
			vZ.y = -vZ.y;
			vY.z = -vY.z;
		}

		if(vZ.x != 0.0f || vZ.y != 0.0f || vZ.z != 0.0f)
		{
			return Quaternion.LookRotation(vZ, vY);
		}

		return Quaternion.identity;
	}
	
}


