using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ataquescooldown : MonoBehaviour
{
    [Header("habilidade 3")] 
    public Image HabilidadeImage3;
    public float cooldown3 = 10;
    private bool isCooldown = false;
    public KeyCode habilidade3 = KeyCode.L;
    
    [Header("habilidade 3")] 
    public Image HabilidadeImage2;
    public float cooldown2 = 1;
    private bool isCooldown2 = false;
    public KeyCode habilidade2 = KeyCode.K;
    // Start is called before the first frame update
    void Start()
    {
        HabilidadeImage3.fillAmount = 0;
        HabilidadeImage2.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        habilidad3();
        habilidad2();
    }

    void habilidad3()
    {
        if (Input.GetKey(habilidade3) && isCooldown == false)
        {
            isCooldown = true;
            HabilidadeImage3.fillAmount = 1;
        }

        if (isCooldown)
        {
            HabilidadeImage3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (HabilidadeImage3.fillAmount <= 0)
            {
                HabilidadeImage3.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
    
    void habilidad2()
    {
        if (Input.GetKey(habilidade2) && isCooldown2 == false)
        {
            isCooldown2 = true;
            HabilidadeImage2.fillAmount = 1;
        }

        if (isCooldown2)
        {
            HabilidadeImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (HabilidadeImage2.fillAmount <= 0)
            {
                HabilidadeImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }
}
