using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseController : MonoBehaviour {
    private static bool paused = false;
    public static bool Paused
    {
        get { return paused; }
    }

    private static bool sfxOn = true;
    public static bool SFXOn
    {
        get { return sfxOn; }
    }

    public GameObject pausePanel;
    public CloudLauncher cloudLauncher;
    public AudioSource music;
    public Slider lookSpeedSlider;
    public Toggle invert;
    public Toggle musicToggle;
    public Toggle sfx;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;

    }

    public void ToggleInvert()
    {
        bool on = invert.isOn;
        cloudLauncher.invertLook = on;
    }

    public void UpdateLookSpeed()
    {
        cloudLauncher.lookSpeed = lookSpeedSlider.value;
    }

    public void SetMusicOn()
    {
        bool on = musicToggle.isOn;
        if (on)
        {
            music.Play();
        }
        else
        {
            music.Stop();
        }
    }

    public void SetSfxOn()
    {
        bool on = sfx.isOn;
        sfxOn = on;
    }
}