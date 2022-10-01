using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, Node<int>
{
    public int ID = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector3 getPosition()
    {
        return this.transfrom.position;
    }

    public int getValue()
    {
        return ID;
    }
    public void setValue(int data)
    {
        ID = data;
    }

    public Color getValue()
    {
        return Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
