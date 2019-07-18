using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : HealthManager
{
    public override void Kill()
    {
        Player player = GetComponent<Player>();
        Destroy(gameObject);
        Instantiate(player.DeadCharacter, transform.position, new Quaternion(player._Camera.transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
    }
}
