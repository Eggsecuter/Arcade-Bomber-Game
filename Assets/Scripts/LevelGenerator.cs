using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float _tileSize = .4f;
    private const int _columns = 3, _rows = 5, _maxStraightPath = 2;

    [SerializeField]
    private GameObject _wall = null;
    [SerializeField]
    private GameObject _path = null;

    private GameObject[,] _level = new GameObject[_columns, _rows];

    // Defines rules for next line
    private int _countStraightPath = 0;
    private int _lastLineEnd = 1;
    private bool _rightAllowed = false;
    private bool _leftAllowed = false;

    private void Start()
    {
        // Initialize level
        for (int i = 0; i < _rows; i++)
        {
            GenerateNextLine();
        }

        // Start interaction
        PlayerController.Instance.Action += GenerateNextLine;
    }

    private void OnDestroy()
    {
        PlayerController.Instance.Action -= GenerateNextLine;
    }

    private void GenerateNextLine()
    {
        // Destroy bottom row
        for (int column = 0; column < _columns; column++)
        {
            Destroy(_level[column, _rows - 1]);
        }

        // Replace middle rows
        for (int row = _rows; row --> 1;)
        {
            for (int column = 0; column < _columns; column++)
            {
                GameObject tileAbove = _level[column, row - 1];

                if (tileAbove)
                {
                    _level[column, row] = tileAbove;
                    tileAbove.transform.position += new Vector3(0, -_tileSize, 0);
                }
            }
        }

        // Add top row
        int[] line = GetNextLine();

        for (int column = 0; column < _columns; column++)
        {
            GameObject tile = line[column] == 0 ? _path : _wall;
            Vector2 position = (Vector2)transform.position + new Vector2(column * _tileSize, 0);
            _level[column, 0] = Instantiate(tile, position, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// Returns the next line (0 is path, 1 is wall)
    /// </summary>
    /// <returns>List of ints</returns>
    private int[] GetNextLine()
    {
        // Set wall tiles as defaults
        int[] line = Enumerable.Repeat(1, _columns).ToArray();

        if (!_rightAllowed && !_leftAllowed)
        {
            line[_lastLineEnd] = 0;

            _countStraightPath += 1;
            _rightAllowed = _columns > _lastLineEnd + 1;
            _leftAllowed = _lastLineEnd != 0;
        }
        else
        {
            // If both direcitons choose one else the only possible
            bool moveRight = _rightAllowed && _leftAllowed ?
                Random.Range(0, 2) == 0 : _rightAllowed;

            // Choose a random distance
            int distanceMin = _countStraightPath >= _maxStraightPath ? 1 : 0;
            int distanceMax = moveRight ? _columns - _lastLineEnd : _lastLineEnd + 1;
            int distance = Random.Range(distanceMin, distanceMax);

            _countStraightPath = distance == 0 ? _countStraightPath + 1 : 0;
            
            // Set path tiles
            for (int i = 0; i <= distance; i++)
            {
                int element = _lastLineEnd + i;

                if (!moveRight)
                    element -= distance;

                line[element] = 0;
            }

            // Set rules for next line
            if (moveRight)
            {
                _lastLineEnd += distance;
                _leftAllowed = _lastLineEnd != 0 && distance == 0;
                _rightAllowed = _columns > _lastLineEnd + 1;
            }
            else
            {
                _lastLineEnd -= distance;
                _rightAllowed = _columns > _lastLineEnd + 1 && distance == 0;
                _leftAllowed = _lastLineEnd != 0;
            }
        }

        return line;
    }
}
