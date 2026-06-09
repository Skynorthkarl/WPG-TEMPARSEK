using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    [Header("Ground Check")]
    private Animator anim;
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Vector3 originalScale;
    private bool facingRight = true; // ← tambah flag arah hadap
    public Transform torchLight;
    public bool canMove = false;

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (!value)
            rb.linearVelocity = Vector2.zero;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!canMove)
        {
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
        anim.SetBool("isWalking", move != 0);

        if (move > 0 && !facingRight)
        {
            Flip(true);
        }
        else if (move < 0 && facingRight)
        {
            Flip(false);
        }

        rb.linearVelocity = new Vector2(
            move * speed,
            rb.linearVelocity.y
        );

        // Lompat
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ← fungsi flip baru, scale tidak disentuh di tempat lain
    private void Flip(bool right)
    {
        facingRight = right;

        transform.localScale = new Vector3(
            right ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
            originalScale.y, // ← selalu pakai originalScale.y, aman
            originalScale.z
        );

        if (torchLight != null)
            torchLight.localPosition = new Vector3(right ? 0.5f : -0.5f, 0.2f, 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}