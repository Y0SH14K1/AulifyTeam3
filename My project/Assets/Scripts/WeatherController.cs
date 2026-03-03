using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherController : MonoBehaviour
{
    [Header("References")]
    public Light sun;                    // Directional Light (Sun)
    public Slider timeSlider;            // 0-24 hours
    public Slider fogSlider;             // 0-1 fog density
    public TextMeshProUGUI timeLabel;               // optional label to show hour

    [Header("Settings")]
    public AnimationCurve sunIntensityCurve; // sun intensity over day
    public Color dayColor = Color.white;
    public Color morningColor = new Color(1f, 0.5f, 0f); // orange
    public Color nightColor = new Color(0.1f, 0.2f, 0.5f);

    [Range(0f, 24f)]
    public float timeOfDay = 12f;

    void Start()
    {
        // Link sliders to functions
        if (timeSlider != null) timeSlider.onValueChanged.AddListener(SetTime);
        if (fogSlider != null) fogSlider.onValueChanged.AddListener(SetFog);

        // Enable standard fog
        RenderSettings.fog = true;
    }

    void Update()
    {
        UpdateSun();
    }

    void UpdateSun()
    {
        // Rotate sun based on time of day
        float angle = (timeOfDay / 24f) * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        // Adjust intensity
        if (sunIntensityCurve != null)
            sun.intensity = sunIntensityCurve.Evaluate(timeOfDay / 24f);

        // Adjust color
        if (timeOfDay < 6f) sun.color = nightColor;
        else if (timeOfDay < 9f) sun.color = Color.Lerp(morningColor, dayColor, (timeOfDay - 6f) / 3f);
        else if (timeOfDay < 18f) sun.color = dayColor;
        else if (timeOfDay < 21f) sun.color = Color.Lerp(dayColor, morningColor, (timeOfDay - 18f) / 3f);
        else sun.color = nightColor;

        // Update time label
        if (timeLabel != null)
            timeLabel.text = "Hour: " + Mathf.RoundToInt(timeOfDay).ToString();
    }

    // Slider callbacks
    public void SetTime(float value)
    {
        timeOfDay = Mathf.Clamp(value, 0f, 24f);
    }

    public void SetFog(float value)
    {
        // Control the simple, built-in fog density
        RenderSettings.fogDensity = Mathf.Clamp01(value) * 0.05f;
    }
}