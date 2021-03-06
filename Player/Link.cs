using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Link : MonoBehaviour
{
    static public Link S;
    #region 参数
    [Header("状态")]
    public bool Alife = true;
    public bool Death = false;
    public bool Drop = true;
    public bool isTurningRight = true;
    public float beforeDeath;
    float DeathTime;

    [Header("移动")]
    public float WalkSpeed;
    public float AccelerateTime;
    public float DecelerateTime;
    public Vector2 InputOffset;
    public bool CanMove = true;

    [Header("跳跃")]
    public float JumpSpeed;
    public int JumpTimes;
    public bool isJumping;
    public bool CanJump = true;
    public float JumpStartTime;

    [Header("冲刺")]
    public Vector2 dir;
    public int Dir;
    public bool isDashing;
    public bool CanDash;
    public float DashSpeed;
    public float DashSpeed1;
    public float DashSpeed2;
    public float DragMaxForce;
    public float DragDuration;
    public float DashWaitTime;
    public float DashDelayTime;
    

    [Header("弹墙跳")]
    public bool CanWallJump;
    public bool isWallJumped;
    public float WallJumpSpeedX;
    public float WallJumpStartTime;
    public float WallJumpSpeedY;
    public bool isJumpingButtonReleased = true;

    [Header("重力调整")]
    public float FallMultiplier;
    public float LowJumpMultiplier;
    public float JumpGravity;
    public bool CanUsingGravityAdjuster = true;

    [Header("触地判定")]
    public Vector2 GroundPointOffset;
    public Vector2 GroundJudgeSize;
    public LayerMask GroundLayerMask;
    public bool isOnGround;

    [Header("触墙判定")]
    public bool isOnWall;
    public bool isOnLeftWall;
    public bool isOnRightWall;
    public Vector2 LeftWallPointOffset;
    public Vector2 RightWallPointOffset;
    public Vector2 WallJudgeSize;
    public LayerMask WallLayerMask;

    public bool DeathSounds = false;
    #endregion

    #region 组件
    [Header("音效组件")]
    public AudioSource DeathAudio;
    public AudioSource JumpAudio;
    public AudioSource DashAudio;
    public AudioSource WalkAudio;
    public bool WalkAudioPlayed;
    public Animator animator;
    public Rigidbody2D Rig;
    float velocityX;

    #endregion

    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Link.Awake()-Attempted to assign second Link.S!");
        }
        animator = GetComponent<Animator>();
        Rig = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        animator.SetInteger("Dir", Dir);
        animator.SetFloat("Vertical", Rig.velocity.y);
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isTurningRight", isTurningRight);
        animator.SetBool("Alife", Alife);
        animator.SetBool("isOnGround", isOnGround);
        dir = GetDir();
        isOnGround = OnGround();
        isOnLeftWall = OnLeftWall();
        isOnRightWall = OnRightWall();
        isOnWall = (isOnLeftWall ^ isOnRightWall);
        Move();
        Jump();
        Dash();
        WallJump();
        drop();
        BeforeDeath();
    }

    void Update()
    {

    }
    void BeforeDeath()
    {
        if (!Alife)
        {
            CanDash = false;
            CanJump = false;
            CanMove = false;
            Rig.velocity = Vector2.zero;
            Rig.gravityScale = 0;
            CanUsingGravityAdjuster = false;
            if (DeathSounds == false)
            {
                DeathSounds = true;
                DeathAudio.Play();
            }
        }
    }
    #region 死亡判定
    void KillLink()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region 落地
    void drop()
    {
        if (!isOnGround && Time.time - JumpStartTime > 0.2f)
        {
            Drop = true;
        }
        if (isOnGround && Drop == true)
        {
            Drop = false;
            JumpAudio.Play();
        }
    }
    #endregion

    #region 移动
    void Move()
    {
        //Move
        if (CanMove)
        {
            if (!isOnGround)
            {
                WalkAudio.Stop();
                WalkAudioPlayed = false;
            }
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                if (!WalkAudioPlayed && isOnGround)
                {
                    WalkAudio.Play();
                    WalkAudioPlayed = true;
                }

                isTurningRight = true;
                float tempv = Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime);
                Rig.velocity = new Vector2(tempv, Rig.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                if (!WalkAudioPlayed && isOnGround)
                {
                    WalkAudio.Play();
                    WalkAudioPlayed = true;
                }
                isTurningRight = false;
                float tempv = Mathf.SmoothDamp(Rig.velocity.x, -1 * WalkSpeed * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime);
                Rig.velocity = new Vector2(tempv, Rig.velocity.y);
            }
            else
            {
                WalkAudio.Stop();
                WalkAudioPlayed = false;
                float tempv = Mathf.SmoothDamp(Rig.velocity.x, 0, ref velocityX, DecelerateTime);
                Rig.velocity = new Vector2(tempv, Rig.velocity.y);
            }
        }
        return;
    }
    #endregion

    #region 跳跃
    void Jump()
    {
        //Jump
        if (CanJump)
        {
            if (isOnGround)
            {
                JumpTimes = 1;
                if (Time.time - JumpStartTime > 0.2f)
                    isJumping = false;
            }
            if (Input.GetAxis("Jump") == 1 && !isJumping && isJumpingButtonReleased)
            {
                //JumpAudio.Play();
                isJumpingButtonReleased = false;
                JumpStartTime = Time.time;
                Rig.velocity = new Vector2(Rig.velocity.x, JumpSpeed);
                isJumping = true;
                JumpTimes--;
            }
            if (CanUsingGravityAdjuster)
            {
                if (Rig.velocity.y < 0)
                {
                    Rig.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
                }
                else if (Rig.velocity.y > 0 && Input.GetAxis("Jump") != 1)
                {
                    Rig.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
                }
            }
        }
    }
    #endregion

    #region 冲刺
    //冲刺方向判定
    public Vector2 GetDir()
    {
        Vector2 JudgeDir = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                JudgeDir = new Vector2(1, 1);
                Dir = 1;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                JudgeDir = new Vector2(1, -1);
                Dir = 7;
            }
            else
            {
                JudgeDir = new Vector2(1, 0);
                Dir = 0;
            }
        }

        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                JudgeDir = new Vector2(-1, 1);
                Dir = 3;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                JudgeDir = new Vector2(-1, -1);
                Dir = 5;
            }
            else
            {
                JudgeDir = new Vector2(-1, 0);
                Dir = 4;
            }
        }

        else
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                JudgeDir = new Vector2(0, 1);
                Dir = 2;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                JudgeDir = new Vector2(0, -1);
                Dir = 6;
            }
            else
            {
                if (isTurningRight)
                {
                    JudgeDir = new Vector2(1, 0);
                    Dir = 0;
                }
                else
                {
                    JudgeDir = new Vector2(-1, 0);
                    Dir = 4;
                }
            }
        }
        return JudgeDir;
    }

    void Dash()
    {
        if (Input.GetAxisRaw("Dash") == 1 && CanDash)
        {
            if (dir.x + dir.y == 2)
            {
                DashSpeed = DashSpeed1;
            }
            else
            {
                DashSpeed = DashSpeed2;
            }
            CanDash = false;
            //将玩家当前所有动量清零
            Rig.velocity = Vector2.zero;
            //施加一个力，让玩家飞出去
            Rig.velocity += dir * DashSpeed;
            StartCoroutine(Dashing());
        }

        if (isOnGround && Input.GetAxisRaw("Dash") == 0)
        {
            CanDash = true;
        }
    }

    IEnumerator Dashing()
    {
        //关闭玩家的移动和跳跃功能
        CanMove = false;
        CanJump = false;
        isDashing = true;
        //关闭重力调整器
        CanUsingGravityAdjuster = false;
        //关闭重力影响
        Rig.gravityScale = 0;
        //施加空气阻力
        DOVirtual.Float(DragMaxForce, 0, DragDuration, (x) => Rig.drag = x);
        // ShakeCamera.S.enabled = true;
        DashAudio.Play();
        //等待一段时间
        yield return new WaitForSeconds(DashWaitTime);
        //开启所有关闭的东西
        CanMove = true;
        CanJump = true;
        CanUsingGravityAdjuster = true;
        isDashing = false;
        Rig.gravityScale = 4;
    }
    #endregion

    #region 弹墙跳
    void WallJump()
    {
        if (!isJumpingButtonReleased && Input.GetAxis("Jump") == 0)
        {
            isJumpingButtonReleased = true;
        }
        if (CanWallJump)
        {
            if (Input.GetAxis("Jump") == 1 && isOnWall && !isOnGround && isJumpingButtonReleased)
            {
                JumpStartTime = Time.time;
                if (isOnLeftWall)
                {
                    Rig.velocity = new Vector2(WallJumpSpeedX, WallJumpSpeedY);
                }
                else if (isOnRightWall)
                {
                    Rig.velocity = new Vector2(WallJumpSpeedX * -1, WallJumpSpeedY);
                }
            }
        }
    }
    #endregion

    #region 地形判定
    bool OnGround()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + GroundPointOffset, GroundJudgeSize, 0, GroundLayerMask);
        if (Coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool OnLeftWall()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + LeftWallPointOffset, WallJudgeSize, 0, WallLayerMask);
        if (Coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool OnRightWall()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + RightWallPointOffset, WallJudgeSize, 0, WallLayerMask);
        if (Coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + GroundPointOffset, GroundJudgeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + LeftWallPointOffset, WallJudgeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + RightWallPointOffset, WallJudgeSize);
    }
    #endregion

}

