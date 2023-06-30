using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayer : MonoBehaviour
{
    public static RunnerPlayer Instance;

    #region CONSTS
    private const int COUNT_BLOCK_TO_WIN = 100;
    private const float BASE_SPEED = 0.009f;
    private const int MAX_JUMPS = 2;
    private const float MULTIPLY_SPEED_BONUS = 2;
    private const float MULTIPLY_SLOWLY = 0.5F;
    #endregion

    #region CameraAndMove
    private Direction PreviusDirect;
    private Direction DirectionRun;
    public Transform PivotCamera;
    private Rigidbody rb;
    public bool IsPlay;
    private float speed => BASE_SPEED *
        (timerSlowly > 0 ? MULTIPLY_SLOWLY : 1f) *
        (TimerSpeedBonus > 0f ? MULTIPLY_SPEED_BONUS : 1f);

    public int Jumps = 0;
    private bool isCanJump => Jumps < MAX_JUMPS;

    public AnimationCurve AnimationFlashing;
    private Renderer renderer;
    #endregion

    #region GamePlayFields
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            UIController.Instance.UpdateGameplay();
        }
    }
    public int CountAllBlocks = 0;

    private Dictionary<string, int> stats = new Dictionary<string, int>();
    #endregion

    #region Timers
    private float TimerForDirectCamera = 1f;
    private float damageTimer = 1f;
    public float timerSlowly = 0f;
    public float TimerSpeedBonus = 0f;
    public float TimerSpeedShow => TimerSpeedBonus;

    public float TimerShieldBonus = 0f;
    public float TimerShieldShow => TimerShieldBonus;
    #endregion

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        renderer = GetComponent<Renderer>();
    }

    public void Play()
    {
        DirectionRun = Direction.Zpositive;
        timerSlowly = 2f;
        CountAllBlocks = 0;
        stats.Clear();
        TimerSpeedBonus = 0f;
        TimerShieldBonus = 0f;
        renderer.material.color = Color.white;
        Health = 3;
        transform.position = Vector3.up * 2;
        IsPlay = true;
        rb.useGravity = true;
    }

    void Update()
    {
        if (IsPlay)
        {
            transform.position += DirectionRun.GetVector3Direction() * speed;
        }
        PivotCamera.position = new Vector3(transform.position.x, 2f, transform.position.z);
        Timers();
    }

    private void Timers()
    {
        if (damageTimer < 1f)
        {
            DamageFlashing();
        }
        if (TimerSpeedBonus > 0f)
        {
            TimerSpeedBonus -= Time.deltaTime;
        }
        if (TimerShieldBonus > 0f)
        {
            TimerShieldBonus -= Time.deltaTime;
        }
        if (timerSlowly > 0f)
        {
            timerSlowly -= Time.deltaTime;
        }
        if (TimerForDirectCamera < 1f)
        {
            TimerForDirectCamera += Time.deltaTime;
            PivotCamera.rotation = Quaternion.Lerp(PreviusDirect.GetQuaternionirection(), DirectionRun.GetQuaternionirection(), TimerForDirectCamera);
        }
    }

    private void DamageFlashing()
    {
        damageTimer += Time.deltaTime;
        Color color = renderer.material.color;
        color.b = 1 - AnimationFlashing.Evaluate(damageTimer);
        color.g = 1 - AnimationFlashing.Evaluate(damageTimer);
        renderer.material.color = color;
        if (damageTimer >= 1f)
        {
            if (Health > 0)
            {
                RestoreAfterDamage();
            }
            else
            {
                UIController.Instance.OpenFail(stats);
            }
        }
    }

    public void JumpTap()
    {
        if (isCanJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5f, rb.velocity.z);
            Jumps++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case Tags.Ground:
                Jumps = 0;
                break;
            case Tags.Hole:
            case Tags.BigHole:
                Damage(true);
                break;
            case Tags.Rotater:
                PreviusDirect = DirectionRun;
                DirectionRun = collision.gameObject.GetComponentInParent<EntityBlock>().blockDirection;
                PivotCamera.rotation = DirectionRun.GetQuaternionirection();
                transform.position = new Vector3(collision.transform.parent.position.x, transform.position.y, collision.transform.parent.position.z);
                TimerForDirectCamera = 0f;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case Tags.BonusHealth:
                HealthBonus();
                Destroy(other.gameObject);
                break;
            case Tags.BonusSpeed:
                SpeedBonus();
                Destroy(other.gameObject);
                break;
            case Tags.BonusShield:
                ShieldBonus();
                Destroy(other.gameObject);
                break;
            case Tags.Fence:
            case Tags.Saw:
                Damage(false, other.gameObject);
                break;
            default:
                break;
        }
    }

    private void SpeedBonus()
    {
        TimerSpeedBonus += LevelController.Instance.bonusConfig.GetByKey(Tags.BonusSpeed).BonusValue;
    }

    private void HealthBonus()
    {
        Health += LevelController.Instance.bonusConfig.GetByKey(Tags.BonusHealth).BonusValue;
    }

    private void ShieldBonus()
    {
        TimerShieldBonus += LevelController.Instance.bonusConfig.GetByKey(Tags.BonusShield).BonusValue;
    }

    private void Damage(bool isFall = false, GameObject barrier = null)
    {
        if (TimerShieldBonus > 0f)
        {
            if (barrier != null)
            {
                Debug.Log("TryDestroy");
                Destroy(barrier);
            }
            if (isFall)
            {
                RestoreAfterDamage();
            }
        }
        else
        {
            IsPlay = false;
            damageTimer = 0f;
            Health--;
        }
    }

    public void RestoreAfterDamage()
    {
        renderer.material.color = Color.white;
        transform.position = new Vector3(0f, 1f, transform.position.z + 1.5f);
        IsPlay = true;
    }

    public void EnterToNewBlock(BlockConfig _blockConfig)
    {
        LevelController.Instance.AddingPath();

        CountAllBlocks++;
        UIController.Instance.UpdateGameplay();
        if (CountAllBlocks > COUNT_BLOCK_TO_WIN)
        {
            IsPlay = false;
            UIController.Instance.OpenWin(stats);
        }

        switch (_blockConfig.blockType)
        {
            case BlockType.hole:
            case BlockType.bighole:
            case BlockType.saw:
            case BlockType.fence:
                AddStat(_blockConfig.blockType.ToString());
                break;
        }
    }

    private void AddStat(string key)
    {
        if (stats.ContainsKey(key))
        {
            stats[key]++;
        }
        else
        {
            stats.Add(key, 1);
        }
    }
}