using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, BaseNode<int>
{
    public int ID = -1;
    public float offsetRatio = 1.0f;
    private float offset = 0.0f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    public void setOffset(float _offset)
    {
        offset = _offset;
    }

    public void StartSeq() { anim.enabled = true; }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    public int getValue()
    {
        return ID;
    }
    public void setValue(int data)
    {
        ID = data;
    }

    public Color getColor()
    {
        return Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, offsetRatio * offset, transform.localPosition.z);
    }
}
