using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    int layerNum;
    bool isSet;
    float stayZPos;
    float stayXPos; 
    float stayDis; //기준이 되는 길이
    float beforeDis;

    float changePosX;
    float playerDistance;
    bool isFindPlayer;

    Transform playerPos;

    JellySetting jellySetting;
    TileSetting tileSetting;
    ObstacleSetting obstacleSetting;

    [Range(0, 0.5f)]
    [SerializeField] float CompulsionDistance;

    [SerializeField] float MaxDistance; //최대 검사 길이
    [SerializeField] float yPosSpeed;
    [SerializeField] BackGroundScrolling[] backGround;
    // Update is called once per frame
    private void Start()
    {
        stayZPos = transform.position.z;
        stayXPos = transform.position.x;
        if (MaxDistance == 0)
            MaxDistance = 15;
        if(yPosSpeed <= 0)
        {
            yPosSpeed = 1.2f;
        }
        if (CompulsionDistance <= 0)
            CompulsionDistance = 0.05f;
        int layer = LayerMask.NameToLayer("Tile");
        layerNum = 1 << layer;
        Init();
        GameObject player = GameObject.Find("PlayerCharacter");
        if(player != null)
        {
            isFindPlayer = true;
            playerPos = player.transform;
            changePosX = playerPos.position.x;
            playerDistance = transform.position.x - changePosX;
        }
        GameObject setting = GameObject.Find("FirstObjectPooling");
        if (setting != null)
        {
            jellySetting = setting.GetComponent<JellySetting>();
            obstacleSetting = setting.GetComponent<ObstacleSetting>();
            tileSetting = setting.GetComponent<TileSetting>();
        }
    }

    private void LateUpdate()
    {
        //if (PlayerCharacter.instance != null && !InGameManager.instance.isGameover)
        //    transform.position = new Vector3(PlayerCharacter.instance.transform.position.x + posx, transform.position.y, transform.position.z);
        LenthCheck();
        ChangeX();
    }
    void ChangeX()
    {
        if (isFindPlayer)
        {
            if (playerPos.position.x < changePosX)
            {
                Debug.Log("playerPos.position.x : " + playerPos.position.x + ", chagnePosX : " + changePosX);
                float distance = changePosX - playerPos.position.x;
                changePosX = playerPos.position.x;
                if (jellySetting != null)
                    jellySetting.m_fReturnXPos -= distance;
                if (obstacleSetting != null)
                    obstacleSetting.m_fReturnXPos -= distance;
                if (tileSetting != null)
                {
                    tileSetting.m_fReturnXPos -= distance;
                    Debug.Log("return Posx : " + tileSetting.m_fReturnXPos + ", -= " + distance);
                }
                stayXPos = playerPos.position.x + playerDistance;
                transform.position = new Vector3(stayXPos, transform.position.y, transform.position.z);
            }
        }
    }

    void Init()
    {
        if (stayDis == 0)
        {
            RaycastHit2D hit = ReturnHit();
            isSet = false;
            if (hit)
                SetStayDis(hit);
        }
        else
            isSet = true;
    }

    void LenthCheck()
    {
        RaycastHit2D hit = ReturnHit();

        if (hit)
        {
            if (isSet)
            {
                //Debug.Log(hit.collider.gameObject.name);
                float currentYPos = transform.position.y; //현재 위치
                float distance = currentYPos - hit.transform.position.y; //거리차

                if (distance != stayDis)
                {
                    float dist = stayDis - distance; //현재 차이 - 유지거리차이
                    float absDist = Mathf.Abs(dist);
                    if (absDist < beforeDis  && absDist > CompulsionDistance)
                    {
                        if (dist > 0)
                        {
                            dist = 1 * yPosSpeed * Time.deltaTime;
                        }
                        else if (dist < 0)
                        {
                            dist = -1 * yPosSpeed * Time.deltaTime;
                        }
                        Vector3 Temp = new Vector3(stayXPos, dist + currentYPos, stayZPos); //조정 위치
                        transform.Translate(new Vector3(0, dist, 0));
                        TossYPos(dist);
                        return;
                    }
                    else
                    {
                        transform.position = new Vector3(stayXPos, transform.position.y + dist, stayZPos);
                        TossYPos(dist);
                        return;
                    }
                    //transform.position = Temp; //위치 조정
                }
                TossYPos(0);
                return;
            }
            else
            {
                TossYPos(0);
                SetStayDis(hit);
            }
        }
        //else
        //{
        //    transform.Translate(new Vector3(0, -1 * yPosSpeed * Time.deltaTime , 0));
        //}
    }
    void TossYPos(float dist)
    {
        for (int i = 0; i < backGround.Length; i++)
        {
            if (backGround[i] != null)
                backGround[i].GetMoveYPos(dist);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector3.down * MaxDistance) + transform.position);
    }

    RaycastHit2D ReturnHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, MaxDistance, layerNum);
        return hit;
    }

    void SetStayDis(RaycastHit2D hit)
    {
        stayDis = transform.position.y - hit.transform.position.y;
        isSet = true;
        beforeDis = Mathf.Abs(stayDis);
    }

}
