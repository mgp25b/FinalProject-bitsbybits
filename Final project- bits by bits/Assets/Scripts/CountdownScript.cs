using UnityEngine;
using TMPro;

public class CountdownScript : MonoBehaviour
{
    public float countdownDuration = 3f;
    public float goDisplayDuration = 1f;
    public TextMeshProUGUI countdownText;

    private float currentTime;
    private float goDisplayTime;
    private bool isCountingDown;
    private bool isShowingGo;

    void Start()
    {
        currentTime = countdownDuration;
        isCountingDown = true;
        Time.timeScale = 0f;

        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }

    void Update()
    {
        if (isShowingGo)
        {
            goDisplayTime -= Time.unscaledDeltaTime;

            if (goDisplayTime <= 0f)
            {
                if (countdownText != null)
                {
                    countdownText.text = "";
                }

                isShowingGo = false;
                enabled = false;
            }

            return;
        }

        if (!isCountingDown)
        {
            return;
        }

        currentTime -= Time.unscaledDeltaTime;
        int displayValue = Mathf.CeilToInt(Mathf.Max(currentTime, 0f));

        if (countdownText != null)
        {
            countdownText.text = displayValue > 0 ? displayValue.ToString() : "GO!";
        }

        if (currentTime <= 0f)
        {
            isCountingDown = false;
            isShowingGo = true;
            goDisplayTime = goDisplayDuration;
            Time.timeScale = 1f;

            if (countdownText != null)
            {
                countdownText.text = "GO!";
            }
        }
    }
}
