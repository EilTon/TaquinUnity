using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tile;
    [SerializeField]
    private List<GameObject> tiles = new List<GameObject>();
    int[] test = new int[9];
    private float x = 0;
    private float y = 0;

    void Start()
    {
        CreateAllTiles();
        Shuffle();
        while (!IsItSolvable())
        {
            Shuffle();
        }
        Debug.Log(IsItSolvable());

    }

    private void CreateAllTiles()
    {

        for (int i = 0; i < 9; i++)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                CreateTile("Android/" + (i + 1), i);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                CreateTile("Apple/" + (i + 1), i);
            }
            else
            {
                CreateTile("Android/" + (i + 1), i);
            }
        }

        GameObject emptySpace = tiles[Mathf.CeilToInt(tiles.Count / 2)].gameObject;
        emptySpace.name = "EmptySpace";
        emptySpace.GetComponent<SpriteRenderer>().sprite = null;
        emptySpace.GetComponent<Tile>().SetEmptySpace(true);


    }

    private void CreateTile(string path, int countLoop)
    {
        GameObject newTile = Object.Instantiate(tile);
        newTile.name = "Tile" + countLoop;
        newTile.transform.SetParent(this.transform);
        newTile.transform.position = new Vector3(x, y, 0);
        newTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
        if (x == 10.24f)
        {
            x = 0;
            y = y - 5.12f;
        }
        else
        {
            x = x + 5.12f;
        }
        tiles.Add(newTile);
    }

    void Shuffle()
    {
        for (int i = 0; i < 9; i++)
        {
            if (tiles[i].GetComponent<Tile>().GetEmptySpace() == false)
            {
                int randomIndex = Random.Range(0, 9);
                Vector3 lastPosition = tiles[i].transform.position;
                tiles[i].transform.position = tiles[randomIndex].transform.position;
                tiles[randomIndex].transform.position = lastPosition;

                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;

                test[i] = randomIndex;
            }
        }
    }

    bool IsItSolvable()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (test[i] > test[j])
                {
                    count++;
                }

            }
        }
        Debug.Log(count);
        return count % 2 == 0;
    }
}
