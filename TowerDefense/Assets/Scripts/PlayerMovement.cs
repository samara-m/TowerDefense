using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;

    private NavMeshAgent myAgent;
    public Animator playerAnimator;
    public bool isRunning;
    public bool targetedEnemy = false;
    public bool leftClick = true;
    RaycastHit hitInfo;
    public PlayerStats player;
  
    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        myAgent.speed = 20.0f;
        player = GetComponent<PlayerStats>();
        player.health = player.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Moving to position
        if (Input.GetMouseButtonDown(1))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                myAgent.isStopped = false;
                myAgent.SetDestination(hitInfo.point);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitInfo))
            {
                // Check if click on enemy
                if (hitInfo.transform != null){
                    if (hitInfo.transform.gameObject.tag == "Enemy"){
                        targetedEnemy = true;
                        leftClick = true;
                    }
                }
            }
        }

        // Follow the targeted enemy 
        if (targetedEnemy == true ){
            myAgent.isStopped = false;
            myAgent.SetDestination(hitInfo.transform.position);
        }

        movingAnimation();
    }

    void movingAnimation(){
        if (myAgent.remainingDistance <= myAgent.stoppingDistance || myAgent.isStopped == true)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }
        playerAnimator.SetBool("isRunning", isRunning);
    }

    // Reached enemy
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag( "Enemy") && leftClick == true) {
            Damage(other.gameObject.transform);
            Attack();
        } 
    }

    void  OnTriggerStay(Collider other)
    {
        if (other.CompareTag( "Enemy") && leftClick == true) {
            Damage(other.gameObject.transform);
            Attack();
        } 
    }

    void Damage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();
        transform.LookAt(e.transform);

		if (e != null)
		{
            e.enemyAttacked = true;
			e.TakeDamage(player.damage);
		}
	}

    public void TakeDamage (float amount){
        player.health -= amount;
        Debug.Log("Player health -" + amount + ", remain " + player.health);

		if (player.health <= 0)
		{
			playerAnimator.SetTrigger("isDead");
		}
    }

    void Attack(){
        myAgent.isStopped = true;
        targetedEnemy = false;
        leftClick = false;
        playerAnimator.SetTrigger("isAttacking");
    }

}
