using UnityEngine;

/// <summary>
/// Interface for accessing Unity services.
/// Needed to separate Unity Services from game code logic for unit testing.
/// Isolating Unity Services allows us to perform dependency injection during unit testing.
/// We can now "mock" (NSubstitute) UnityServices during unit tests, and will not need to rely on Unity's functions.
/// </summary>
public interface IUnityService
{
    bool GetKeyDown(KeyCode key);
    void Destroy(GameObject gameObject);
    GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation);
}

class UnityService : IUnityService
{
    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public void Destroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }

    public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
    {
        return Object.Instantiate(original, position, rotation);
    }
}