using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool facingLeft = true;
    public Transform player;
    public Animator animator;
    public float attackRange = 0.5f;
    public float visionRange = 2f;
    public bool inAttackRange = false;
    public bool isPatrolling = true;
    public bool isChasing = false;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inAttackRange = true;
            isPatrolling = false;
            isChasing = false;
        }
        else if (Vector2.Distance(transform.position, player.position) <= visionRange)
        {
            var direction = (player.position - transform.position).normalized;
            var hit = Physics2D.Raycast(transform.position, direction, visionRange, layerMask);

            if (hit.collider == null)
            {
                inAttackRange = false;
                isPatrolling = false;
                isChasing = true;
                //Debug.Log("Chasing!");
            }
            else
            {
                inAttackRange = false;
                isPatrolling = true;
                isChasing = false;
                //Debug.Log("Wall, can't chase.");
            }
        }

        if (inAttackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            moveSpeed = 0;
            //Debug.Log("Attack!");
            animator.SetTrigger("Attack_2");
            lastAttackTime = Time.time;
        }
        else if (isChasing)
        {
            moveSpeed = 1.5f;
            //Debug.Log("Chasing!");
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            var direction = (player.position - transform.position).normalized;
            if (direction.x > 0 && facingLeft)
                Flip();
            else if (direction.x < 0 && !facingLeft)
                Flip();
        }
        else if (isPatrolling)
        {
            moveSpeed = 0.5f;
            //Debug.Log("Patrolling!");
            transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));

            var hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
            RaycastHit2D hitWall;
            
            if (facingLeft)
                hitWall = Physics2D.Raycast(checkPoint.position, Vector2.left, 0.1f, layerMask);
            else
                hitWall = Physics2D.Raycast(checkPoint.position, Vector2.right, 0.1f, layerMask);
            
            if (hit == false || hitWall.collider != null)
                Flip();
        }

        if (moveSpeed == 0)
            animator.SetBool("isStop", true);
        else
            animator.SetBool("isStop", false);

        if (moveSpeed >= 1.5f)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);
    }
    
    public void Attack()
    {
        var collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collInfo)
        {
            Debug.Log($"{collInfo.transform.name} has been attacked");
            if (collInfo.gameObject.GetComponent<Player>() != null)
            {
                collInfo.gameObject.GetComponent<Player>().TakeDamage(10);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.green;
        if (facingLeft)
            Gizmos.DrawRay(checkPoint.position, Vector2.left * 0.1f);
        else
            Gizmos.DrawRay(checkPoint.position, Vector2.right * 0.1f);

        

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void Flip()
    {
        if (facingLeft)
        {
            //Debug.Log("Flip Enemy");
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = !facingLeft;
        }
        else
        {
            //Debug.Log("Flip Enemy");
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = !facingLeft;
        }
    }
}