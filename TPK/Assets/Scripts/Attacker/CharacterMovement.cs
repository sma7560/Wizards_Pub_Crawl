using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encapsulates the hero character movement logic.
/// This class is needed to isolate the logic for unit testing without any Unity dependencies.
/// </summary>
public class CharacterMovement
{
    private float speed;

    public CharacterMovement(float speed)
    {
        if (speed > 0)
        {
            this.speed = speed;
        }
        else
        {
            this.speed = 0;
        }
    }

    public Vector3 Calculate(float x, float z)
    {
        if (x != 0 || z != 0)
        {
            return new Vector3(x * speed, 0, z * speed);
        }

        return new Vector3(0, 0, 0);
    }

    public float GetSpeed()
    {
        return speed;
    }
}
