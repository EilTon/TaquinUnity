using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour, IClick, IDrag<Vector3>, IRelease
{


    #region Declarations
    
    private bool isDrag = false;
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        isDrag = false;
        Debug.Log("click");
    }

    public void Drag(Vector3 position)
    {
  
        transform.position = position;
        if(!isDrag)
        {
            isDrag = true;
        }
        Debug.Log("drag");
    }

    public void Release()
    {
        isDrag = false;
        Debug.Log("release");
    }
}
