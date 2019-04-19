using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Behaviour of basic attack.
/// </summary>
public class BasicAttack : NetworkBehaviour
{
    public GameObject projectilePrefab;
    private PlayerSoundController playerSounds; // sound effects for basic attack

    // Attack stats
    private readonly float cooldown = 0.5f;
    private readonly float range = 3;

    private float nextActiveTime;
    
    void Awake()
    {
        nextActiveTime = 0;
        playerSounds = GetComponent<PlayerSoundController>();
    }

    /// <summary>
    /// Performs the basic attack.
    /// </summary>
    public void PerformAttack()
    {
        if (!isLocalPlayer) return;

        if (Time.time > nextActiveTime)
        {
            Vector3 fwd = transform.forward;
            Vector3 pos = transform.position;
            CmdDoMagic(GetComponent<HeroModel>().GetPlayerId(), fwd.x, fwd.y, fwd.z, pos.x, pos.y, pos.z);
            nextActiveTime = Time.time + cooldown;
        }
    }

    /// <summary>
    /// Spawns the basic attack projectile on the server.
    /// </summary>
    /// <param name="id">ID of the player who spawned this basic attack projectile.</param>
    [Command]
    private void CmdDoMagic(int id, float x, float y, float z, float px, float py, float pz)
    {
        GameObject bolt = Instantiate(projectilePrefab);

        Vector3 fwd = new Vector3(x, y, z);
        Vector3 pos = new Vector3(px, py, pz);
        // Set projectile parameters
        bolt.transform.position = pos + fwd * 2f + transform.up * 1.5f;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 28f;
        bolt.GetComponent<Projectile>().SetProjectileParams(range, GetComponent<HeroModel>().GetCurrentAttack(), id);

        NetworkServer.Spawn(bolt);
        playerSounds.RpcPlayBasicAttackSound();
        Destroy(bolt, range);

    }
}
