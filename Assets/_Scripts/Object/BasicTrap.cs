using System.Collections;
using UnityEngine;

public class BasicTrap : MonoBehaviour
{
    private Vector2 knockbackDirection;
    private float knockbackForce = 200;

    public bool isDelay = false;
    private WarriorStatus stats;
    private Rigidbody2D rb;
    public int basicTrapDamage = 10;
    public float slowDownFactor = 0.5f;

    private string objectName;
    
    public void Start()
    {

        string tag = "Player";
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject playerObject = GameObject.FindGameObjectWithTag(tag);
        
        foreach (GameObject obj in objectsWithTag)
        {
            objectName = obj.name;
        }
        if (objectName =="Warrior")
        {
            stats = playerObject.GetComponent<WarriorStatus>();
            rb = playerObject.GetComponent<Rigidbody2D>();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            knockbackDirection = (other.transform.position - transform.position).normalized;
            Debug.Log(knockbackDirection);
            rb.velocity = Vector2.zero;
            isDelay = true;
            StartCoroutine(ApplyKnockbackForceWithDelay());
        }
    }
    

    private IEnumerator ApplyKnockbackForceWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Change the delay time as needed

        ApplyKnockbackForce();
    }

    private void ApplyKnockbackForce()
    {
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        

        if (objectName == "Warrior")
        {
            stats.TakeDamagePlayer(basicTrapDamage);
        }

    }

    private void FixedUpdate()
    {
        if (isDelay)
        {
            ApplyKnockbackForce();
            isDelay = false;
        }
    }

}
