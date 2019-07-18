using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 10f;
    public int damage;
    public bool bounce;
    private Vector3 previousPosition;
    int mask = LayerMask.GetMask("Default", "InteractSolid");

    private IEnumerator Start()
    {
        previousPosition = transform.position;
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

    public void FixedUpdate()
    {
        RaycastHit raycastHit = new RaycastHit();
        Vector3 delta = transform.position - previousPosition;
        Physics.Raycast(transform.position, delta.normalized, out raycastHit, delta.magnitude, mask);
        if(raycastHit.transform != null)
        {
            Hit(raycastHit.transform.gameObject);
        }
        previousPosition = transform.position;
    }

    public virtual void Hit(GameObject obj)
    {
        Debug.Log(gameObject + " was hit");
    }
}
