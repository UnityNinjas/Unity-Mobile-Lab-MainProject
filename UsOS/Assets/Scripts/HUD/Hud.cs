using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Hud : MonoBehaviour
{
    public static Hud instance;

    public Image chipCircuit;
    public RectTransform topVignete;
    public RectTransform bottomVignete;
    public MotionBlur motionBlur;
    public GameObject hideButton;

    public CanvasGroup mobileControls;

    public Scrollbar battery;
    public GameObject tryAgain;
    public InputField inputField;
    public GameObject finishScreen;

    public void Awake()
    {
        instance = this.GetComponent<Hud>();
    }

    public void Start()
    {
        UpdateBattery();
        UpdateHealth();
        this.motionBlur.blurAmount = 0;
        this.mobileControls.alpha = 1;
    }

    public void SwitchHideButton(bool show)
    {
        this.hideButton.SetActive(show);
    }

    public void UpdateHealth()
    {
        if (GameData.Health <= 100)
        {
            this.chipCircuit.gameObject.SetActive(true);
            this.topVignete.gameObject.SetActive(true);
            this.bottomVignete.gameObject.SetActive(true);
            this.chipCircuit.color = new Color(1f, 1f, 1f, 2f / (GameData.Health + 7f));
            this.motionBlur.blurAmount = 5f / (GameData.Health + 1f);
            this.mobileControls.alpha = GameData.Health / 100f;
            this.topVignete.anchoredPosition = new Vector2(0, -(GameData.Health / -100f * 384f));
            this.bottomVignete.anchoredPosition = new Vector2(0, GameData.Health / -100f * 384f);

        }
        else
        {
            this.topVignete.gameObject.SetActive(false);
            this.bottomVignete.gameObject.SetActive(false);
            this.chipCircuit.gameObject.SetActive(false);
        }
    }

    public void UpdateBattery()
    {
        this.battery.size = GameData.batteryPower;
    }

    public void ActivateTryAgainPanel()
    {
        this.tryAgain.SetActive(true);
        EventSystem.current.SetSelectedGameObject(this.inputField.gameObject, null);
    }

    public void OnEndEditField()
    {

        if (!string.IsNullOrEmpty(this.inputField.text))
        {
            string input = this.inputField.text.ToLower();
            switch (input)
            {
                case "y":
                case "yes":
                    SceneManager.UnloadScene(0);
                    SceneManager.LoadScene(0);
                    break;
                case "n":
                case "no":
                    Application.Quit();
                    break;
                default:
                    this.inputField.text = "Incorect input.";
                    break;
            }
        }
    }

    public void FinishScreenOpen()
    {
        this.finishScreen.SetActive(true);
        StartCoroutine(FinishTimerOn());
    }

    private IEnumerator FinishTimerOn()
    {
        yield return new WaitForSeconds(10f);
        Application.Quit();
    }
}
