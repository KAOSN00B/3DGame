using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{

    [SerializeField] private Image dialogueImage;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private float score = 1;
    

    void Start()
    {
        //    dialogueImage.sprite = null;
        //dialogueImage.color = Color.blue;
        resumeButton.onClick.AddListener(() =>
        {
            TestButtonPress();
            Debug.Log("Event works");
        });
            

    }

    public void TestButtonPress()
    {
        score++;
        dialogueText.text = $"Score: {score}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
