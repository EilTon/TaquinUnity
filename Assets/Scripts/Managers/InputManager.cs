using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script handle input from player
public class InputManager : MonoBehaviour
{
    #region Declarations
    public Camera cameraInteraction; // camera where the player play
    public float dragDistanceFromCamera = 4; //offset when the player grab a tile
    private bool isTouch = false; // boolean if the player touch or not
    private Vector3 touchPosition; // position where the player touch
    private Ray ray; // ray convert from screenPoint 
    private RaycastHit hit = new RaycastHit(); // what the player hit with the ray
    private Tile currentTile, emptyTile; // tile the play has grab and a ref from the emptyTile
    private IClick currentClickObject;
    private IDrag<Vector3> currentDragObject;
    private IRelease<Tile> currentReleaseObject;
    #endregion

    #region Unity Functions 

    void Update()
    {
        GetInputs();
        Click();
        Drag();
        Release();

    }

    #endregion

    #region Movement Functions

    //when the player touch
    private void Click()
    {
        if (isTouch && currentTile == null)
        {
            ray = ScreenCast();
            if (Physics.Raycast(ray, out hit, 100f))
            {
                currentTile = hit.collider.gameObject.GetComponent<Tile>();
                if (currentTile != null && currentTile is IClick)
                {
                    currentTile.SetCollider(false);
                    currentClickObject = (IClick)currentTile;
                    currentClickObject.Click();
                }
            }
        }
    }
    //when the player drag is touch
    private void Drag()
    {

        if (isTouch)
        {
            if (currentTile != null)
            {
                if (currentTile is IDrag<Vector3>)
                {

                    currentDragObject = (IDrag<Vector3>)currentTile;
                    ray = ScreenCast();
                    currentDragObject.Drag(ray.origin + ray.direction * dragDistanceFromCamera);
                }
            }
        }
    }

    //when the player release is touch
    private void Release()
    {
        if (!isTouch)
        {
            if (currentTile != null)
            {
                if (currentTile is IRelease<Tile>)
                {
                    ray = ScreenCast();
                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider.gameObject.GetComponent<Tile>().GetEmptySpace())
                        {
                            emptyTile = hit.collider.gameObject.GetComponent<Tile>();
                        }
                    }

                    currentReleaseObject = (IRelease<Tile>)currentTile;
                    currentTile.SetCollider(true);
                    currentReleaseObject.Release(emptyTile);

                    currentTile = null;
                    currentReleaseObject = null;
                    emptyTile = null;
                }
            }
        }
    }

    #endregion

    #region System Functions

    // function get input from player
    private void GetInputs()
    {
        isTouch = Input.GetMouseButton(0);
        touchPosition = Input.mousePosition;
    }

    //convert where the player touch into a ray
    private Ray ScreenCast()
    {
        return cameraInteraction.ScreenPointToRay(touchPosition);
    }

    #endregion


}
