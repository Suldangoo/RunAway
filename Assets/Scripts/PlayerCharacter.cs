using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    float jumpForce;
    [SerializeField]
    int maxJumpCount = 1;

    int lifeCount;
    int currentJumpCount;
    bool isGrounded;
    bool isSliding;
    bool isDead;
    bool isInvincible;

    Vector2 startPos;
    Rigidbody2D rb;
    Animator anim;
    AudioSource AudioSource;
    SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        // pc�� ��� (����Ͽ����� ��ư ó��)
        if (Input.GetButtonDown("Jump"))
        {
            JumpCheck();
        }

        if (InputManager.Instance.SlideInput)
        {
            if(!isSliding) OnStartSlide();

        }
        else
        {
            OnEndSlide();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ݶ��̴��� �ε�ģ ��ġ�� ���� �� �̻��϶��� �۵�
        // �Ӹ� �κ��� �ε����� �������� �ʵ���
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            currentJumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public void JumpCheck()
    {
        if (currentJumpCount >= maxJumpCount) return;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //AudioSource.Play();

        currentJumpCount++;

        anim.SetBool("Grounded", isGrounded);
    }

    public void OnStartSlide()
    {
        anim.SetBool("slide", true);
        isSliding = true;
    }

    public void OnEndSlide()
    {
        isSliding = false;
        anim.SetBool("slide", false);
    }

}
