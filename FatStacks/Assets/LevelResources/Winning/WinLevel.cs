using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    public string nextLevel;

    private void Start()
    {
        //Set layer so that it only collides with player
        gameObject.layer = LayerMask.NameToLayer("HitPlayerOnly");
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        GameObject winPlayer = Instantiate(player.WinCharacter, player.transform.position, new Quaternion(player._Camera.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w));
        winPlayer.GetComponentInChildren<WinScreenScripts>().scene = nextLevel;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(player.gameObject);
    }
}
