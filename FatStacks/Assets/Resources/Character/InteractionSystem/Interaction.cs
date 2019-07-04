using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public int current_prompt = 0;
    public PromptData _PromptData;
    public bool isBusy = false;
    public virtual void interact(Pickup pickup)
    {
        return;
    }
    public virtual string getPrompt(int index = -1)
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
    public virtual string get_exception(int index)
    {
        return _PromptData.exceptions[index];
    }
}
