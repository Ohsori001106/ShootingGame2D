using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int _health = 10;

    private void Start()
    {
        /*// GetComponent<컴포넌트 타입>(); -> 게임 오브젝트의 컴포넌트를 가져오는 메서드
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;

        //Transform tr = GetComponent<Transform>();
        //tr.position = new Vector2(0f, -2.7f);
        transform.position = new Vector2(0f, -2.7f);

        PlayerMove playerMove = GetComponent<PlayerMove>();
        Debug.Log(playerMove.Speed);
        playerMove.Speed = 5f;
        Debug.Log(playerMove.Speed);*/
    }

    public int GetPlayerHealth()
    {
        return _health;
    }

    public void MinusPlayerHealth(int health)
    {
        _health -= health;
        Debug.Log($"HP: {_health}");
        // 플레이어 체력이 적다면..
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPlayerHealth(int health)
    {
        _health = health;

    }
    public void AddHealthPlus(int health)
    {
        _health += health;
        Debug.Log($"HP: {_health}");
    }







}

