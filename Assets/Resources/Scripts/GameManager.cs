using System;
using System.Collections;
using System.Collections.Generic;
using Huawei.Agconnect.Auth;
//using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Text questionText, A, B, C, D,Loading,ScoreText;
    [SerializeField] private GameObject answersGameObject,gameOverPanel;

    private int _questionIndex = 0;
    private string _category;
    private List<Question> _questions = new List<Question>();
    private int _score=0;

    void Start()
    {
        
        _category = PlayerPrefs.GetString("category");
        StartCoroutine(GetQuestions());
        _score=0;
    }

    

    IEnumerator GetQuestions()
    {
        UnityWebRequest www =
            UnityWebRequest.Get($"https://the-trivia-api.com/api/questions?categories={_category}&limit=5");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Loading.gameObject.SetActive(false);
            answersGameObject.SetActive(true);
            questionText.gameObject.SetActive(true);
            _questions = JsonConvert.DeserializeObject<List<Question>>(www.downloadHandler.text);
            SetQuestion();
        }
    }

    private static void Shuffle(List<string> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private async void SetQuestion()
    {
        if (_questionIndex==_questions.Count)
        {
            GameScore score = new GameScore
            {
                category = _category,
                score = _score,
                userid = AGConnectAuth.Instance.GetCurrentUser().GetUid()
            };

            await DBManager._instance.ExecuteUpsert(score);
            ScoreText.text += " " + _score;
            answersGameObject.SetActive(false);
            questionText.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            
        }
        else
        {
            questionText.text = _questions[_questionIndex].question;
            List<string> answers = new List<string>();
            answers.Add(_questions[_questionIndex].correctAnswer);
            answers.AddRange(_questions[_questionIndex].incorrectAnswers);
            Shuffle(answers);
            A.text = answers[0];
            B.text = answers[1];
            C.text = answers[2];
            D.text = answers[3];
        }
      
    }

    public void Answer(GameObject clicked)
    {
      
        if (clicked.transform.GetChild(0).GetComponent<Text>().text.Equals(_questions[_questionIndex].correctAnswer))
        {
            _score += 10;
            clicked.GetComponent<Image>().color=Color.green;
        }
        else
        {
            clicked.GetComponent<Image>().color=Color.red;
        }
        StartCoroutine(NextQuestion(clicked));
 
    }

    IEnumerator NextQuestion(GameObject clicked)
    {
        yield return new WaitForSeconds(0.5f);
        clicked.GetComponent<Image>().color=Color.white;
        _questionIndex++;
        SetQuestion();
    }
}