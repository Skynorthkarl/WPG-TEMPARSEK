using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Vector3 originalScale;
    public Transform torchLight;
    public bool canMove = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update() {
        if (!canMove) {
        rb.linearVelocity = Vector2.zero;
        return;
    }

        // Cek tanah
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );

        // Gerak kanan kiri
        float move = Input.GetAxisRaw("Horizontal");
            if (move > 0) {
                transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        
            if (torchLight != null)
            {
                torchLight.localPosition =
                    new Vector3(0.5f, 0.2f, 0);
            }
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        
            if (torchLight != null)
            {
                torchLight.localPosition =
                    new Vector3(-0.5f, 0.2f, 0);
            }
        }

        rb.linearVelocity = new Vector2(
            move * speed,
            rb.linearVelocity.y
        );

        // Lompat
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                0f
            );

            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }

        // Jongkok
        if (Input.GetKey(KeyCode.S)) {
            transform.localScale = new Vector3(
            transform.localScale.x,
            originalScale.y * 0.5f,
            originalScale.z );
        } else {
            transform.localScale = new Vector3(
            transform.localScale.x > 0 ?
            Mathf.Abs(originalScale.x) :
            -Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z );
}
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            checkRadius
        );
    }
}