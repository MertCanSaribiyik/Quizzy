using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private GameObject correctPanel, incorrectPanel, finishPanel;

    [SerializeField] private Question[] questions;
    private int questionIndex = 0;

    [SerializeField] TMPro.TextMeshProUGUI questionTxt;
    [SerializeField] TMPro.TextMeshProUGUI[] answerTxts;

    [SerializeField] private TMPro.TextMeshProUGUI incorrectCountTxt;
    private int incorrectCount;

    private void Awake() {
        correctPanel.SetActive(false);
        incorrectPanel.SetActive(false);
        finishPanel.SetActive(false);

        SetQuestions();
        SetIncorrectCount(0);
    }

    public void AnswerButton(int index) {
        if (index == questions[questionIndex].currentIndex) {
            if(questionIndex == questions.Length - 1) {
                finishPanel.SetActive(true);
            }

            else {
                correctPanel.SetActive(true);
            }
        }

        else {
            incorrectPanel.SetActive(true);
        }
    }

    public void PanelButton(bool isTrue) {
        if(isTrue) {
            if(questionIndex == questions.Length - 1) {
                finishPanel.SetActive(false);
                questionIndex = 0;
                SetIncorrectCount(0);
            }

            else {
                correctPanel.SetActive(false);
                questionIndex++;
            }

            SetQuestions();
        }

        else {
            incorrectPanel.SetActive(false);
            SetIncorrectCount(incorrectCount + 1);
        }
    }

    private void SetQuestions() {
        questionTxt.text = questions[questionIndex].question;

        for (int i = 0; i < answerTxts.Length; i++) {
            answerTxts[i].text = questions[questionIndex].answers[i];
        }
    }

    private void SetIncorrectCount(int value) {
        incorrectCount = value;
        incorrectCountTxt.text = $"Incorrect Count : {incorrectCount}";
    }
}
