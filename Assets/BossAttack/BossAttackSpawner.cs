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

    public int lastP;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        lastP = 0;

        anim.SetBool("isIdle", isidle);
        StartCoroutine(RandomPattern());
    }

    private IEnumerator RandomPattern()
    {
        Debug.Log(lastP);

        yield return new WaitForSeconds(patternDelay);

        if (lastP == 0)
        {
            Debug.Log("경우의수1");
            int randomIndex = UnityEngine.Random.Range(0, 3);
            switch (randomIndex)
            {
                case 0:
                    PatternAanim();
                    break;
                case 1:
                    PatternBanim();
                    break;
                case 2:
                    PatternCanim();
                    break;
            }

        }
        else if (lastP == 1)
        {
            Debug.Log("경우의수2");
            int randomIndex = UnityEngine.Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    PatternBanim();
                    break;
                case 1:
                    PatternCanim();
                    break;
            }

        }
        else if (lastP == 2)
        {
            Debug.Log("경우의수3");
            int randomIndex = UnityEngine.Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    PatternAanim();
                    break;
                case 1:
                    PatternCanim();
                    break;
            }

        }
        else if (lastP == 3)
        {
            Debug.Log("경우의수4");
            int randomIndex = UnityEngine.Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    PatternAanim();
                    break;
                case 1:
                    PatternBanim();
                    break;
            }

        }

    }

    private void PatternAanim()
    {
        lastP = 1;

        ispatternL = true;
        anim.SetBool("LeftPattern", ispatternL);
    }
    private void PatternBanim()
    {
        lastP = 2;

        ispatternR = true;
        anim.SetBool("RightPattern", ispatternR);
    }
    private void PatternCanim()
    {
        lastP = 3;

        ispatternM = true;
        anim.SetBool("CenterPattern", ispatternM);
    }

    private IEnumerator PatternAattack()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            Instantiate(BossAttackPrefabs, PatternA + spacing * i, quaternion.identity);
            Instantiate(BossAttackPrefabs, PatternA - spacing * i - new Vector3Int(3, 0, 0), quaternion.identity);
            yield return new WaitForSeconds(attackDelay);
        }

    }
    private IEnumerator PatternBattack()
    {
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

}
