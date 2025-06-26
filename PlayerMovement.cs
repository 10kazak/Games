using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform planet;
    public float rotationSpeed = 200f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canMove = false;
    private Animator animator;
    private PlayerHealth playerHealth;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();

        rb.useGravity = false;

        // Убрали автоматический запуск движения, теперь управляется извне
    }

    void Update()
    {
        if (!canMove) return;

        // Проверка на смерть
        if (!isDead && playerHealth.health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            canMove = false;
            return;
        }

        // Движение вперёд
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        transform.position += forwardMove;

        // Поворот
        float turnInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        // Поворот с использованием левого стика по горизонтали (ось "Joystick Axis X")
        float turnInput = Input.GetAxis("Joystick X"); // Альтернатива: "Horizontal"
        transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // Прыжок с использованием кнопки "X" на контроллере (обычно "Joystick Button 1")
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }


        // Гравитация к планете
        Vector3 gravityDirection = (transform.position - planet.position).normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;

        rb.AddForce(-gravityDirection * 9.81f * 1.5f);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Planet")
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Planet")
        {
            isGrounded = false;
        }
    }

    // Управление движением извне (например, из интро)
    public void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    public void PlayHurtAnimation()
    {
        if (!isDead)
        {
            animator.SetTrigger("hurt");
        }
    }
}
