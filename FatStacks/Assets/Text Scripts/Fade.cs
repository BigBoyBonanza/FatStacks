using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public enum prompt_state
    {
        fading_in,
        fading_out,
        faded_in,
        faded_out
    }
    [HideInInspector]
    public prompt_state state;
    [HideInInspector]
    public Text uiText;
    private CanvasRenderer canvasRenderer;
    [HideInInspector]
    public float alpha;
    private float t = -1f;
    private void Start()
    {
        uiText = GetComponent<Text>();
        canvasRenderer = GetComponent<CanvasRenderer>();
        state = prompt_state.faded_out;
    }
    private void Update()
    {
        if (state == prompt_state.fading_in)
        {
            LerpAlpha(0f, 1f, prompt_state.faded_in);
        }
        else if (state == prompt_state.fading_out)
        {
            LerpAlpha(1f, 0f, prompt_state.faded_out);
        }
        if(t > 0)
        {
            t -= Time.deltaTime;
            if (t < 0f)
                state = prompt_state.fading_out;
        }
    }
    public void FadeInText(string message)
    {
        state = prompt_state.fading_in;
        uiText.text = message;
    }
    public void fade_out_text()
    {
        state = prompt_state.fading_out;
        t = -1f;
    }
    public void FlashText(string message, float time)
    {
        uiText.text = message;
        state = prompt_state.faded_in;
        t = time;
        alpha = 1f;
        canvasRenderer.SetAlpha(1f);
    }
    private void LerpAlpha(float start, float target, prompt_state stateIfFinished)
    {
        alpha += Mathf.Sign(target - start) * 0.1f;
        if (start < target)
        {
            alpha = Mathf.Clamp(alpha, start, target);
        }
        else
        {
            alpha = Mathf.Clamp(alpha, target, start);
        }

        canvasRenderer.SetAlpha(alpha);
        if (alpha == target)
        {
            state = stateIfFinished;
        }
    }
}
