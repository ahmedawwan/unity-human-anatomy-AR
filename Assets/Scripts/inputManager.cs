using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
public class inputManager : ARBaseGestureInteractable
{
    [SerializeField] public Camera arCam;            //Our Main Camera
    public GameObject crosshair;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Pose pose;
    GameObject placedOBJ;   
    [SerializeField] GameObject btnDelete;
    private bool isObjectPlaced; 

    void Start()
    {
        isObjectPlaced = false;
    }


    
    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData,results);
        return results.Count > 0;
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if(gesture.targetObject == null)
            return true;
        return false;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if(gesture.isCanceled)
        return;
        if(gesture.targetObject != null && IsPointerOverUI(gesture))
        {
            return;
        }
        if(GestureTransformationUtility.Raycast(gesture.startPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            if(isObjectPlaced == false)
            {
                spawnObject();
            }
            else
            {
                deleteObject();
                spawnObject();
            }
        }

    }

    public void deleteObject()
    {
        placedOBJ.SetActive(false);
        btnDelete.SetActive(false);
    }

    private void spawnObject()
    {
        placedOBJ = Instantiate(dataHandler.Instance.GetAnatomy(), pose.position, pose.rotation);
        isObjectPlaced = true;
        btnDelete.SetActive(true);
        var anchorObject = new GameObject("PlacementAnchor");
        anchorObject.transform.position = pose.position;
        anchorObject.transform.rotation = pose.rotation;
        placedOBJ.transform.parent = anchorObject.transform;

    }
    private bool placementPoseIsValid = false;

    void FixedUpdate()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }
    
    private void UpdatePlacementPose()
	{
        Vector3 orgin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        if(GestureTransformationUtility.Raycast(orgin, _hits, TrackableType.PlaneWithinPolygon))
        {
            placementPoseIsValid = _hits.Count > 0;
            if(placementPoseIsValid)
            {
                pose = _hits[0].pose;
                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                pose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }
private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            crosshair.SetActive(true);
            crosshair.transform.SetPositionAndRotation(pose.position, pose.rotation);
            crosshair.transform.eulerAngles = new Vector3(90,0,0);
        }
        else
        {
            crosshair.SetActive(false);
        }
	}
}
