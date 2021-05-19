using UnityEngine;

public class ScreenShotCupture : MonoBehaviour
{
    private static int _counter = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Capture();
    }

    private void Capture()
    {
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/Sreenshot" + _counter.ToString("00") + ".png");
        _counter++;
    }
}