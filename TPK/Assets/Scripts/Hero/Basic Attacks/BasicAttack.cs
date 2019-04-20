using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Behaviour of basic attack.
/// </summary>
[NetworkSettings(channel = 0, sendInterval = 0.05f)]
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
            CmdDoMagic(GetComponent<HeroModel>().GetPlayerId());
            nextActiveTime = Time.time + cooldown;
        }
    }

    /// <summary>
    /// Spawns the basic attack projectile on the server.
    /// </summary>
    /// <param name="id">ID of the player who spawned this basic attack projectile.</param>
    [Command]
    private void CmdDoMagic(int id)
    {
		// Set projectile parameters
		Vector3 projPos = transform.position + transform.forward * 2f + transform.up * 1.5f;

		GameObject bolt = Instantiate (projectilePrefab, projPos, transform.rotation);
        

		bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 28f;
		bolt.GetComponent<Projectile>().SetProjectileParams(range, GetComponent<HeroModel>().GetCurrentAttack(), id);
        
		NetworkServer.Spawn(bolt);
		playerSounds.RpcPlayBasicAttackSound();
        
		Destroy(bolt, range);

    }
}
