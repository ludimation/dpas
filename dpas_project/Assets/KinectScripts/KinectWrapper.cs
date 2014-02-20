using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

// Wrapper class that holds the various structs, variables, and dll imports
// needed to set up a model with the Kinect.
public class KinectWrapper
{
	// Kinect-given Variables to keep track of the skeleton's joints.
	public enum SkeletonJoint
	{ 
		HEAD = 0,
        NECK = 1,
        SPINE = -1,

		LEFT_SHOULDER = 2,
		RIGHT_SHOULDER = 3,
        LEFT_ELBOW = 4,
		RIGHT_ELBOW = 5,
		LEFT_WRIST = -1,
		RIGHT_WRIST = -1,
        LEFT_HAND = 6,
		RIGHT_HAND = 7,

		HIPS = 8,
		
        LEFT_HIP = 9,
        RIGHT_HIP = 10,
        LEFT_KNEE = 11,
		RIGHT_KNEE = 12,
        LEFT_ANKLE = -1,
        RIGHT_ANKLE = -1,
        LEFT_FOOT = 13,
		RIGHT_FOOT = 14,
		
		COUNT 
	};
	
	// Struct to store color RGB888
	public struct ColorRgb888
	{
		public byte r;
		public byte g;
		public byte b;
	}
	
	// Struct to store the joint's poision.
    public struct SkeletonJointPosition
    {
        public float x, y, z;
    }
	
	// Struct that will hold the joints orientation.
    public struct SkeletonJointOrientation
    {
        public float x, y, z, w;
    }
	
	// Struct that combines the previous two and makes the transform.
    public struct SkeletonJointTransformation
    {
		public SkeletonJoint jointType;
        public SkeletonJointPosition position;
        public float positionConfidence;
        public SkeletonJointOrientation orientation;
        public float orientationConfidence;
    }
	
	// DLL Imports to pull in the necessary Unity functions to make the Kinect go.
	[DllImport("UnityInterface2.dll")]
	public static extern int Init(bool isInitDepthStream, bool isInitColorStream);
	[DllImport("UnityInterface2.dll")]
	public static extern void Shutdown();
	[DllImport("UnityInterface2.dll")]
	public static extern int Update();
	
	[DllImport("UnityInterface2.dll")]
	public static extern IntPtr GetLastErrorString();
	[DllImport("UnityInterface2.dll")]
	public static extern int GetDepthWidth();
	[DllImport("UnityInterface2.dll")]
	public static extern int GetDepthHeight();
	[DllImport("UnityInterface2.dll")]
	public static extern int GetColorWidth();
	[DllImport("UnityInterface2.dll")]
	public static extern int GetColorHeight();
	[DllImport("UnityInterface2.dll")]
	public static extern IntPtr GetUsersLabelMap();
    [DllImport("UnityInterface2.dll")]
    public static extern IntPtr GetUsersDepthMap();
    [DllImport("UnityInterface2.dll")]
    public static extern IntPtr GetUsersColorMap();

	[DllImport("UnityInterface2.dll")]
    public static extern void SetSkeletonSmoothing(float factor);

    [DllImport("UnityInterface2.dll")]
    public static extern bool GetJointTransformation(uint userID, int joint, ref SkeletonJointTransformation pTransformation);
    [DllImport("UnityInterface2.dll")]
    public static extern bool GetJointPosition(uint userID, int joint, ref SkeletonJointPosition pTransformation);
    [DllImport("UnityInterface2.dll")]
    public static extern bool GetJointOrientation(uint userID, int joint, ref SkeletonJointOrientation pTransformation);
    [DllImport("UnityInterface2.dll")]
    public static extern float GetJointPositionConfidence(uint userID, int joint);
    [DllImport("UnityInterface2.dll")]
    public static extern float GetJointOrientationConfidence(uint userID, int joint);
	
    [DllImport("UnityInterface2.dll")]
    public static extern void StartLookingForUsers(IntPtr NewUser, IntPtr CalibrationStarted, IntPtr CalibrationFailed, IntPtr CalibrationSuccess, IntPtr UserLost);
    [DllImport("UnityInterface2.dll")]
    public static extern void StopLookingForUsers();

    public delegate void UserDelegate(uint userId);

