using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    public float displayTime;
    public float fadeTime;

    private IEnumerator fadeAlpha;

    private Image container;

    private static DisplayManager displayManager;

    public static DisplayManager Instance () {
        if (!displayManager) {
            displayManager = FindObjectOfType(typeof (DisplayManager)) as DisplayManager;

            if (!displayManager)
                Debug.LogError ("There needs to be one active DisplayManager script on a GameObject in your scene.");
        }

        return displayManager;
    }

    public void DisplayMessage (string message) {
      container = displayManager.gameObject.GetComponent<Image>();
        gameObject.transform.GetChild(0).GetComponent<Text>().text = message;
        SetAlpha ();
    }

    void SetAlpha () {
        if (fadeAlpha != null) {
            StopCoroutine (fadeAlpha);
        }
        fadeAlpha = FadeAlpha ();
        StartCoroutine (fadeAlpha);
    }

    IEnumerator FadeAlpha () {
        Color resetColor = container.color;
        resetColor.a = 1;
        container.color = resetColor;

        yield return new WaitForSeconds (displayTime);

        while (container.color.a > 0) {
            Color displayColor = container.color;
            displayColor.a -= Time.deltaTime / fadeTime;
            container.color = displayColor;
            yield return null;
        }
        yield return null;
    }
}
