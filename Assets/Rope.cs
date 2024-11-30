using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform[] ropeNodes;

    /// <summary>
    /// 根據玩家附在某個節點上的位置，固定其以下的節點關節
    /// </summary>
    /// <param name="index"></param>
    public void FixNodesAbove(int index)
    {

        for (int i = index; i < ropeNodes.Length; i++)
        {
            ropeNodes[i].GetComponent<HingeJoint>().useLimits = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
