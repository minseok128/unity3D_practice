using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 8f, defaultMoveSpeed = 8f;
    Vector3 forward, right, finalMovement, dodgeMovement;
    Rigidbody rigidbody;
    Animator animator;
    bool jDown;
    bool isJump = false;
    bool dDown;
    bool isDodge = false;

    bool rDown;

    [Range(0f, 1.0f)] public float animationD;
    [Range(1f, 2f)] public float jumpForce = 1.0f;

    void Awake()
    {
        this.animator = this.GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetInput();
        this.Move();
        this.Turn();
        this.Jump();
        this.Dodge();
    }

    void GetInput()
    {
        rDown = Input.GetButton("Run");
        dDown = Input.GetButton("Dodge");
        Vector3 RigthMovement = right * Input.GetAxis("Horizontal");
        Vector3 ForwardMovement = forward * Input.GetAxis("Vertical");

        jDown = Input.GetButtonDown("Jump");

        this.finalMovement = Vector3.Normalize((RigthMovement + ForwardMovement)) * defaultMoveSpeed * Time.smoothDeltaTime;
    }

    void Move()
    {
        if (this.finalMovement != Vector3.zero && !isDodge)
        {
            if (rDown)
            {
                this.animator.SetBool("isWalk", false);
                this.animator.SetBool("isRun", true);
            }
            else
            {
                this.animator.SetBool("isWalk", true);
                this.animator.SetBool("isRun", false);
            }
        }
        else
        {
            this.animator.SetBool("isRun", false);
            this.animator.SetBool("isWalk", false);
        }

        if (this.isDodge)
            this.finalMovement = this.dodgeMovement * 2;
        else if (rDown)
            this.finalMovement *= 1.5f;

        rigidbody.position += this.finalMovement;
    }

    void Turn()
    {
        this.transform.LookAt(this.transform.position + this.finalMovement);
    }

    void Jump()
    {
        if (jDown && !isJump && !isDodge)
        {
            this.rigidbody.AddForce(Vector3.up * 10f * jumpForce, ForceMode.Impulse);
            this.animator.SetBool("isJump", true);
            this.animator.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        //Debug.Log("D Down");
        if (dDown && this.finalMovement != Vector3.zero && !isJump && !isDodge)
        {
            //Debug.Log("Dodge");
            this.moveSpeed = defaultMoveSpeed * 2;
            this.dodgeMovement = this.finalMovement;
            this.animator.SetTrigger("doDodge");
            this.animator.SetBool("isDodge", true);
            isDodge = true;

            StartCoroutine(DodgeOut());
        }
    }

    IEnumerator DodgeOut()
    {
        yield return new WaitForSeconds(animationD);
        this.animator.SetBool("isDodge", false);

        Debug.Log("Dodge out");
        this.moveSpeed = defaultMoveSpeed;


        isDodge = false;
    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Floor")
        {
            this.animator.SetBool("isJump", false);
            isJump = false;
        }
    }
}
