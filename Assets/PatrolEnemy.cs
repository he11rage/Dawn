using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool facingLeft = true;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));

        var hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

        if (hit == false && facingLeft)
        {
            Debug.Log("Flip Enemy");
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = !facingLeft;
        }
        else if (hit == false && !facingLeft)
        {
            Debug.Log("Flip Enemy");
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = !facingLeft;
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

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
    }
}