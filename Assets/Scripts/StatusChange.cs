using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class StatusChange : MonoBehaviour
{
    [SerializeField] PlayerAnimation AnimationScript;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (AnimationScript.isMX)
        {

            StartCoroutine(MXText());
            StopCoroutine(DisguisedText());
            StopCoroutine(CorruptedDisguiseText());

        } else if (AnimationScript.isPCrawler)
        {
            StartCoroutine(CorruptedDisguiseText());
            StopCoroutine(DisguisedText());
            StopCoroutine(MXText());
        } else if (AnimationScript.isFH)
        {
            StartCoroutine(DisguisedText());
            StopCoroutine(CorruptedDisguiseText());
            StopCoroutine(MXText());
        }
    }

    IEnumerator CorruptedDisguiseText()
    {
        while (AnimationScript.isPCrawler)
        {
            text.color = Color.red;
            

                yield return new WaitForSeconds(0.5f);

                text.text = "S̸T̶A̶T̸U̵S̵ : DISGUISE CORR̴U̴P̷T̵E̸D̶";

                yield return new WaitForSeconds(0.50f);

                text.text = "S̸T̶A̶T̸U̵S̵ : DISGUISE CORR̸̬̔U̵͖̇P̴̣͝T̸̤̚E̶͔̿D̵̖͐";

                yield return new WaitForSeconds(0.25f);


                // REPEAT

                text.text = "S̸T̶A̶T̸U̵S̵ : DISGUISE CORR̴U̴P̷T̵E̸D̶";

                yield return new WaitForSeconds(0.50f);

                text.text = "S̸T̶A̶T̸U̵S̵ : DISGUISE CORR̸̬̔U̵͖̇P̴̣͝T̸̤̚E̶͔̿D̵̖͐";

                yield return new WaitForSeconds(0.25f);


                text.text = "";

                yield return new WaitForSeconds(1.0f);
            }
    }
    
    IEnumerator DisguisedText()
    {
        while (AnimationScript.isFH)
        {
            text.color = Color.white;

                yield return new WaitForSeconds(0.5f);


                text.text = "STATUS̶̟̻̩̃̃͜  : DISGUISED AS MARIO";

                yield return new WaitForSeconds(1.5f);

                text.text = "";

                yield return new WaitForSeconds(1.0f);
            }
    }
    
    IEnumerator MXText()
    {
        while (AnimationScript.isMX)
        {
                yield return new WaitForSeconds(0.5f);

                text.text = "Ș̴͂T̶̖͂A̶̬̕T̴̘͑U̴̥̽S̶̟̃ : GET LUCAS";

                yield return new WaitForSeconds(1.5f);

                text.text = "Ș̴͂T̶̖͂A̶̬̕T̴̘͑U̴̥̽S̶̟̃ : ";

                yield return new WaitForSeconds(1.0f);
            }
    }
}
