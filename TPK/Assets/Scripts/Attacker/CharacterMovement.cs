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

    // Mathematically speaking it is now correct.
    public Vector3 Calculate(float x, float z)
    {
        if (x != 0 || z != 0)
        {
            float mag = Mathf.Sqrt((x * x) + (z * z));
            Vector3 dir = new Vector3(x/mag, 0, z/mag);
            //Debug.Log(dir.magnitude);
            //Debug.Log(dir.ToString());
            return speed * dir.normalized;

            //return new Vector3(x * speed, 0, z * speed);
        }

        return new Vector3(0, 0, 0);
    }

    public float GetSpeed()
    {
        return speed;
    }
}
