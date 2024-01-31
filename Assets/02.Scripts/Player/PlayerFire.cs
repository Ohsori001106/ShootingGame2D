using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // [총알 발사 제작]
    // 목표: 총알을 만들어서 발사하고 싶다.
    // 속성:
    // - 총알 프리팹
    // - 총구
    // 구현 순서
    // 1. 발사 버튼을 누르면
    // 2. 프리팹으로부터 총알을 동적으로 만들고,
    // 3. 만든 총알의 위치를 총구의 위치로 바꾼다.

    [Header("총알 프리팹")]
    public GameObject BulletPrefab;     // 총알 프리팹
    public GameObject SubBulletPrefab;  // 보조 총알 프리팹

    // 목표: 태어날 때 풀에다가 총알을 (풀 사이즈)개 생성한다.
    // 속성: 
    // - 풀 사이즈
    public int PoolSize = 20;
    // - 오브젝트(총알) 풀
    public List<Bullet> _bulletPool;
    //public List<GameObject> _subbulletPool;
    // 순서:
    // 1. 태어날 때: Awake
    private void Awake()
    {
        // 2. 오브젝트 풀 할당해주고.
        _bulletPool = new List<Bullet>();
        //_subbulletPool = new List<GameObject>();
        // 3. 총알 프리팹으로부터 총알을 풀 사이즈만큼 생성해준다.
        // 3-1. 메인 총알
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);


            // 4. 생성한 총알을 풀에다가 넣는다.
            _bulletPool.Add(bullet.GetComponent<Bullet>());


            bullet.SetActive(false);

        }
        // 3-2. 서브 총알
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(SubBulletPrefab);
            _bulletPool.Add(bullet.GetComponent<Bullet>());
            bullet.SetActive(false);
        }

    }


    [Header("총구들")]
    public List<GameObject> Muzzles;
    public List<GameObject> SubMuzzles;


    //public GameObject[] MainMuzzle;     // 총구들
    //public GameObject[] SubMuzzle;  // 보조 총구들

    [Header("타이머")]
    public float Timer = 10f;
    public const float COOL_TIME = 0.6f;

    public float BoomTimer = 0f;
    public const float BOOM_COOL_TIME = 5f;


    [Header("자동 모드")]
    public bool AutoMode = false;

    public AudioSource FireSource;


    // 생성할 붐 프리팹
    public GameObject BoomPrefab;



    private void Start()
    {
        Debug.Log(Application.dataPath);
        Timer = 0f;
        BoomTimer = 0f;
        AutoMode = false;


    }

    void Update()
    {
        // 타이머 계산
        Timer -= Time.deltaTime;
        BoomTimer -= Time.deltaTime;
        CheckAutoMode();
        bool ready = AutoMode || Input.GetKeyDown(KeyCode.Space);
        if (Timer <= 0 && ready)
        {
            Fire();
        }
        
        

        
        

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Boom();
        }
        
        
    }

    private void Boom()
    {
          
        // 붐 타이머가 0보다 같거나 작고 && 3번 버튼을 누르면
        if (BoomTimer <= 0f  )
        {
            // 붐 타이머 시간을 다시 쿨타임으로..
            BoomTimer = BOOM_COOL_TIME;

            // 붐 프리팹을 씬으로 생성한다.
            GameObject boomObject = Instantiate(BoomPrefab);
            boomObject.transform.position = Vector2.zero;
            boomObject.transform.position = new Vector2(0, 0);
            boomObject.transform.position = new Vector2(0, 1.6f);
        }
    }
    private void CheckAutoMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("자동 공격 모드");
            AutoMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("수동 공격 모드");
            AutoMode = false;
        }
    }
    private void OnClickAutoMode()
    {
        if (AutoMode == false)
        {
            Debug.Log("자동 공격 모드");
            AutoMode = true;
        }
        else if (AutoMode==true)
        {
            Debug.Log("수동 공격 모드");
            AutoMode = false;
        }
    }

    private void Fire()
    {
        // 1. 타이머가 0보다 작은 상태에서 발사 버튼을 누르거나 자동 공격 모드면
            FireSource.Play();

            // 타이머 초기화
            Timer = COOL_TIME;

            // 2. 프리팹으로부터 총알을 만든다.
            //GameObject bullet1 = Instantiate(BulletPrefab);
            //GameObject bullet2 = Instantiate(BulletPrefab);

            // 3. 만든 총알의 위치를 총구의 위치로 바꾼다.
            //bullet1.transform.position = Muzzle.transform.position;
            //bullet2.transform.position = Muzzle2.transform.position;


            // 목표: 총구 개수 만큼 총알을 풀에서 꺼내쓴다,
            // 순서:


            for (int i = 0; i < Muzzles.Count; i++)
            {
                // 1. 꺼져있는 총알을 찾아 꺼낸다
                Bullet bullet = null;
                foreach (Bullet b in _bulletPool)
                {
                    //만약에 꺼져있다면
                    if (b.gameObject.activeInHierarchy == false && b.BType==BulletType.Main)
                    {
                        bullet = b;
                        break;  //찾았기 때문에 그 뒤까지 찾을 필요가 없다
                    }
                }
                // 2. 꺼낸 총알의 위치를 각 총구의 위치로 바꾼다
                bullet.transform.position = Muzzles[i].transform.position;
                // 3. 총알을 킨다(발사)
                bullet.gameObject.SetActive(true);
            }
            for (int i = 0; i < SubMuzzles.Count; i++)
            {
                // 1. 꺼져있는 총알을 찾아 꺼낸다
                Bullet bullet = null;
                foreach (Bullet b in _bulletPool)
                {
                    //만약에 꺼져있다면
                    if (b.gameObject.activeInHierarchy == false && b.BType == BulletType.Sub)
                    {
                        bullet = b;
                        break;  //찾았기 때문에 그 뒤까지 찾을 필요가 없다
                    }
                }
                // 2. 꺼낸 총알의 위치를 각 총구의 위치로 바꾼다
                bullet.transform.position = SubMuzzles[i].transform.position;
                // 3. 총알을 킨다(발사)
                bullet.gameObject.SetActive(true);
            }
        
    }

    // 총알 발사
    public void OnClickXButton()
    {
        Debug.Log("X버튼이 클릭되었습니다");
        if (Timer <= 0 )
        {
            Fire();
        }
        

    }
    // 자동 공격 on/off
    public void OnClickYButton()
    {
        Debug.Log("Y버튼이 클릭되었습니다");
        OnClickAutoMode();
    }
    // 궁극기 사용
    public void OnClickBButton()
    {
        Debug.Log("B버튼이 클릭되었습니다");
        Boom();
    }
}


