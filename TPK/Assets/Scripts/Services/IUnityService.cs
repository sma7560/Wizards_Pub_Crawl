using UnityEngine;

/// <summary>
/// Interface for accessing Unity services.
/// Needed to separate Unity Services from game code logic for unit testing.
/// Isolating Unity Services allows us to perform dependency injection during unit testing.
/// We can now "mock" (NSubstitute) UnityServices during unit tests.
/// </summary>
public interface IUnityService
{
    float GetAxisRaw(string axisName);  // Allows us to mock user input during unit testing
    bool GetKeyDown(KeyCode key);
}

class UnityService : IUnityService
{
    public float GetAxisRaw(string axisName)
    {
        return Input.GetAxisRaw(axisName);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}