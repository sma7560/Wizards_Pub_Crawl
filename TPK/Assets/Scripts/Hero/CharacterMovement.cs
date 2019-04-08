using UnityEngine;

/// <summary>
/// Encapsulates the hero character movement logic.
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

    public void SetSpeed(float s)
    {
        speed = s;
    }

    /// <summary>
    /// Calculates the movement of the unit.
    /// </summary>
    /// <param name="forward">Whether or not unit is moving forward.</param>
    /// <param name="back">Whether or not unit is moving backward.</param>
    /// <param name="left">Whether or not unit is moving left.</param>
    /// <param name="right">Whether or not unit is moving right.</param>
    /// <returns>Vector that the unit is moving towards.</returns>
    public Vector3 Calculate(bool forward, bool back, bool left, bool right)
    {
        Vector3 posX = new Vector3(1, 0, -1);
        Vector3 posZ = new Vector3(1, 0, 1);
        if (forward || back || left || right)
        {
            int x = 0;
            int z = 0;

            if (forward)
            {
                z++;
            }

            if (back)
            {
                z--;
            }

            if (left)
            {
                x--;
            }

            if (right)
            {
                x++;
            }

            Vector3 dir = (x * posX) + (z * posZ);

            // Normalize the direction vector then multiply it with the desired magnitude.
            return speed * dir.normalized;
        }

        return new Vector3(0, 0, 0);
    }
}
