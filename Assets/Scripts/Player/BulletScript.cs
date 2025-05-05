using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;
    RaycastHit hit;
    [Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
    public GameObject decalHitWall;
   
    [Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
    public float floatInfrontOfWall;
    [Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
    public GameObject bloodEffect;
    [Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
    public LayerMask ignoreLayer;
  

    private int damage;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GunInventory gunInventory = player.GetComponent<GunInventory>();
            if (gunInventory != null && gunInventory.currentGun != null)
            {
                GunScript currentGun = gunInventory.currentGun.GetComponent<GunScript>();
                if (currentGun != null)
                {
                    if (currentGun.currentStyle == GunStyles.nonautomatic)
                    {
                        damage = 100;
                    }
                    else if (currentGun.currentStyle == GunStyles.automatic)
                    {
                        damage = 50;
                    }
                }
            }
        }

      
    }

    /*
    * Uppon bullet creation with this script attatched,
    * bullet creates a raycast which searches for corresponding tags.
    * If raycast finds somethig it will create a decal of corresponding tag.
    */
    void Update()
    {
        // Raycast to check if bullet hit something
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            if (decalHitWall)
            {
                if (hit.transform.tag == "LevelPart")
                {
                    // Create decal on wall
                    // hit.normal is the normal of the wall where bullet hit and it is vector that is perpendicular to the wall

                    Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject);
                    Debug.Log("Hit wall");
                }
                if (hit.transform.tag == "Dummie")
                {
                    hit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        Vector3 targetDirection = player.transform.position - hit.transform.position;
                        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                        hit.transform.rotation = Quaternion.Slerp(hit.transform.rotation, lookRotation, Time.deltaTime * 5f); // Quay mượt mà


                    }
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject);
                }
            }
            Destroy(gameObject);
        }
        Destroy(gameObject, 1f);
    }
}
