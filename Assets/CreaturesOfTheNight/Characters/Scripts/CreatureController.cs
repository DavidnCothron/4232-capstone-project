using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour {
    [Header("Physics")]
    Rigidbody2D rb;
    public float MoveVelocity=5f;
    public float JumpVelocity=20f;
    [Header("Animations")]
    Animator animator;
    [HideInInspector]
    public float CurrentAttackTime;
    public string standAnimationState;
    public string RunAnimationState;
    public string JumpAnimationState;
    public string FallAnimationState;
    public string Attack1AnimationState;
    public float Attack1AnimationDuration;
    public string Attack2AnimationState;
    public float Attack2AnimationDuration;
    public string Attack3AnimationState;
    public float Attack3AnimationDuration;
    public string DefAnimationState;

    public string DeathAnimationState;
    public enum state { stand, jump, fall, run, attack1, attack2, attack3, Def, Death}
    public state State;
    [Header("Ground Check")]
    public bool Grounded;
    [HideInInspector]
    public bool Jumping;
    [HideInInspector]
    public bool blowing;
    [HideInInspector]
    public bool Defensing;
    [HideInInspector]
    public bool died;
    public Transform groundChecker;
    public float checkerRadius = 0.2f;
    public LayerMask groundLayer;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
	

	void Update () {
        if (died)
        {
            animator.CrossFade(DeathAnimationState, 0f);
            return;
        }

        if (!UpdateBlows())
        {
            UpdateMovement();
        }
        UpdateAnimations();
    }
    void UpdateMovement()
    {
        //ground Check
        Collider2D Coll = Physics2D.OverlapCircle(groundChecker.transform.position, checkerRadius, groundLayer);

        if (Coll == null)
        {
            Grounded = false;

        }
        else
        {
            //print(Coll.gameObject.name);
            Grounded = true;
            Jumping = false;
        }


        //move
        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            State = state.run;
        }
        else
        {
            State = state.stand;
        }
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveVelocity, rb.velocity.y);

        //flip
        Vector3 scale = transform.localScale;
        if (Input.GetAxisRaw("Horizontal") < 0f)
        { //Flip to left
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        { //Flip to right
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        //Jump
        if (Grounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpVelocity);
            State = state.jump;
        }
        if (!Grounded)
        {
            if (rb.velocity.y > 0.1f)
            {
                State = state.jump;
            }
            else if (rb.velocity.y < -0.1f)
            {
                State = state.fall;
            }
        }

    }
    bool holdingDefense;

    bool Defense()
    {
        if (!Defensing)
        {
            // if (Input.GetButtonDown())
            // {
            //     animator.CrossFade(DefAnimationState, 0f, 0, 0f);
            //     Defensing = true;
            //     holdingDefense = true;
            //     State = state.Def;
            // }
        }
        else
        {
            if (Input.GetButtonUp("Defense"))
            {
                holdingDefense = false;
            }
            if (holdingDefense && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            {
                animator.CrossFade(DefAnimationState, 0f, 0, 0.5f);
            }
            if (!holdingDefense && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                Defensing = false;
            }

        }
        return Defensing;
    }

    bool UpdateBlows()
    {

        if(Defense())
        {
            return true;
        }



        if (!blowing)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CurrentAttackTime = Time.deltaTime;
                blowing = true;
                State = state.attack1;
                return true;
            }
            if (Input.GetButtonDown("Fire2"))
            {
                CurrentAttackTime = Time.deltaTime;
                blowing = true;
                State = state.attack2;
                return true;
            }
            if (Input.GetButtonDown("Fire3"))
            {
                CurrentAttackTime = Time.deltaTime;
                blowing = true;
                State = state.attack3;
                return true;
            }
        } else
        {
            CurrentAttackTime += Time.deltaTime;
            if(State == state.attack1)
            {
                if (CurrentAttackTime>Attack1AnimationDuration)
                {
                    blowing = false;
                    return false;
                }
            }
            if (State == state.attack2)
            {
                if (CurrentAttackTime > Attack2AnimationDuration)
                {
                    blowing = false;
                    return false;
                }
            }
            if (State == state.attack3)
            {
                if (CurrentAttackTime > Attack3AnimationDuration)
                {
                    blowing = false;
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    void UpdateAnimations()
    {
        if (State == state.stand)
        {
            animator.CrossFade(standAnimationState, 0f);
        } else
        if (State == state.run)
        {
            animator.CrossFade(RunAnimationState, 0f);
        }
        else
        if (State == state.jump)
        {
            animator.CrossFade(JumpAnimationState, 0f);
        }
        else if (State == state.fall)
        {
            animator.CrossFade(FallAnimationState, 0f);
        }
        else
        if (State == state.attack1)
        {
            animator.CrossFade(Attack1AnimationState, 0f);
        }
        else
        if (State == state.attack2)
        {
            animator.CrossFade(Attack2AnimationState, 0f);
        }
        else
        if (State == state.attack3)
        {
            animator.CrossFade(Attack3AnimationState, 0f);
        } else
        if (State == state.Death)
        {
            animator.CrossFade(DeathAnimationState, 0f);
            print("morra");
        }
        else if (State == state.Def)
        {
            animator.CrossFade(DefAnimationState, 0f);
        }
    }

    public void Die()
    {
        died = true;
        State = state.Death;

    }
}
