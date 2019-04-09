using UnityEngine;

/// <summary>
/// Interface for accessing Unity services.
/// Needed to separate Unity Services from game code logic for unit testing.
/// Isolating Unity Services allows us to perform dependency injection during unit testing.
/// We can now "mock" (NSubstitute) UnityServices during unit tests, and will not need to rely on Unity's functions.
/// </summary>
public interface IUnityService
{
    bool GetKey(KeyCode key);
    bool GetKeyDown(KeyCode key);
}

class UnityService : IUnityService
{
    public bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}