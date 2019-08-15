using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextController : MonoBehaviour
{
    public Tutorial activeTutorial;
    private Fade textFader;
    public Stack<Tutorial> tutorialStack = new Stack<Tutorial>();
    public bool isBusy;
    Coroutine coroutine;

    private void Start()
    {
        textFader = GetComponent<Fade>();
    }
    private void Update()
    {
        if(activeTutorial != null && activeTutorial.done == true)
        {
            PopTutorial();
        }
    }
    public void pushTutorial(Tutorial tutorial)
    {
        tutorialStack.Push(tutorial);
        coroutine = StartCoroutine(SwitchTutorial(tutorial));
    }
    public void PopTutorial()
    {
        tutorialStack.Pop();
        Tutorial tutorial = null;
        if (tutorialStack.Count > 0)
        {
            tutorial = tutorialStack.Peek();
        }
        coroutine = StartCoroutine(SwitchTutorial(tutorial));
    }
    IEnumerator SwitchTutorial(Tutorial newTutorial)
    {
        if (activeTutorial != null)
        {
            if (activeTutorial.done == true)
            {
                Destroy(activeTutorial.gameObject);
            }
            else
            {
                activeTutorial.gameObject.SetActive(false);
            }
        }
        textFader.fadeOutText();
        yield return new WaitUntil(IsTextFadedOut);
        if(newTutorial != null)
        {
            textFader.fadeInText(newTutorial.tutorialHeader);
            activeTutorial = newTutorial;
            activeTutorial.ready = true;
            activeTutorial.gameObject.SetActive(true);
        }
        StopCoroutine(coroutine);
        yield return null;
    }

    bool IsTextFadedOut()
    {
        return textFader.state == Fade.prompt_state.faded_out;
    }
}
