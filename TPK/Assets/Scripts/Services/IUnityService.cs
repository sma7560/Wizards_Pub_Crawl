using UnityEngine;

/// <summary>
/// Interface for accessing Unity services.
/// Needed to separate Unity Services from game code logic for unit testing.
/// Isolating Unity Services allows us to perform dependency injection during unit testing.
/// We can now "mock" (NSubstitute) UnityServices during unit tests, and will not need to rely on Unity's functions.
/// </summary>
public interface IUnityService
{
    float GetAxisRaw(string axisName);  // Allows us to mock user input during unit testing
    float GetAxis(string axisName);
    bool GetKeyDown(KeyCode key);
    bool GetKeyUp(KeyCode key);
    void Destroy(GameObject gameObject);
    bool GetMouseButtonUp(int button);
    Vector3 GetMousePosition();
    float GetDeltaTime();
}

class UnityService : IUnityService
{
    public float GetAxisRaw(string axisName)
    {
        return Input.GetAxisRaw(axisName);
    }

    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public void Destroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }

    public bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public float GetDeltaTime()
    {
        return Time.deltaTime;
    }
}