using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    PlayerData playerData;

    //현재 플레이어 정보
    float hp;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp < 0f) hp = 0f;
            else if (hp > maxHP) hp = maxHP;
        }
    }

    float maxHP;

    Color curColor = Color.black;
    [SerializeField] Renderer[] renderers;
    [SerializeField] Material[] materials;

    //피격
    float hitDelay;
    float hitBlinkTime = 0f;
    Color blinkColor = new Color(0.5f, 0.5f, 0, 1);
    //피격UI
    [SerializeField] Slider mainHPSlider;
    [SerializeField] Slider subHPSlider;
    [SerializeField] Image subHPFillImage;
    //HP슬라이더 이전값
    float prevSliderValue;
    float subUIBlinkDelay;
    float subUIBlinkTime;
    Color subUICurColor = Color.white;
    Color subUIBlinkColor = new Color(1, 1, 0.4f, 1);

    //체력경고
    float warningBlinkTime = 0f;
    Color warningColor = new Color(1, 0, 0, 1);

    void Update()
    {
        //무적시간
        if (hitDelay > 0f)
        {
            hitDelay -= Time.deltaTime;
            if (hitDelay < 0f)
            {
                hitDelay = 0f;
                subHPSlider.value = hp / maxHP;
                //emission으로 발광효과주기
                //foreach (var renderer in renderers)
                curColor = Color.black;
                foreach(var material in materials)
                    material.SetColor("_EmissionColor", Color.black);
            }
            else
            {
                //발광효과주기
                hitBlinkTime += Time.deltaTime;
                if (hitBlinkTime > playerData.hitBlinkDelay)
                {
                    hitBlinkTime = 0f;
                    if (curColor == Color.black)
                    {
                        curColor = blinkColor;
                        foreach (var material in materials)
                            material.SetColor("_EmissionColor", blinkColor);

                    }
                    else
                    {
                        curColor = Color.black;
                        foreach (var material in materials)
                            material.SetColor("_EmissionColor", Color.black);
                    }
                }

                //UI
                if (hitDelay < subUIBlinkDelay)
                {
                    subHPFillImage.color = subUIBlinkColor;
                    //게이지 감소
                    subHPSlider.value = Mathf.Lerp(prevSliderValue, hp / maxHP, 1 - (hitDelay / subUIBlinkDelay));
                }
                else
                {
                    subUIBlinkTime += Time.deltaTime;
                    if (subUIBlinkTime > playerData.hitBlinkDelay)
                    {
                        subUIBlinkTime = 0f;
                        if (subUICurColor == Color.white)
                        {
                            subUICurColor = subUIBlinkColor;
                            subHPFillImage.color = subUICurColor;
                        }
                        else
                        {
                            subUICurColor = Color.white;
                            subHPFillImage.color = subUICurColor;
                        }
                    }
                }
            }
        }
        //체력 경고
        else if ((float)hp / maxHP <= playerData.healthWarningRatio)
        {
            //발광효과주기
            warningBlinkTime += Time.deltaTime;
            if (curColor == Color.black)
            {
                Color newColor = new Color(warningBlinkTime/ playerData.healthWarningBlinkDelay, 0, 0);
                foreach (var material in materials)
                    material.SetColor("_EmissionColor", newColor);
            }
            else
            {
                Color newColor = new Color(1-warningBlinkTime / playerData.healthWarningBlinkDelay, 0, 0);
                foreach (var material in materials)
                    material.SetColor("_EmissionColor", newColor);
            }

            if (warningBlinkTime > playerData.healthWarningBlinkDelay)
            {
                warningBlinkTime = 0f;
                if (curColor == Color.black)
                    curColor = warningColor;
                else
                    curColor = Color.black;
            }
        }
    }

    public void Set(PlayerData playerData)
    {
        this.playerData = playerData;
        maxHP = playerData.health;
        hp = maxHP;
    }

    public void Hit(Vector3 hitDir, float damage, bool drop)
    {
        if (hitDelay > 0f) return; //무적상태
        hitDelay = playerData.hitDelay; //무적시간 설정
        subUIBlinkDelay = playerData.hitDelay * 0.5f;
        subUIBlinkTime = 0;
        prevSliderValue = mainHPSlider.value;
        HP -= damage;
        mainHPSlider.value = hp / maxHP;
        if (hp <= 0f)
            Die();
        else
        {
            PlayerManager.Instance.Hit();
            //피격 애니메이션 실행
            PlayerManager.Instance.PMovement.Hit(hitDir);

            //피격UI?

            //아이템 드롭
            if (drop)
            {
                PlayerManager.Instance.UnChange(hitDir, true);
            }
        }
    }

    void Die()
    {
        Time.timeScale = 0f;
        //게임오버 연출
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
