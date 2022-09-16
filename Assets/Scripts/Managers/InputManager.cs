using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Declarations
    public static InputManager instance;
    bool isTouch = false;
    Vector3 touchPosition;
    Camera cameraInteraction;
    Ray ray;
    RaycastHit hit = new RaycastHit();
    Tile currentTile;
    IClick currentClickObject;
    IDrag<Vector3> currentDragObject;
    IRelease currentReleaseObject;
    #endregion

    void Awake()
    {
        CreateSingleton();
        cameraInteraction = Camera.main;
    }

    void Update()
    {
        GetInputs();
        Click();
        Drag();
        Release();
    
    }

    #region Movement Functions


    private void Click()
    {
        if (isTouch && currentTile == null)
        {
            ray = ScreenCast();
            if(Physics.Raycast(ray,out hit,100f))
            {
                currentTile = hit.collider.gameObject.GetComponent<Tile>();
                if(currentTile != null && currentTile is IClick)
                {
                    currentClickObject = (IClick)currentTile;
                    currentClickObject.Click();
                }
            }
        }
    }

    private void Drag()
    {
        
        if (isTouch)
        {
            if(currentTile!=null)
            {
                if(currentTile is IDrag<Vector3>)
                {
                    
                    currentDragObject = (IDrag<Vector3>)currentTile;
                    ray=ScreenCast();
                    currentDragObject.Drag(ray.origin + ray.direction);
                }
            }
        }
    }

    private void Release()
    {
        if (!isTouch)
        {
            if(currentTile != null)
            {
                if(currentTile is IRelease)
                {
                    ray = ScreenCast();
                    currentReleaseObject = (IRelease)currentTile;
                    currentReleaseObject.Release();
                    currentTile = null;
                    currentReleaseObject = null;
                }
            }
        }
    }

    #endregion

    #region System Functions

    private void GetInputs()
    {
        isTouch = Input.GetMouseButton(0);
        touchPosition = Input.mousePosition;
    }

    private void CreateSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private Ray ScreenCast()
    {
        return cameraInteraction.ScreenPointToRay(touchPosition);
    }

    #endregion


}
