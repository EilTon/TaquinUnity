using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{

    #region Declarations 

    public static TileManager instance; // singleton
    public GameObject tile; //Prefab of a tile
    private List<GameObject> tiles = new List<GameObject>(); // list of tiles
    private int[] testSolvable = new int[9]; // to check if the puzzle is solvable
    private float x1 = 0;
    private float y1 = 0;

    #endregion

    #region Unity Functions 

    void Awake()
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

    void Start()
    {
        CreateAllTiles();
        Shuffle();
        while (!IsItSolvable())
        {
            Shuffle();
        }
    }
    private void Update()
    {
        WinCheat();
    }

    #endregion

    #region Main Functions 

    //Function to create,Set all tiles and create the empty one
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
            else if (Application.isEditor)
            {
                CreateTile("Apple/" + (i + 1), i);
            }
        }

        GameObject emptySpace = tiles[Mathf.CeilToInt(tiles.Count / 2)].gameObject;
        emptySpace.name = "EmptySpace";
        emptySpace.GetComponent<SpriteRenderer>().sprite = null;
        emptySpace.GetComponent<Tile>().SetEmptySpace(true);
    }


    //function to create a Tile
    private void CreateTile(string path, int countLoop)
    {
        GameObject newTile = Object.Instantiate(tile);
        newTile.name = "Tile" + countLoop;
        newTile.transform.SetParent(this.transform);
        newTile.transform.position = new Vector3(x1, y1, 0);
        newTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
        if (x1 == 10.24f)
        {
            x1 = 0;
            y1 = y1 - 5.12f;
        }
        else
        {
            x1 = x1 + 5.12f;
        }
        newTile.GetComponent<Tile>().SetCorrectPosition(newTile.transform.position);
        tiles.Add(newTile);
    }


    //function to shuffle
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

                testSolvable[i] = randomIndex;
            }
        }
    }


    // function to check if the puzzle is solvable
    bool IsItSolvable()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (testSolvable[i] > testSolvable[j])
                {
                    count++;
                }

            }
        }
        return count % 2 == 0;
    }

    // check if the puzzle is finish
    public void checkIfIsWin()
    {
        bool isWin = true;
        foreach (var tile in tiles)
        {
            Tile currentTile = tile.GetComponent<Tile>();
            if (!currentTile.CheckIsPositionEqualToCorrectPosition())
            {
                isWin = false;
                break;
            }
        }
        if (isWin)
        {
            Debug.Log("you win");
            ScoreManager.instance.SetIsTimerOn(false);
            ScoreManager.instance.LoadSceneHome();
        }

    }

    // just a cheat for the dev :)
    void WinCheat()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var tile in tiles)
            {
                tile.transform.position = tile.GetComponent<Tile>().GetCorrectPosition();
            }
            checkIfIsWin();
        }
    }

    #endregion
}
