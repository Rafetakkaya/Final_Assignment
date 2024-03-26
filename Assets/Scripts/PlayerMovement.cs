using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool isDashReadyW = true;
    bool isDashReadyA = true;
    bool isDashReadyS = true;
    bool isDashReadyD = true;
    bool isRunning = false;
    bool isSliding = false;
    bool canDash = true; // Yeni eklenen değişken

    float lastClickTimeW = 0f;
    float lastClickTimeA = 0f;
    float lastClickTimeS = 0f;
    float lastClickTimeD = 0f;
    float lastShiftTime = 0f;
    float lastCtrlTime = 0f;

    float doubleClickTimeThreshold = 0.3f; // Çift dokunma aralığı (örneğin, 0.5 saniye)
    float dashSpeedMultiplier = 3f; // Dash sırasında hız çarpanı
    float dashDuration = 1f; // Dash süresi (örneğin, 1 saniye)
    float runSpeedMultiplier = 1.5f; // Koşu sırasında hız çarpanı
    float slideSpeedMultiplier = 2.5f; // Kayma sırasında hız çarpanı
    float slideDuration = 0.5f; // Kayma süresi (örneğin, 0.5 saniye)

    private CharacterController controller;
    public float speed = 1f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;

        CheckDash(KeyCode.W, ref isDashReadyW, ref lastClickTimeW);
        CheckDash(KeyCode.A, ref isDashReadyA, ref lastClickTimeA);
        CheckDash(KeyCode.S, ref isDashReadyS, ref lastClickTimeS);
        CheckDash(KeyCode.D, ref isDashReadyD, ref lastClickTimeD);

        CheckRun(KeyCode.LeftShift);
        CheckSlide(KeyCode.LeftControl);
    }

    void CheckDash(KeyCode key, ref bool isDashReady, ref float lastClickTime)
    {
        if (Input.GetKeyDown(key))
        {
            if (Time.time - lastClickTime < doubleClickTimeThreshold && isDashReady && canDash) // canDash kontrolü eklendi
            {
                isDashReady = false;
                lastClickTime = Time.time;
                StartCoroutine(PerformDash(key));
            }
            else
            {
                lastClickTime = Time.time;
                isDashReady = true;
            }
        }
    }

    void CheckRun(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            isRunning = true;
            lastShiftTime = Time.time;
            speed *= runSpeedMultiplier;
        }
        else if (Input.GetKeyUp(key) && isRunning)
        {
            isRunning = false;
            speed /= runSpeedMultiplier;
        }

        // Koşu süresi kontrolü
        if (Time.time - lastShiftTime > dashDuration && isRunning)
        {
            isRunning = false;
            speed /= runSpeedMultiplier;
        }
    }

    void CheckSlide(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!isSliding)
            {
                isSliding = true;
                lastCtrlTime = Time.time;
                StartCoroutine(PerformSlide());
            }
        }
    }

    IEnumerator PerformDash(KeyCode key)
    {
        float originalSpeed = speed;
        speed *= dashSpeedMultiplier; // Hızı artır
        Vector3 dashVector = Vector3.zero;

        switch (key)
        {
            case KeyCode.W:
                dashVector = transform.forward;
                break;
            case KeyCode.A:
                dashVector = -transform.right;
                break;
            case KeyCode.S:
                dashVector = -transform.forward;
                break;
            case KeyCode.D:
                dashVector = transform.right;
                break;
        }

        canDash = false; // Dash yapıldığında canDash'i false yap
        yield return new WaitForSeconds(dashDuration); // Dash süresi

        canDash = true; // Dash süresi bittiğinde canDash'i true yap
        speed = originalSpeed; // Dash süresi bittiğinde hızı geri döndür
    }

    IEnumerator PerformSlide()
    {
        float originalSpeed = speed;
        speed *= slideSpeedMultiplier; // Hızı artır

        yield return new WaitForSeconds(slideDuration); // Kayma süresi

        speed = originalSpeed; // Kayma süresi bittiğinde hızı geri döndür
        isSliding = false;
    }
}
