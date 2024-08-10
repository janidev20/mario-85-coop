using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoresManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject yourScoresTitle;
    [SerializeField] GameObject brokenObstacles;
    [SerializeField] GameObject QMBlock;
    [SerializeField] GameObject brickBlock;
    [SerializeField] GameObject stoneBlock;
    [SerializeField] GameObject emptyBlock;
    [SerializeField] GameObject pipe;
    [SerializeField] GameObject timeSpent;
    [SerializeField] GameObject lucasStatusTitle;
    [SerializeField] GameObject fellInVoidTitle;

    [Header("Values")]
    [SerializeField] TextMeshProUGUI qmBlockAmount;
    [SerializeField] TextMeshProUGUI brickBlockAmount;
    [SerializeField] TextMeshProUGUI stoneBlockAmount;
    [SerializeField] TextMeshProUGUI emptyBlockAmount;
    [SerializeField] TextMeshProUGUI pipeAmount;
    [SerializeField] TextMeshProUGUI timeAmount;
    [SerializeField] TextMeshProUGUI lucasDead;
    [SerializeField] TextMeshProUGUI lucasEscaped;
    [SerializeField] TextMeshProUGUI fellInVoidAmount;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    IEnumerator Scores()
    {
        yourScoresTitle.SetActive(true);

        yield return new WaitForSeconds(3f);

        brokenObstacles.SetActive(true);

        yield return new WaitForSeconds(2f);

        QMBlock.SetActive(true);

        yield return new WaitForSeconds(1f);

        brickBlock.SetActive(true);

        yield return new WaitForSeconds(1f);


        stoneBlock.SetActive(true);

        yield return new WaitForSeconds(1f);


        emptyBlock.SetActive(true);

        yield return new WaitForSeconds(1f);

        pipe.SetActive(true);

        yield return new WaitForSeconds(2f);

        qmBlockAmount.gameObject.SetActive(true);
        brickBlockAmount.gameObject.SetActive(true);
        stoneBlockAmount.gameObject.SetActive(true);
        emptyBlockAmount.gameObject.SetActive(true);
        pipe.gameObject.SetActive(true);

        StartCoroutine(countDestroyedObstacles());

        yield return new WaitForSeconds(6f);

        timeSpent.SetActive(true);

        yield return new WaitForSeconds(2f);

        timeAmount.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        lucasStatusTitle.SetActive(true);

        yield return new WaitForSeconds(2f);

        lucasDead.gameObject.SetActive(true); // PLACEHOLDER

        yield return new WaitForSeconds(2f);

        fellInVoidTitle.SetActive(true);

        yield return new WaitForSeconds(2f);

        fellInVoidAmount.gameObject.SetActive(true); // PLACEHOLDER
    }

    // FINISH THIS
    IEnumerator countDestroyedObstacles()
    {
        yield return new WaitForEndOfFrame(); 
    }
}
