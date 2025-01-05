using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f; // Normal yavaþ yürüyüþ hýzý
    public float runSpeed = 7f; // Koþma hýzý
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float JumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    void FixedUpdate()
    {
        // Yerde olup olmadýðýmýzý kontrol et
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yere temas ettiðinde düþüþ hýzýný ayarla
        }

        // Hareket kontrolü
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Shift tuþuna basýlýysa yavaþ yürü, deðilse koþ
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? speed : runSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Zýplama kontrolü
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime; // Yerçekimini uygula
        controller.Move(velocity * Time.deltaTime); // Karakter hareketi
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity); // Zýplama hesaplamasý
    }
}
