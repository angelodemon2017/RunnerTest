using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
     RunnerPlayer rp;

    public AnimationCurve runCurveY;
    private float runTiming;
    public AnimationCurve curveGroundingY;
    private float groundingTimer;

    private void Start()
    {
        rp = GetComponent<RunnerPlayer>();
    }

    private void Update()
    {
        if (rp.IsPlay)
        {
            if (rp.Jumps > 0)
            {
                if (transform.localScale.x > 0.5f)
                {
                    transform.localScale =
                        new Vector3(transform.localScale.x * 0.99f,
                        transform.localScale.y,
                        transform.localScale.z * 0.99f);
                }
                groundingTimer = 0.5f;
                runTiming = 1f;
            }
            else
            {
                if (transform.localScale.x < 1f)
                {
                    transform.localScale =
                        new Vector3(transform.localScale.x * 1.025f,
                        transform.localScale.y,
                        transform.localScale.z * 1.025f);
                }

                if (groundingTimer > 0f)
                {
                    groundingTimer -= Time.deltaTime;

                    transform.localScale = new Vector3(transform.localScale.x, curveGroundingY.Evaluate(groundingTimer), transform.localScale.z);
                }
                else
                {
                    runTiming -= Time.deltaTime;
                    if (runTiming < 0f)
                    {
                        runTiming = 1f;
                    }
                    transform.localScale = new Vector3(transform.localScale.x, runCurveY.Evaluate(runTiming), transform.localScale.z);
                }
            }
        }
    }
}
