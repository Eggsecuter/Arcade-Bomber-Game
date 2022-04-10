using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LevelGenerator generator;

    private readonly Queue<RowMovement> _movements = new Queue<RowMovement>();
    private RowMovement _movement;

    private void Start()
    {
        // Initialize level
        for (int i = 0; i < LevelGenerator.Rows; i++)
        {
            MoveUp();
        }

        _movement = _movements.Dequeue();

        // Start Interaction
        InteractionController.Instance.Action += UpdatePosition;
    }

    private void OnDestroy()
    {
        InteractionController.Instance.Action -= UpdatePosition;
    }

    private void UpdatePosition()
    {
        if (_movement.Distance == 0)
        {
            MoveUp();

            _movement = _movements.Dequeue();
        }
        else
        {
            int direction = _movement.HeadRight ? 1 : -1;
            transform.position += new Vector3(direction * LevelGenerator.TileSize, 0);

            _movement.Distance -= 1;
        }
    }

    private void MoveUp()
    {
        _movements.Enqueue(
            generator.NextRow()
        );
    }
}
