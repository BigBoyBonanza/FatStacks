using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public int current_prompt = 0;
    public PromptData _PromptData;
    protected bool isBusy = false;

    public virtual void Interact(Pickup pickup)
    {
        return;
    }

    public virtual string GetPrompt(int index = -1)
    {
        if (index == -1)
        {
            return _PromptData.prompts[current_prompt];
        }
        else
        {
            return _PromptData.prompts[index];
        }
    }

    public virtual string GetException(int index)
    {
        return _PromptData.exceptions[index];
    }
    public virtual bool IsBusy()
    {
        return isBusy;
    }
}
