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
            Vector3 fwd = transform.forward;
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
			CmdDoMagic(GetComponent<HeroModel>().GetPlayerId(), fwd, pos, transform.rotation);
            nextActiveTime = Time.time + cooldown;
        }
    }

    /// <summary>
    /// Spawns the basic attack projectile on the server.
    /// </summary>
    /// <param name="id">ID of the player who spawned this basic attack projectile.</param>
    [Command]
	private void CmdDoMagic(int id, Vector3 originalFWD, Vector3 originalPOS,  Quaternion originalROT)
    {
        // As commands are run on the server,  need to send in information with relation to client side positioning.
		// Set projectile parameters
		Vector3 projPos = originalPOS + originalFWD * 2f + transform.up * 1.5f;

		GameObject bolt = Instantiate (projectilePrefab, projPos, originalROT);
        

		bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 28f;
		bolt.GetComponent<Projectile>().SetProjectileParams(range, GetComponent<HeroModel>().GetCurrentAttack(), id);
        
		NetworkServer.Spawn(bolt);
		playerSounds.RpcPlayBasicAttackSound();
        
		Destroy(bolt, range);

    }
}
