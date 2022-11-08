using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent enemy;
    public GameObject tower;
    public GameObject player;

	public float startHealth = 60.0f;
	public float health;


    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("Tower");
        enemy = GetComponent<NavMeshAgent>();
		health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Move to the tower
        enemy.SetDestination(tower.transform.position);

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag( "Tower")) {
            enemy.isStopped = true;
        } 
    }

	public void TakeDamage (float amount)
	{
		health -= amount;

		// healthBar.fillAmount = health / startHealth;

		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

}

