using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class DeepOneAttackManager : MonoBehaviour
{
    private Animator animator;
    public bool enemyMove = true;
    public bool attackStop = false;
    public bool enemydead = false;
    public int count = 1;
    private Rigidbody2D rb;
    private WarriorStatus warriorStatus;
    private DeepOneMovement DeeponeMvmt;


    public float attackRange = 2.0f; // ���� �Ÿ�



    public GameObject warningPrefab;    // ��� ǥ�ÿ� ������
    private Transform playerTransform;   // �÷��̾� Ʈ������
    public float warningmoveSpeed = 5f;        // ��� ǥ�� �̵� �ӵ�

    public SpriteRenderer energyPointSpriteRenderer;   // ����ü ��������Ʈ



    private GameObject warning;         // ��� ǥ�� ������Ʈ
    private Vector2 targetPosition;     // �÷��̾� ĳ���� ��ġ
    private bool isWarningActive = false; // ��� ǥ�� Ȱ��ȭ ����
    private bool isInAttackState = false; // DeepOne�� ���� �������� ����

    private bool DeepOneattacking = true; // DeepOne ħ �����

    public AudioClip EnemyattacksoundClip; // Enemy ���� ���� Ŭ��
    public AudioClip PlayerHitsoundClip; // �ǰ� ���� Ŭ��
    public AudioClip DeepOnesoundClip; // ���� ���� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private bool isEnemyAttackSoundPlaying = false; // Enemy ���� ���� ��� ���θ� �����ϴ� ����
    private bool isPlayerHitSoundPlaying = false; // �ǰ� ���� ��� ���θ� �����ϴ� ����
    private bool isDeepOneAttackSoundPlaying = false; // ���� ���� ���� ��� ���θ� �����ϴ� ����




    public void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        warriorStatus = FindObjectOfType<WarriorStatus>();
        DeeponeMvmt = GetComponent<DeepOneMovement>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform; // ã�� ���� ������Ʈ�� Transform�� �����ɴϴ�.
        }

        // ���� ������Ʈ�� AudioSource ������Ʈ�� �߰�
        audioSource = GetComponent<AudioSource>();

    }

    public void UpdateAttack(Transform playerTransform)
    {
    

        if (attackStop == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Enemy_attack") && stateInfo.normalizedTime >= 1.0f)
            {
                enemyMove = true;
                animator.SetBool("isEnemyAttack", false);
                UnityEngine.Debug.Log("isEnemyAttack, false");
            }
        }
    }

    public void StartAttack(Transform playerTransform)
    {
        UnityEngine.Debug.Log("Warrior�� �߰�!");
        Vector2 direction = playerTransform.position - transform.position;
        if (enemydead == false)
        {
            animator.SetFloat("EnemyMoveX", direction.x);
            animator.SetFloat("EnemyMoveY", direction.y);
        }

        animator.SetBool("isEnemyAttack", true);
        enemyMove = false;
        attackStop = false;
    }


    public void StopAttack()
    {
        if (enemydead == false)
        {
            attackStop = true;
            count = 1;
            UnityEngine.Debug.Log("count �ʱ�ȭ");
        }
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!");
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 relativeVelocity = playerRigidbody.velocity - rb.velocity;

            if (relativeVelocity.magnitude > 2.0f)
            {
                Vector2 force = relativeVelocity * 10.0f;
                rb.AddForce(-force);
                playerRigidbody.AddForce(force);
            }

            if (isInAttackState) // DeepOne ����ü�� �÷��̾� ĳ���Ͱ� �¾�����
            {
                warriorStatus.TakeDamagePlayer(10);
                UnityEngine.Debug.Log("DeepOne ������");
                isInAttackState = false;
            }

        }

    }





    IEnumerator ResetSoundFlags()
    {
        yield return new WaitForSeconds(0.1f); // ������ �����̸� �����մϴ�. ���⼭�� 0.1�ʸ� ����Ͽ����ϴ�.

        isEnemyAttackSoundPlaying = false;
        isPlayerHitSoundPlaying = false;
        isDeepOneAttackSoundPlaying = false;

    }


    public void Enemyreaction()
    {
        animator.SetBool("isEnemyHit", false);
    }


    public void DeepOneattack()
    {
        if (!isWarningActive)
        {
            isWarningActive = true;

            if (warningPrefab != null) // warningPrefab�� null���� Ȯ��
            {

                // ��� ǥ�� ���� �� �ʱ�ȭ
                warning = Instantiate(warningPrefab, transform.position, Quaternion.identity);
                targetPosition = playerTransform.position;
                StartCoroutine(MoveAndAttack());
            }
        }
    }

    private IEnumerator MoveAndAttack()
    {
        float elapsedTime = 0f;
        DeepOneattacking = true;


        while (DeepOneattacking)
        {
            // ��� ǥ�ø� �÷��̾� ��ġ�� �̵�
            warning.transform.position = Vector2.MoveTowards(warning.transform.position, targetPosition, warningmoveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            // 2�� �Ŀ� Ż��
            if (elapsedTime >= 1f)
            {
                break;
            }
            yield return null;
        }

        // ��� ǥ�� ����
        Destroy(warning);
        isWarningActive = false;

        // energyPointSpriteRenderer ��ȯ �� ����
        Vector2 spawnPosition = targetPosition + new Vector2(0f, 1.5f);
        GameObject energyPoint = Instantiate(energyPointSpriteRenderer.gameObject, spawnPosition, Quaternion.identity);
        // energyPoint�� ��� �ӵ� ���� (0.5�� ������ ��)
        energyPoint.GetComponent<Animator>().speed = 2f;
        StartCoroutine(MoveAndDestroyEnergyPoint(energyPoint, spawnPosition));
    }

    private IEnumerator MoveAndDestroyEnergyPoint(GameObject energyPoint, Vector2 spawnPosition)
    {
        // energyPoint�� spawnPosition ��ġ�� ��ȯ
        energyPoint.transform.position = spawnPosition;

        // energyPoint�� Animator ������Ʈ ��������
        Animator energyPointAnimator = energyPoint.GetComponent<Animator>();


        // energyPoint �ִϸ��̼��� ��� �ð� �������� - ���� ���� �ȵ���
        // float animationDuration = energyPointAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        StartCoroutine(PlayDeepOneshotSound());


        // energyPoint �ִϸ��̼��� ��� ����� ������ ��ٸ�- ���� �ȵ��ͼ� animationDuration��� ���� ������ �־������
        yield return new WaitForSeconds(3f);

        // energyPoint ����
        Destroy(energyPoint);


    }

    IEnumerator PlayDeepOneshotSound()
    {
        yield return new WaitForSeconds(1.5f); // ���� ��� ������ f�ʷ� �����մϴ�.
        if (!isDeepOneAttackSoundPlaying)
        {
            audioSource.PlayOneShot(DeepOnesoundClip);
            yield return new WaitForSeconds(1.5f); // ���尡 f�� ���� ����ǵ��� �����մϴ�.
            audioSource.Stop(); // ���� ����� �����մϴ�.
            isDeepOneAttackSoundPlaying = true;
            StartCoroutine(ResetSoundFlags());
        }
    }



    public void DeepOneshot()
    {
        DeepOneattacking = false;
        Destroy(warning);
    }






}
