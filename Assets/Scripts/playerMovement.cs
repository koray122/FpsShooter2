using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f; // Normal yava� y�r�y�� h�z�
    public float runSpeed = 7f; // Ko�ma h�z�
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float JumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    void FixedUpdate()
    {
        // Yerde olup olmad���m�z� kontrol et
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yere temas etti�inde d���� h�z�n� ayarla
        }

        // Hareket kontrol�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Shift tu�una bas�l�ysa yava� y�r�, de�ilse ko�
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? speed : runSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Z�plama kontrol�
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime; // Yer�ekimini uygula
        controller.Move(velocity * Time.deltaTime); // Karakter hareketi
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity); // Z�plama hesaplamas�
    }
}
