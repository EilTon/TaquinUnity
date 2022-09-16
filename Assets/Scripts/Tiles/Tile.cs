using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour, IClick, IDrag<Vector3>, IRelease<Tile>
{


    #region Declarations

    private bool isDrag = false;
    private bool isEmptySpace = false;
    private bool canMoveToEmptySpace = false;
    private new BoxCollider collider;
    private Vector3 currentPosition;
    private RaycastHit hit = new RaycastHit();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public void Drag(Vector3 position)
    {

        transform.position = position;
        if (!isDrag && !isEmptySpace)
        {
            isDrag = true;
        }
        Debug.Log("drag");
    }

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

    public void SetCollider(bool isEnable)
    {
        collider.enabled = isEnable;
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
}
