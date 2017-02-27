﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

namespace UWBNetworkingPackage.ViveDemo
{
    /// <summary>
    /// A script that is used for Vive Avatar instantiation/tracking. This script instantiates the 
    /// user specified "ViveRightHandObject" prefab (must be located in the Photon Resources folder), 
    /// and matches its transform and rotation to the Vive Players Camera Rig right controller. 
    /// </summary>
    public class ViveRightHandTracking : PunBehaviour
    {
        public GameObject ViveRightHandObject;  // Object to instantiate
                                                // Must be in Photon Resources folder

        private GameObject _cameraRig;          // Reference to the Camera Rig 
                                                // (A Vive prefab provided by SteamVR)
        private GameObject _rightHand;          // Reference to the instance of the instantiated "ViveRightHandObject"
        private Transform _cameraRigRightHand;  // Reference to the transform of the Camera Rig's right controller
        private bool _instantiated = false;     // For determining if the "ViveRightHandObject" was instantiated

        /// <summary>
        /// On start, get reference to the Camera Rig and the Camera Rig's right controller's transform (for tracking) 
        /// </summary>
        private void Start()
        {
            try
            {
                _cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
                _cameraRigRightHand = _cameraRig.transform.GetChild(1);
            }
            catch (Exception e)
            {
                Debug.Log("Could not find a 'CameraRig' tagged object: " + e.StackTrace);
            }
        }

        /// <summary>
        /// On joined room, instantiate a "ViveRightHandObject" and store reference to the instance that was 
        /// instantiated
        /// </summary>
        public override void OnJoinedRoom()
        {
            _rightHand = PhotonNetwork.Instantiate(ViveRightHandObject.name, Vector3.zero, Quaternion.identity, 0);
            _instantiated = true;
        }

        /// <summary>
        /// Called once per frame, if a "ViveRightHandObject" has been properly instantiated, 
        /// then match its postion and transform to the Camera Rig's right controller's transform
        /// </summary>
        private void Update()
        {
            if (!_instantiated || _cameraRig == null) return;
            _rightHand.transform.position = _cameraRigRightHand.transform.position;
            Quaternion rot = _cameraRigRightHand.transform.rotation;
            rot.eulerAngles = rot.eulerAngles + new Vector3(0, 180, 0);
            _rightHand.transform.rotation = rot;
        }
    }
}
