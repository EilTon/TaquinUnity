using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tile;
    List<GameObject> tiles = new List<GameObject>();

    void Start()
    {
        CreateAllTiles();
    }

    private void CreateAllTiles()
    {

        for (int i = 0; i < 9; i++)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                CreateTile("Android/" + (i + 1));
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                CreateTile("Apple/" + (i + 1));
            }
            else
            {
                CreateTile("Android/" + (i + 1));
            }
        }

        Destroy(tiles[Mathf.CeilToInt(tiles.Count/2)]);    
    
        }

    private void CreateTile(string path)
    {
        GameObject newTile = Object.Instantiate(tile);
        newTile.transform.SetParent(this.transform);
        //newTile.transform.position = ;
        newTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
        tiles.Add(newTile);
    }
}
