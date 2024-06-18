using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private GameObject correctPanel, incorrectPanel, finishPanel;

    [SerializeField] private Question[] questions;
    private int questionIndex = 0, currentIndex;

    [SerializeField] private TMPro.TextMeshProUGUI questionTxt;
    [SerializeField] private Button[] answersButtons;
    [SerializeField] private TMPro.TextMeshProUGUI[] answerTxts;

    [SerializeField] private TMPro.TextMeshProUGUI incorrectCountTxt;
    [SerializeField] private TMPro.TextMeshProUGUI maxIncorrectCountTxt;
    private int incorrectCount;

    private void Awake() {
        correctPanel.SetActive(false);
        incorrectPanel.SetActive(false);
        finishPanel.SetActive(false);

        RandomizeQuestions();
        SetQuestions();
        SetIncorrectCount(0);
    }

    public void AnswerButton(int index) {
        //If the answer is correct :
        if (index == currentIndex) {
            //If all questions are finished :
            if (questionIndex == questions.Length - 1) {
                SavedHighestIncorrectCount();
                finishPanel.SetActive(true);
            }

            //If all questions aren`t finished :
            else {
                correctPanel.SetActive(true);
            }
        }

        //If the answer is incorrect
        else {
            incorrectPanel.SetActive(true);
            SetIncorrectCount(incorrectCount + 1);
        }

        //Making buttons disable :
        DisableButton();
    }

    public void PanelButton(bool isTrue) {
        if(isTrue) {
            if(questionIndex == questions.Length - 1) {
                finishPanel.SetActive(false);
                questionIndex = 0;
                SetIncorrectCount(0);
                RandomizeQuestions();
            }

            else {
                correctPanel.SetActive(false);
                questionIndex++;
            }

            SetQuestions();
        }

        else {
            incorrectPanel.SetActive(false);
        }

        //Making buttons enable :
        EnableButton();
    }

    private void RandomizeQuestions() {
        //Randomize the questions with the fisher yates shuffle algorithm :   
        for (int i = questions.Length - 1; i >= 0; i--) { 
            int j = Random.Range(0, i + 1);

            Question temp = questions[i];
            questions[i] = questions[j];
            questions[j] = temp;
        }
    }

    private void SetQuestions() {
        Question question = questions[questionIndex];

        questionTxt.text = question.question;

        //Randomize the answers for each question : 
        string[] randomAnswer = new string[question.answers.Length];
        int counter = 0, index = 0;

        while (counter != question.answers.Length) {
            index = Random.Range(0, question.answers.Length);

            if (randomAnswer[index] == null) {
                randomAnswer[index] = question.answers[counter];

                if(counter == question.currentIndex) 
                    currentIndex = index;

                counter++;
            }
        }

        //Print questions on all text ui : 
        for (int i = 0; i < answerTxts.Length; i++) {
            answerTxts[i].text = randomAnswer[i];
        }
    }

    private void SetIncorrectCount(int value) {
        incorrectCount = value;
        incorrectCountTxt.text = $"Incorrect Count : {incorrectCount}";
    }

    private void SavedHighestIncorrectCount() {
        //The highest incorrect answer count is saved :
        if (incorrectCount > PlayerPrefs.GetInt("maxIncorrectCount", 0))
            PlayerPrefs.SetInt("maxIncorrectCount", incorrectCount);

        maxIncorrectCountTxt.text = $"Max Incorrect Count : {PlayerPrefs.GetInt("maxIncorrectCount", 0)}";
    }

    private void DisableButton() {
        for (int i = 0; i < answersButtons.Length; i++) { 
            answersButtons[i].interactable = false;
        }
    }

    private void EnableButton() {
        for (int i = 0; i < answersButtons.Length; i++) {
            answersButtons[i].interactable = true;
        }
    }
}
