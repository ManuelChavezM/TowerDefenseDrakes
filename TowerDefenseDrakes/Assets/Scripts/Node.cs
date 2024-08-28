using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Node : MonoBehaviour
{
    public static Node selectedNode;
    private Animator anim;
    private bool OnSelected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if(selectedNode && selectedNode != this)
        {
            selectedNode.OnCloseSelection();
        }
        selectedNode = this;
        OnSelected = !OnSelected;
        if (OnSelected)
        {
            TowerRequestManager.instance.OnOpenRequesPanel();
        }
        else
        {
            TowerRequestManager.instance.OnCloseRequesPanel();
        }
        anim.SetBool("OnSelected", OnSelected);
    }
    public void OnCloseSelection()
    {
        OnSelected = false;
        anim.SetBool("OnSelected", OnSelected);
    }
}
