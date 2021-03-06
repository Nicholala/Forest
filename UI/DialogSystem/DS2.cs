using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DS2 : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;
    public float textSpeed;

    [Header("文本文件")]
    public TextAsset textFile;
    public int index;

    bool textFinished = true;
    List<string> textList = new List<string>();

    void Awake()
    {
        Link.S.CanMove = false;
        Link.S.CanJump = false;
        Link.S.CanDash = false;
        Link.S.Rig.velocity = Vector2.zero; ;
        GetTextFromFile(textFile);
        index = 0;
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
        //textLabel.text = textList[index];
        //index++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            Link.S.CanMove = true;
            Link.S.CanJump = true;
            Link.S.CanDash = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && textFinished)
        {
            //textLabel.text = textList[index];
            //index++;
            StartCoroutine(SetTextUI());
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var linedata = file.text.Split('\n');

        foreach (var line in linedata)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }
        textFinished = true;
        index++;
    }
}
