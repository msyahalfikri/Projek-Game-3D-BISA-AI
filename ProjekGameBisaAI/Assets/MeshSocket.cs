using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketID socketID;
    public HumanBodyBones bone;
    Transform attachPoint;
    public Vector3 offset;
    public Vector3 rotation;
    public Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponentInParent<Animator>();
        attachPoint = new GameObject("socket" + socketID).transform;
        attachPoint.SetParent(animator.GetBoneTransform(bone));
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
        attachPoint.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}
