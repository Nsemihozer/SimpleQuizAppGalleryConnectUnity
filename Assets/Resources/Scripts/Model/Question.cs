using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string category { get; set; }
    public string id { get; set; }
    public string correctAnswer { get; set; }
    public List<string> incorrectAnswers { get; set; }
    public string question { get; set; }
    public List<string> tags { get; set; }
    public string type { get; set; }
    public string difficulty { get; set; }
}
