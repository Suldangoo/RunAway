using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    Collider2D standCollider;
    [SerializeField]
    Collider2D slideCollider;

    [SerializeField]
    GameObject jumpEfx;

    [SerializeField]
    float jumpForce;
    [SerializeField]
    int maxJumpCount = 1;

    [Header("피격 상태 유지 시간")]
    [SerializeField]
    float hitHoldTime = 1f;

    [Header("피격 상태 후 무적 시간")]
    [SerializeField]
    float invincibleTIme = 0.25f;

    //int lifeCount;
    int currentJumpCount;
    bool isGrounded;
    bool isSliding;
    bool isDead;
    bool isInvincible;
    bool isOnHit;

    Vector2 startPos;
    Rigidbody2D rb;
    Animator anim;
    AudioSource AudioSource;
    SpriteRenderer SpriteRenderer;

    GameManager GameManager => GameManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        slideCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = GameManager.GameSpeed;
        anim.SetFloat("animSpeed", animSpeed);

        if (isDead) return;

        anim.SetFloat("velocityY", rb.velocity.y);

        
        // pc의 경우 (모바일에서는 버튼 처리)
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
            if (isSliding) OnEndSlide();
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadZone") &&
            collision.gameObject.layer == LayerMask.NameToLayer("Drill") && !isDead)
        {
            //Hit();

            //if (!isDead)
            //{
            //    rb.velocity = Vector2.zero;
            //    transform.position = startPos; // Repositioning
            //}

            GameManager.Instance.GameOver();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !isDead && !isInvincible)
        {
            Hit();
            Debug.Log("HIT WITH : " + collision.gameObject.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 콜라이더가 부딪친 위치가 일정 값 이상일때만 작동
        // 머리 부분이 부딪히면 동작하지 않도록
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            currentJumpCount = 0;
            anim.SetBool("jump", !isGrounded);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //isGrounded = false;
    }

    public void JumpCheck()
    {
        if (isDead) return;
        if (isOnHit) return;
        if (currentJumpCount >= maxJumpCount) return;
        if (isSliding) OnEndSlide();

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //AudioSource.Play();

        currentJumpCount++;
        isGrounded = false;
        anim.SetBool("jump", !isGrounded);

        Vector3 offset = Vector3.up * 0.1f + Vector3.left * 0.5f;
        Instantiate(jumpEfx, transform.position + offset, Quaternion.identity);

        //anim.SetBool("Grounded", isGrounded);

        if (currentJumpCount > 1)
        {
            anim.SetTrigger("doubleJump");
        }
    }

    public void OnStartSlide()
    {
        if (isDead) return;
        if (isOnHit) return;

        anim.SetBool("slide", true);
        isSliding = true;

        standCollider.enabled = false;
        slideCollider.enabled = true;
    }

    public void OnEndSlide()
    {
        isSliding = false;
        anim.SetBool("slide", false);

        standCollider.enabled = true;
        slideCollider.enabled = false;
    }

    void Die()
    {
        isDead = true;
        rb.GetComponent<Collider2D>().isTrigger = true;
    }

    void Hit()
    {
        float holdTime = 1f;
        StartCoroutine(HitCr(hitHoldTime));
        StartCoroutine(InvincibleControl(holdTime + invincibleTIme));
    }

    IEnumerator HitCr(float duration)
    {
        //float stopTime = 0.2f;

        isOnHit = true;
        anim.SetBool("onHit", true);
        //GameManager.SetGameSpeed(0);

        //yield return new WaitForSeconds(duration);

        float t = 0;
        while (true)
        {
            t += Time.deltaTime / duration;
            if (t > 1) t = 1;

            float currentSpeed = Mathf.Lerp(1, 0, t);
            GameManager.SetGameSpeed(currentSpeed);

            if (t == 1) break;

            yield return null;
        }
        
        GameManager.SetGameSpeed(1);
        isOnHit = false;
        anim.SetBool("onHit", false);        
    }

    IEnumerator InvincibleControl(float duration, float interval = 0.1f)
    {        
        isInvincible = true;        
        
        float InvincibleTime = 0;

        while (InvincibleTime < duration)
        {         
            SpriteRenderer.color = new Color32(255, 255, 255, 90);            

            yield return new WaitForSeconds(interval);

            SpriteRenderer.color = new Color32(255, 255, 255, 180);
       
            yield return new WaitForSeconds(interval);

            InvincibleTime += interval;
        }

        SpriteRenderer.color
                   = new Color32(255, 255, 255, 255);

        isInvincible = false;
     
        yield return null;
    }

}
