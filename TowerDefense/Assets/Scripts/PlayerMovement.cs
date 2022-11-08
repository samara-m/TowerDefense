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
    public float playerHealth = 500.0f;
    public float playerDamage = 20.0f;
    public bool leftClick = true;
    RaycastHit hitInfo;
  
    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        myAgent.speed = 20.0f;
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

		if (e != null)
		{
			e.TakeDamage(playerDamage);
		}
	}

    void Attack(){
        myAgent.isStopped = true;
        targetedEnemy = false;
        leftClick = false;
        playerAnimator.SetTrigger("isAttacking");
    }

}
