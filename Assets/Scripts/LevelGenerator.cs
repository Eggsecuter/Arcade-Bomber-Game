using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public const float TileSize = .4f;
    public const int Columns = 3, Rows = 5, MaxStraightPath = 1;

    [SerializeField]
    private GameObject _wall = null;
    [SerializeField]
    private GameObject _path = null;

    private GameObject[,] _level = new GameObject[Columns, Rows];
    // Defines rules for next line
    private int _countStraightPath = 0;
    private int _lastRowEnd = 1;
    private bool _rightAllowed = false;
    private bool _leftAllowed = false;

    public RowMovement NextRow()
    {
        // Destroy bottom row
        for (int column = 0; column < Columns; column++)
        {
            Destroy(_level[column, Rows - 1]);
        }

        // Replace middle rows
        for (int row = Rows; row --> 1;)
        {
            for (int column = 0; column < Columns; column++)
            {
                GameObject tileAbove = _level[column, row - 1];

                if (tileAbove)
                {
                    _level[column, row] = tileAbove;
                    tileAbove.transform.position += new Vector3(0, -TileSize);
                }
            }
        }

        // Add top row
        var movement = GetRowMovement();
        CreateNextRow(movement);

        return movement;
    }

    /// <summary>
    /// Gets the movement of the next line considering the rules
    /// </summary>
    /// <returns>Row movement with direction and distance</returns>
    private RowMovement GetRowMovement()
    {
        var movement = new RowMovement
        {
            Distance = 0,
            HeadRight = false
        };

        if (_rightAllowed || _leftAllowed)
        {
            // If both direcitons choose one else the only possible
            movement.HeadRight = _rightAllowed && _leftAllowed ?
                Random.Range(0, 2) == 0 : _rightAllowed;

            // Choose a random distance
            int distanceMin = _countStraightPath >= MaxStraightPath ? 1 : 0;
            int distanceMax = movement.HeadRight ? Columns - _lastRowEnd : _lastRowEnd + 1;
            movement.Distance = Random.Range(distanceMin, distanceMax);
        }

        if (movement.Distance == 0)
        {
            _countStraightPath++;
        }
        else
        {
            _countStraightPath = 0;
        }

        return movement;
    }

    /// <summary>
    /// Instantiates all tiles and updates level
    /// </summary>
    private void CreateNextRow(RowMovement movement)
    {
        int pathStartIndex = _lastRowEnd;

        if (!movement.HeadRight)
            pathStartIndex -= movement.Distance;

        int pathEndIndex = pathStartIndex + movement.Distance;

        for (int column = 0; column < Columns; column++)
        {
            if (column < pathStartIndex || column > pathEndIndex)
            {
                // Place Wall
                PlaceTile(_wall, column);
            }
            else
            {
                // Place Path
                PlaceTile(_path, column);
            }
        }

        // Set rules for next line
        if (movement.HeadRight)
        {
            _lastRowEnd += movement.Distance;
        }
        else
        {
            _lastRowEnd -= movement.Distance;
        }

        _rightAllowed = Columns > _lastRowEnd + 1 && movement.Distance == 0;
        _leftAllowed = _lastRowEnd != 0 && movement.Distance == 0;
    }

    private void PlaceTile(GameObject tile, int column)
    {
        Vector3 position = transform.position + new Vector3(column * TileSize, 0);
        _level[column, 0] = Instantiate(tile, position, Quaternion.identity, transform);
    }
}
