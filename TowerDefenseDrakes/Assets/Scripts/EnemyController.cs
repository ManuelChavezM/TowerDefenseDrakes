using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Movment")]
    public List<Transform> waypoints = new List<Transform>();
    private int targetIndex = 1;
    public float movementSpeed = 4;
    public float roationSpeed = 6;
    public Animator animEnemy;
    
    [Header("Life")]
    public bool DieEnemybool = false;
    public float maxLife = 100;
    public float CurrentLife = 0;
    public Image fillLifeImage;
    private Transform canvasTransform;
    private Quaternion initLifeRotation;

    private void Awake()
    {
        canvasTransform = fillLifeImage.transform.parent.parent;
        initLifeRotation = canvasTransform.rotation;
        animEnemy = GetComponent<Animator>();
        animEnemy.SetBool("Walk", true);
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        waypoints.Clear();
        var rootWayPoints = GameObject.Find("WayPointContainer").transform;
        for(int i=0; i< rootWayPoints.childCount; i++)
        {
            waypoints.Add(rootWayPoints.GetChild(i));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentLife = maxLife;
    }


    // Update is called once per frame
    void Update()
    {

        if(DieEnemybool== false)
        {
            canvasTransform.rotation = initLifeRotation;
            Movement();
            LookAt();
            if (Input.GetKeyDown(KeyCode.A)){
                Takedamage(10f);
            }
        
        }

       
    }

    #region Movimiento y Rotacion
    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, movementSpeed * Time.deltaTime);
        var distance = Vector3.Distance(transform.position, waypoints[targetIndex].position);
        if (distance <= 0.1f)
        {
            if (targetIndex >= waypoints.Count - 1)
            {
                return;
            }

            targetIndex++;
        }

    }

    private void LookAt()
    {
        //obtener la direccion 
        var dir = waypoints[targetIndex].position - transform.position;
        //obtener rotacion segun la direccion
        var rootTarget = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rootTarget, roationSpeed * Time.deltaTime);
    }

    #endregion

    public void Takedamage(float damage)
    {
        var newVida = CurrentLife - damage;
        if(newVida <= 0)
        {
            DieEnemybool = true;
            DieEnemy();
        }
        CurrentLife = newVida;
        var fillValue = CurrentLife * 1 / 100;
        fillLifeImage.fillAmount = fillValue;
        AnimDamageEnemy();
    }

    public void AnimDamageEnemy()
    {
        animEnemy.SetTrigger("TakeDamage");
    }
    public void DieEnemy()
    {
        animEnemy.SetBool("Die",true);
        CurrentLife = 0;
        fillLifeImage.fillAmount = 0;
        StartCoroutine(DieEffect());
    }
    private IEnumerator DieEffect()
    {
        yield return new WaitForSeconds(2.5f);
        var FinalPositionY = transform.position.y - 5;
        Vector3 target = new Vector3(transform.position.x, FinalPositionY, transform.position.z);
        while(transform.position.y != FinalPositionY)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 1.5f * Time.deltaTime);
            yield return null;
        }

    }

}
