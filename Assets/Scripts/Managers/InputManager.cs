using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Declarations
    public static InputManager instance;
    public float dragDistanceFromCamera = 4;
    private bool isTouch = false;
    private Vector3 touchPosition;
    private Camera cameraInteraction;
    private Ray ray;
    private RaycastHit hit = new RaycastHit();
    private Tile currentTile, emptyTile;
    private IClick currentClickObject;
    private IDrag<Vector3> currentDragObject;
    private IRelease<Tile> currentReleaseObject;
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
