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
    public GameManager gameManager;

	public float startHealth = 60.0f;
	public float health;
    private bool nextAttack = true;

    public float enemyDamage = 20.0f;
    public bool enemyAttacked = false;
    public bool attackTower = false;
    public bool attackPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.FindWithTag("Tower");
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemy = GetComponent<NavMeshAgent>();
		health = startHealth;
        enemyDamage = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Move to the tower
        enemy.SetDestination(tower.transform.position);
        
        // got attacked, then follow player
        if (enemyAttacked == true){
            enemy.isStopped = false;
            attackTower = false;
            enemy.SetDestination(player.transform.position);
        }

        // Enemy reached target
        if (enemy.isStopped == true && nextAttack == true){
            if (attackTower == true)
                DamageTower(tower.transform);
            if(attackPlayer == true)
                DamagePlayer(player.transform);
            nextAttack = false;
            StartCoroutine(delayAttacks());
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Tower")){
            enemy.isStopped = true;
            transform.LookAt(tower.transform);
            attackTower = true;
        }
        else if(other.CompareTag("Player")){
            transform.LookAt(player.transform);
            enemy.isStopped = true;
            attackPlayer = true;
            enemyAttacked = false;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            enemyAttacked = true;
        }
    }

	public void TakeDamage (float amount)
	{
		health -= amount;
        Debug.Log("Enemy health -"+ amount + ", remain " + health);

		// healthBar.fillAmount = health / startHealth;

		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

    void DamageTower(Transform target){
        Tower t = target.GetComponent<Tower>();
        transform.LookAt(t.transform);

        if (t != null)
		{
			t.towerHealth -= enemyDamage;
            Debug.Log("towerHealth: " + t.towerHealth);
		}
        
    }

    void DamagePlayer(Transform target){
        Debug.Log("Attacking player");
        PlayerMovement p = target.GetComponent<PlayerMovement>();
        transform.LookAt(p.transform);

        if (p != null)
		{
            p.TakeDamage(enemyDamage);
		}
        
    }

    IEnumerator delayAttacks(){
        yield return new WaitForSeconds(3);
        nextAttack = true;
    }

}