    public static void StartLookingForUsers(UserDelegate NewUser, UserDelegate CalibrationStarted, UserDelegate CalibrationFailed, UserDelegate CalibrationSuccess, UserDelegate UserLost)
    {
        StartLookingForUsers(
            Marshal.GetFunctionPointerForDelegate(NewUser),
            Marshal.GetFunctionPointerForDelegate(CalibrationStarted),
            Marshal.GetFunctionPointerForDelegate(CalibrationFailed),
            Marshal.GetFunctionPointerForDelegate(CalibrationSuccess),
            Marshal.GetFunctionPointerForDelegate(UserLost)
		);
    }
	
	// copies and configures the needed resources in the project directory
	public static bool CheckOpenNIPresence()
	{
		bool bOneCopied = false, bAllCopied = true;

		// check openni directory and resources
		string sOpenNIPath = System.Environment.GetEnvironmentVariable("OPENNI2_REDIST");
		if(sOpenNIPath == String.Empty || !Directory.Exists(sOpenNIPath))
			throw new Exception("OpenNI directory not found. Please check the OpenNI installation.");
		
		sOpenNIPath = sOpenNIPath.Replace('\\', '/');
		if(sOpenNIPath.EndsWith("/"))
			sOpenNIPath = sOpenNIPath.Substring(0, sOpenNIPath.Length - 1);
		
		if(!File.Exists("OpenNI2.dll"))
		{
			string srcOpenNiDll = sOpenNIPath + "/OpenNI2.dll";
			if(!File.Exists(srcOpenNiDll))
				throw new Exception("OpenNI library not found. Please check the OpenNI installation.");
			
			Debug.Log("Copying OpenNI library...");
			File.Copy(srcOpenNiDll, "OpenNI2.dll");
				
			bOneCopied = File.Exists("OpenNI2.dll");
			bAllCopied = bAllCopied && bOneCopied;
		}
		
		if(!File.Exists("OpenNI.ini"))
		{
			Debug.Log("Copying OpenNI configuration...");
			TextAsset textRes = Resources.Load("OpenNI.ini", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				string sResText = textRes.text.Replace("%OPENNI_REDIST_DIR%", sOpenNIPath);
				File.WriteAllText("OpenNI.ini", sResText);
				
				bOneCopied = File.Exists("OpenNI.ini");
				bAllCopied = bAllCopied && bOneCopied;
			}
		}

		// check nite directory and resources
		string sNiTEPath = System.Environment.GetEnvironmentVariable("NITE2_REDIST");
		Debug.Log("sNiTEPath = "+sNiTEPath);
		if(sNiTEPath == String.Empty || !Directory.Exists(sNiTEPath))
			throw new Exception("NiTE directory not found. Please check the NiTE installation.");
		
		sNiTEPath = sNiTEPath.Replace('\\', '/');
		if(sNiTEPath.EndsWith("/"))
			sNiTEPath = sNiTEPath.Substring(0, sNiTEPath.Length - 1);
		
		if(!File.Exists("NiTE2.dll"))
		{
			string srcNiteDll = sNiTEPath + "/NiTE2.dll";
			if(!File.Exists(srcNiteDll))
				throw new Exception("NiTE library not found. Please check the NiTE installation.");
			
			Debug.Log("Copying NiTE library...");
			File.Copy(srcNiteDll, "NiTE2.dll");
				
			bOneCopied = File.Exists("NiTE2.dll");
			bAllCopied = bAllCopied && bOneCopied;
		}
		
		if(!File.Exists("NiTE.ini"))
		{
			Debug.Log("Copying NiTE configuration...");
			TextAsset textRes = Resources.Load("NiTE.ini", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				string sResText = textRes.text.Replace("%NITE_REDIST_DIR%", sNiTEPath);
				File.WriteAllText("NiTE.ini", sResText);
				
				bOneCopied = File.Exists("NiTE.ini");
				bAllCopied = bAllCopied && bOneCopied;
			}
		}

		// check the unity interface library
		if(!File.Exists("UnityInterface2.dll"))
		{
			Debug.Log("Copying UnityInterface library...");
			TextAsset textRes = Resources.Load("UnityInterface2.dll", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				File.WriteAllBytes("UnityInterface2.dll", textRes.bytes);
				
				bOneCopied = File.Exists("UnityInterface2.dll");
				bAllCopied = bAllCopied && bOneCopied;
			}
		}

		return bOneCopied && bAllCopied;
	}
	
}

