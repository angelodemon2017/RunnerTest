using UnityEngine;

public class EntityBlock : MonoBehaviour
{
    private BlockConfig _blockConfig;
    private float scale = 1f;
    public GameObject BonusObject;
    public Direction blockDirection;
    private bool WaitDelete = false;

    public void Instantiate(BlockConfig blockConfig, Direction dir)
    {
        blockDirection = dir;
        if (RunnerPlayer.Instance.IsPlay)
        {
            transform.localScale = Vector3.zero;
            scale = 0f;
        }

        _blockConfig = blockConfig;
    }

    private void Update()
    {
        if (scale < 1f)
        {
            scale += Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        if (WaitDelete)
        {
            PlayerEnterToNewBlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            RunnerPlayer.Instance.EnterToNewBlock(_blockConfig);
            Debug.Log("Player Enter");
            WaitDelete = true;
        }
    }

    private void PlayerEnterToNewBlock()
    {
        if (Vector3.Distance(transform.position, RunnerPlayer.Instance.transform.position) > 5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (BonusObject != null)
        {
            Destroy(BonusObject);
        }
    }
}