using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class zoomManager : MonoBehaviour
{
   private GraphicRaycaster _raycaster;
   public Transform selectionPoint;
   private PointerEventData pData;
   private EventSystem eventSystem;
   private static zoomManager instance;
   public static zoomManager Instance 
   { 
       get {
           if (instance == null)
           {
               instance = FindObjectOfType<zoomManager>();
           }
           return instance;
       } 
   }
   
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        _raycaster = GetComponent<GraphicRaycaster>();

        //Fetch the Event System from the Scene
        eventSystem = GetComponent<EventSystem>();

        //Set up the new Pointer Event
        pData = new PointerEventData(eventSystem);

        //Set the Pointer Event Position to that of the mouse position
        pData.position = selectionPoint.position;
    }

    void Update()
    {

    }

    public bool onEntered(GameObject button){
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        
        //Raycast using the Graphics Raycaster and mouse click position
        _raycaster.Raycast(pData, results);
        
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach ( RaycastResult result in results){
            if(result.gameObject == button){
                return true;
            }
        }
        return false;
    }

   
}
