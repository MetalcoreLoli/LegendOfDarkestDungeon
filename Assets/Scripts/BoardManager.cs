using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    private Transform boardHolder;

    public GameObject Exit;
    public GameObject FloorTile;
    public GameObject WallTileHorizontal;
    public GameObject WallTileVertical;
    public GameObject WallUpLeftCornerTile;
    public GameObject WallUpRightCornerTile;
    public GameObject WallDownLeftCornerTile;
    public GameObject WallDownRightCornerTile;
    public void SetUpLevel(int level) 
    {
        BoardSetUp();
    }

    private void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;

      
        for (int x = 0; x < 8 + 1; x++)
        {
            for (int y = 0; y < 8 + 1; y++)
            {
                Instantiate(FloorTile, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder); 
                if (x == 0 && y == 0)
                    Instantiate(WallDownLeftCornerTile, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if (x == 8 && y == 0)
                    Instantiate(WallDownRightCornerTile, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if (x == 0 && y == 8)
                    Instantiate(WallUpLeftCornerTile, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if (x == 8 && y == 8)
                    Instantiate(WallUpRightCornerTile, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if ((x == 8 && y > 0 && y < 8))
                    Instantiate(WallTileVertical, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if ((y == 8 && x > 0 && x < 8))
                    Instantiate(WallTileHorizontal, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);
             
                if (x > 0 && x < 8 && y == 0)
                    Instantiate(WallTileHorizontal, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

                if (y > 0 && y < 8 && x == 0)
                    Instantiate(WallTileVertical, new Vector3(x, y, 0), Quaternion.identity).transform.SetParent(boardHolder);

            }
        }


        Instantiate(Exit, new Vector3(7, 7, 0), Quaternion.identity).transform.SetParent(boardHolder);
    }
}
