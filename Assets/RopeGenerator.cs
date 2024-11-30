using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    public GameObject RopePrefab;
    public float BaseDistance;
    public float HorizontalRandomRange;
    public float VerticalRandomRange;
    public GameObject LastRope;
    public Queue<GameObject> RopeQueue = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        RopeQueue.Enqueue(LastRope);
        for (int i = 0; i < 10; i++)
        {
            GenerateRope();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateRope()
    {
        float hOffset = LastRope.transform.position.z + BaseDistance + Random.Range(-HorizontalRandomRange, HorizontalRandomRange);
        float vOffset = LastRope.transform.position.y + Random.Range(-VerticalRandomRange, VerticalRandomRange);
        LastRope = Instantiate(RopePrefab, new Vector3(0, vOffset, hOffset), transform.rotation);
        RopeQueue.Enqueue(LastRope);
        if (RopeQueue.Count > 13)
        {
            Destroy(RopeQueue.Dequeue());
        }
    }
}
