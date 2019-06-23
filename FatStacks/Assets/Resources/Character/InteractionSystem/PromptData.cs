using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New prompts", menuName = "Prompt and exception info")]
public class PromptData : ScriptableObject {

    public string[] prompts = { "No prompts set" };
    public string[] exceptions = { "No exceptions set" };
}
