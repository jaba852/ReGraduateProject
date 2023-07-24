using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
public class BossAttackSpawner : MonoBehaviour
{
    public GameObject BossAttackPrefabs;
    public GameObject BossAttackPrefabsBig;
    public Vector3 PatternA;
    public Vector3 PatternB;
    public Vector3 PatternC;
    public Vector3 spacing;

    public Animator anim;
    public bool isidle = true;
    public bool ispatternL = false;
    public bool ispatternR = false;
    public bool ispatternM = false;

    public float attackDelay = 1f;
    public int repeatCount = 10;
    public float patternDelay = 3f;

    private int patternCount;


    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject LeftHandAtk;
    public GameObject RightHandAtk;
    public BossStatus LeftStun;
    public BossStatus RightStun;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        LeftArm.SetActive(false);
        RightArm.SetActive(false);
        LeftHandAtk.SetActive(false);
        RightHandAtk.SetActive(false);

    }

    private void Start()
    {
        patternCount = 0;
        anim.SetBool("isIdle", isidle);
        StartCoroutine(RandomPattern());
    }
    private void Update()
    {
        if (RightStun.isNeutralize)
        {
            LeftArm.SetActive(false);
            RightArm.SetActive(false);
            anim.SetBool("isStun", RightStun.isNeutralize);
        }
        if (LeftStun.isNeutralize)
        {
            LeftArm.SetActive(false);
            RightArm.SetActive(false);
            anim.SetBool("isStun", LeftStun.isNeutralize);
        }

    }
    private void StunRecover()
    {
        LeftStun.isNeutralize = false;
        RightStun.isNeutralize = false;
        LeftStun.currentNeutralizeGauge = 100;
        RightStun.currentNeutralizeGauge = 100;
        anim.SetBool("isStun", LeftStun.isNeutralize);
    }
    private void RestartPattern()
    {
        StartCoroutine(RandomPattern());
    }

    private IEnumerator RandomPattern()
    {

        yield return new WaitForSeconds(patternDelay);
        Debug.Log("패턴 카운트" + patternCount);
        if (patternCount == 5)
        {
            PatternCanim();
            patternCount = 0;
        }
        else
        {
        int randomIndex = UnityEngine.Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    PatternAanim();
                    patternCount++;
                    break;
                case 1:
                    PatternBanim();
                    patternCount++;
                    break;
            }
        }
        
    }

    private void PatternAanim()
    {
        ispatternL = true;
        anim.SetBool("LeftPattern", ispatternL);
    }
    private void PatternBanim()
    {
        ispatternR = true;
        anim.SetBool("RightPattern", ispatternR);
    }
    private void PatternCanim()
    {
        ispatternM = true;
        anim.SetBool("CenterPattern", ispatternM);
    }

    private IEnumerator PatternAattack()
    {
        LeftArm.SetActive(true);
        for (int i = 0; i < repeatCount; i++)
        {
            Instantiate(BossAttackPrefabs, PatternA + spacing * i, quaternion.identity);
            Instantiate(BossAttackPrefabs, PatternA - spacing * i - new Vector3Int(3, 0, 0), quaternion.identity);
            yield return new WaitForSeconds(attackDelay);
        }

    }
    private IEnumerator PatternBattack()
    {
        RightArm.SetActive(true);
        for (int i = 0; i < repeatCount; i++)
        {
            Instantiate(BossAttackPrefabs, PatternB - spacing * i, quaternion.identity);
            Instantiate(BossAttackPrefabs, PatternB + spacing * i + new Vector3Int(3, 0, 0), quaternion.identity);
            yield return new WaitForSeconds(attackDelay);
        }
    }
    private void PatternCattack()
    {
        Instantiate(BossAttackPrefabsBig, PatternC, quaternion.identity);
        Instantiate(BossAttackPrefabsBig, new Vector3(-PatternC.x, PatternC.y, PatternC.z), quaternion.identity);

    }
    private void PatternAanimEnd()
    {
        ispatternL = false;
        anim.SetBool("LeftPattern", ispatternL);
        StartCoroutine(RandomPattern());

    }
    private void PatternBanimEnd()
    {
        ispatternR = false;
        anim.SetBool("RightPattern", ispatternR);
        StartCoroutine(RandomPattern());
    }
    private void PatternCanimEnd()
    {
        ispatternM = false;
        anim.SetBool("CenterPattern", ispatternM);
        StartCoroutine(RandomPattern());
    }

    private void LeftAttackRBoff()
    {
        
        LeftArm.SetActive(false);
    }
    private void RightAttackRBoff()
    {
        RightArm.SetActive(false);

    }

    private void LeftHandAttack()
    {
        LeftHandAtk.SetActive(true);
    }
    private void LeftHandAttackF()
    {
        LeftHandAtk.SetActive(false);
    }
    private void RightHandAttack()
    {
        RightHandAtk.SetActive(true);
    }
    private void RightHandAttackF()
    {
        RightHandAtk.SetActive(false);
    }
}
