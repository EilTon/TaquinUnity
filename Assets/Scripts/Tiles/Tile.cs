using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour, IClick, IDrag<Vector3>, IRelease<Tile>
{


    #region Declarations

    private bool isDrag = false; // check if the tile is drag
    private bool isEmptySpace = false; // check if the tile is the empty space
    private bool canMoveToEmptySpace = false; // check if the tile can move to the empty space
    private new BoxCollider collider;
    private Vector3 currentPosition; // the current position of the tile
    private Vector3 correctPosition; // the correct position of the tile to finish the game
    private RaycastHit hit = new RaycastHit();

    #endregion

    #region Unity Functions 

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        collider = GetComponent<BoxCollider>();
    }

    #endregion

    #region Main Functions 

    //function call when the click on the tile
    public void Click()
    {
        currentPosition = transform.position;
        isDrag = false;
        Debug.Log("click");
        CheckEmptySpace(Vector3.down);
        CheckEmptySpace(Vector3.up);
        CheckEmptySpace(Vector3.left);
        CheckEmptySpace(Vector3.right);
    }

    // function call when the player drag the tile
    public void Drag(Vector3 position)
    {

        transform.position = position;
        if (!isDrag && !isEmptySpace)
        {
            isDrag = true;
        }
        Debug.Log("drag");
    }

    // function call when the player release the tile
    public void Release(Tile tile)
    {
        isDrag = false;
        Debug.Log("release");
        if (tile != null)
        {
            if (tile.GetEmptySpace() && canMoveToEmptySpace)
            {
                transform.position = tile.transform.position;
                tile.transform.position = currentPosition;
                canMoveToEmptySpace = false;
                TileManager.instance.checkIfIsWin();
            }
            else
            {
                ResetTile();
            }
        }

        else
        {
            ResetTile();
        }
    }


    public void SetEmptySpace(bool isEmptySpace)
    {
        this.isEmptySpace = isEmptySpace;
    }

    public bool GetEmptySpace()
    {
        return isEmptySpace;
    }

    //Enable or disable the collider of the tile
    public void SetCollider(bool isEnable)
    {
        collider.enabled = isEnable;
    }

    public void SetCorrectPosition(Vector3 correctPosition)
    {
        this.correctPosition = correctPosition;
    }

    public Vector3 GetCorrectPosition()
    {
        return correctPosition;
    }

    public bool CheckIsPositionEqualToCorrectPosition()
    {
        return transform.position == correctPosition;
    }

    void CheckEmptySpace(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out hit, 10f))
        {
            if (hit.collider.gameObject.GetComponent<Tile>().GetEmptySpace())
            {
                canMoveToEmptySpace = true;
            }
        }
    }

    void ResetTile()
    {
        transform.position = currentPosition;
        canMoveToEmptySpace = false;
    }

    #endregion

}
