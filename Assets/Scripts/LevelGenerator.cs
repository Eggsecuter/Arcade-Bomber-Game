using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public Transform tileParent;
    public int Columns = 3, Rows = 5, MaxStraightPath = 1;

    [HideInInspector] public int pathTilesToPlayer = 0;

    [SerializeField] private Player _player = null;
    [SerializeField] private GameObject _wall = null;
    [SerializeField] private GameObject _path = null;
    [SerializeField] private GameObject _bomb = null;
    [SerializeField] private BombType[] _bombTypes = null;

    private RectTransform[,] _level = null;
    // Rules for next line
    private int _countStraightPath = 0;
    private int _lastRowEnd = 1;
    private bool _rightAllowed = false;
    private bool _leftAllowed = false;

    private void Awake()
    {
        _level = new RectTransform[Columns, Rows];
    }

    public RowMovement NextRow()
    {
        // Destroy bottom row
        for (int column = 0; column < Columns; column++)
        {
            var gameObject = _level[column, Rows - 1]?.gameObject;
            if (gameObject != null)
                Destroy(gameObject);
        }

        // Replace middle rows
        for (int row = Rows; row --> 1;)
        {
            for (int column = 0; column < Columns; column++)
            {
                RectTransform tileAbove = _level[column, row - 1];

                if (tileAbove)
                {
                    _level[column, row] = tileAbove;
                    tileAbove.pivot -= new Vector2(0, 1f / (Rows -1));
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
        // Set rules for next line
        if (movement.HeadRight)
        {
            for (int column = 0; column < Columns; column++)
            {
                bool isPath = column >= _lastRowEnd && column <= _lastRowEnd + movement.Distance;

                PlaceTile(isPath, column);
            }

            _lastRowEnd += movement.Distance;
        }
        else
        {
            for (int column = Columns; column--> 0;)
            {
                bool isPath = column <= _lastRowEnd && column >= _lastRowEnd - movement.Distance;

                PlaceTile(isPath, column);
            }

            _lastRowEnd -= movement.Distance;
        }

        _rightAllowed = Columns > _lastRowEnd + 1 && movement.Distance == 0;
        _leftAllowed = _lastRowEnd != 0 && movement.Distance == 0;
    }

    private void PlaceTile(bool isPath, int column)
    {
        var tile = isPath ? _path : _wall;

        _level[column, 0] = (RectTransform)Instantiate(tile, tileParent).transform;
        _level[column, 0].pivot = new Vector2(1f / (Columns -1) * column, 1);

        if (isPath)
        {
            if (pathTilesToPlayer >= 2)
            {
                var bombType = GetBombType();

                if (!(bombType is null))
                {
                    var bomb = Instantiate(_bomb, _level[column, 0].transform);
                    bomb.GetComponent<Bomb>().Initialize(_player, pathTilesToPlayer, bombType.GetInstance());
                }
            }

            pathTilesToPlayer++;
        }
    }

    private BombType GetBombType()
    {
        int index = Random.Range(0, 101);

        foreach (BombType type in _bombTypes)
        {
            index -= type.SpawnProbability;

            if (index <= 0)
                return type;
        }

        return null; // place empty path
    }
}
