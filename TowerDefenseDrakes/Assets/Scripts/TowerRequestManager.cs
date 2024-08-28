using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class TowerRequestManager : MonoBehaviour
{
    public List<Tower> towers = new List<Tower>();
    private Animator anim;
    public static TowerRequestManager instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(instance);
        anim = GetComponent<Animator>();
    }

    public void OnOpenRequesPanel()
    {
        anim.SetBool("IsOpen", true);
    }
    public void OnCloseRequesPanel()
    {
        anim.SetBool("IsOpen", false);
    }

    public void RequestTowerBuy(string towerName)
    {
        var tower = towers.Find(x => x.towerName == towerName);
        var towerGo = Instantiate(tower, Node.selectedNode.transform.position, tower.transform.rotation);
        OnCloseRequesPanel();
        Node.selectedNode.OnCloseSelection();
        Node.selectedNode = null;
    }
}
