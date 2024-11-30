using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeSwing : MonoBehaviour
{
    public Transform[] ropeNodes; // 繩索節點（圓柱或空物件陣列）
    public Indicator indicator;
    public float climbSpeed = 5f;
    public int currentNodeIndex = 0; // 當前節點索引
    public Rigidbody characterRigidbody;
    public float swingPower = 10f;
    public float jumpPower = 15f;
    public ForceMode forceMode;
    public RopeGenerator ropeGenerator;
    //騰空墜落時間
    public float fallFailTime = 10f;
    public float currentFallTime = 0f;
    private FixedJoint joint;

    void Update()
    {
        if (joint == null)
        {
            // 如果沒有繩索連接，騰空墜落超過一定時間，遊戲失敗
            //開始計算墜落時間
            currentFallTime += Time.deltaTime;
            if (currentFallTime >= fallFailTime)
            {
                //遊戲失敗
                SceneManager.LoadScene(0);
            }
        }
        if (joint != null)
        {
            float climbInput = Input.GetAxis("Vertical");

            if (climbInput > 0 && currentNodeIndex > 0)
            { // 向上爬
                MoveToNode(currentNodeIndex - 1);
            }
            else if (climbInput < 0 && currentNodeIndex < ropeNodes.Length - 1)
            { // 向下爬
                MoveToNode(currentNodeIndex + 1);
            }

            // 玩家控制擺盪(推)

            float jumpInput = Input.GetMouseButton(0) ? 1f : 0f;
            Vector3 swingForce = new Vector3(0, 0, jumpInput) * swingPower;
            characterRigidbody.AddForce(swingForce, forceMode);

            // 玩家在繩索上，上下移動
        }
        // 玩家跳躍
        if (Input.GetKeyDown(KeyCode.Space) && joint != null)
        {
            indicator.trajectoryFrozen = true;
            ReleaseRope();
            Jump();
            ropeGenerator.GenerateRope();
        }
    }
    void MoveToNode(int targetNodeIndex)
    {
        currentNodeIndex = targetNodeIndex;
        Transform targetNode = ropeNodes[currentNodeIndex];
        // 平滑移動角色
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetNode.position,
            climbSpeed * Time.deltaTime
        );
    }
    void ReleaseRope()
    {
        if (joint != null)
        {
            Destroy(joint);
        }
    }

    void Jump()
    {
        Vector3 jumpDirection = (characterRigidbody.velocity.normalized + Vector3.up).normalized;
        characterRigidbody.AddForce(jumpDirection * jumpPower, ForceMode.Impulse);
    }


    private void OnTriggerEnter(Collider other)
    {
        // 偵測是否抓住繩索
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("Rope"))
        {
            // 更新繩索節點
            Rope rope = otherObject.GetComponentInParent<Rope>();

            // 繩索不能抓自己
            if (rope.ropeNodes == this.ropeNodes)
                return;

            // 繩索接觸時添加FixedJoint
            if (!gameObject.TryGetComponent(out FixedJoint _))
            {
                joint = gameObject.AddComponent<FixedJoint>();
                indicator.trajectoryFrozen = false;

            }
            joint.connectedBody = other.attachedRigidbody;


            if (otherObject.TryGetComponent(out HingeJoint otherJoint))
            {
                Transform touchTransform = otherJoint.transform;
                currentNodeIndex = System.Array.IndexOf(rope.ropeNodes, touchTransform);
                ropeNodes = rope.ropeNodes;
                rope.FixNodesAbove(currentNodeIndex);
            }
        }
    }
    void FindClosestNode(Transform rope)
    {
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < ropeNodes.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, ropeNodes[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentNodeIndex = i;
            }
        }
    }
}
