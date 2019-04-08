using UnityEngine;
using NUnit.Framework;
using UnityEngine.Networking;

/// <summary>
/// This class holds the unit test for MatchManager.cs located in the Scripts > GameManagement folder.
/// </summary>
public class MatchManagerUnitTest
{
    /// <summary>
    /// Testing function: AddPlayerToMatch(NetworkConnection)
    /// Checking for false, as we are calling this function not on the server.
    /// This is because no network is setup.
    /// </summary>
    [Test]
    public void MatchManager_AddPlayerToMatch_ShouldReturnFalse()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        MatchManager matchManager = gameObject.AddComponent<MatchManager>();
        NetworkConnection conn = new NetworkConnection();

        // Assert that AddPlayerToMatch() returns false
        Assert.IsFalse(matchManager.AddPlayerToMatch(conn), "AddPlayerToMatch() did not return false!");
    }

    /// <summary>
    /// Testing function: RemovePlayerFromMatch(NetworkConnection)
    /// Checking for false, as we are calling this function not on the server.
    /// This is because no network is setup.
    /// </summary>
    [Test]
    public void MatchManager_RemovePlayerFromMatch_ShouldReturnFalse()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        MatchManager matchManager = gameObject.AddComponent<MatchManager>();
        NetworkConnection conn = new NetworkConnection();

        // Assert
        Assert.IsFalse(matchManager.RemovePlayerFromMatch(conn), "RemovePlayerFromMatch() did not return false!");
    }

    /// <summary>
    /// Testing function: GetNumOfPlayers()
    /// Testing return value of 0, as we have not added any players to the match.
    /// </summary>
    [Test]
    public void MatchManager_GetNumOfPlayers_ShouldReturnZero()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        // Assert
        Assert.AreEqual(0, matchManager.GetNumOfPlayers(), "Expected 0 number of players, but did not get 0!");
    }

    /// <summary>
    /// Testing functions: GetMaxPlayers()
    /// Testing that the return value is 2.
    /// </summary>
    [Test]
    public void MatchManager_GetMaxPlayers_ShouldReturn2()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        // Assert
        Assert.AreEqual(2, matchManager.GetMaxPlayers(), "Expected 2 for max number of players, but did not get 2!");
    }
}
