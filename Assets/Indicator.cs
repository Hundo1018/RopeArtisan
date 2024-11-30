using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Rigidbody rb;             // 角色剛體
    public float jumpForce = 15f;    // 與 RopeSwing 的 jumpPower 保持一致
    public ForceMode forceMode = ForceMode.Impulse; // 力模式
    public int resolution = 30;      // 軌跡線解析度（點數）
    public LineRenderer lineRenderer;
    public bool trajectoryFrozen = false;

    void Update()
    {
        if (trajectoryFrozen)
            return;

        // 計算初速度
        Vector3 initialVelocity = CalculateInitialVelocity();

        // 繪製預測軌跡
        DrawTrajectory(rb.position, initialVelocity);
    }

    Vector3 CalculateInitialVelocity()
    {
        // 1. 直接使用 Rigidbody 的當前速度作為基礎
        Vector3 swingVelocity = rb.velocity;

        // 2. 計算跳躍方向（基於擺動速度和向上方向）
        Vector3 jumpDirection = (swingVelocity.normalized + Vector3.up).normalized;

        // 3. 根據 ForceMode 計算初速度，僅使用當前速度（避免過度放大跳躍力）
        if (forceMode == ForceMode.Impulse)
        {
            // 加入跳躍力但不過度放大速度，保持原有跳躍力的一致性
            return swingVelocity + jumpDirection * jumpForce / rb.mass;
        }
        else if (forceMode == ForceMode.VelocityChange)
        {
            // 直接加上跳躍方向速度（不再加入質量調整）
            return swingVelocity + jumpDirection * jumpForce;
        }
        return swingVelocity; // 默認情況
    }

    void DrawTrajectory(Vector3 startPosition, Vector3 initialVelocity)
    {
        Vector3[] points = new Vector3[resolution];
        float timeStep = Time.fixedDeltaTime; // 與物理引擎同步

        for (int i = 0; i < resolution; i++)
        {
            float t = i * timeStep; // 當前時間
            points[i] = CalculatePointAtTime(startPosition, initialVelocity, t);
        }

        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(points);
    }

    Vector3 CalculatePointAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        // 根據公式 s = ut + 0.5at² 計算位移
        Vector3 gravity = Physics.gravity; // Unity 的重力
        return startPosition + initialVelocity * time + 0.5f * gravity * time * time;
    }
}
