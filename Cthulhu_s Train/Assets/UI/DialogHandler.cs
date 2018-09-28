using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogHandler : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkAsset;

    [SerializeField]
    private Canvas choiceCanvas;

    [SerializeField]
    private GameObject choicePrefab;

    [SerializeField]
    private TextMeshProUGUI npcText;

    private Story story;
    private bool nextActionPossible = true;
    private bool waitingForAnswer;
    private bool dialogEnded;

    private int selectedOption;
    private bool canGoToNextChoice = true;

    [SerializeField]
    private TextMeshProUGUI nextText;

    [SerializeField]
    private Player player;

    public void StartDialog(string characterName)
    {
        SetDialogTitle(characterName);

        if (story == null)
            story = new Story(inkAsset.text);

        dialogEnded = false;
        nextActionPossible = true;
        waitingForAnswer = false;
        canGoToNextChoice = true;

        story.ChoosePathString(characterName + ".entry");

        nextText.text = "Next";

        npcText.alignment = TextAlignmentOptions.TopLeft;
        RemoveChoices();
        RefreshDialogView();

        player.InputHandler.RefreshView();
    }

    private void SetDialogTitle(string characterName)
    {
        var obj = GameObject.FindGameObjectWithTag("DialogTitle");
        if (obj != null)
        {
            var title = obj.GetComponent<TextMeshProUGUI>();
            if (title != null)
                title.text = characterName;
        }
    }

    private void RefreshDialogView()
    {
        if (story.canContinue)
        {
            var text = story.Continue().Trim();
            npcText.text = text;

            if (story.currentTags.Contains("end"))
            {
                dialogEnded = true;
                nextText.text = "End";
                nextText.transform.parent.gameObject.SetActive(true);
                //nextText.gameObject.SetActive(true);
                return;
            }

            if (story.currentChoices.Count > 0)
            {
                RemoveChoices();

                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    var choice = story.currentChoices[i];
                    CreateChoice("[" + (i + 1).ToString() + "] " + choice.text.Trim());
                }

                selectedOption = 0;
                UpdateChoices();

                waitingForAnswer = true;
                //nextText.gameObject.SetActive(false);
                //nextText.transform.parent.gameObject.SetActive(false);
                nextText.text = "Choose";
            }
            else
                //nextText.gameObject.SetActive(true);
                //nextText.transform.parent.gameObject.SetActive(true);
                nextText.text = "Next";

            nextActionPossible = false;
            StartCoroutine("DisableNextAction", 0.5f);
        }
    }

    private IEnumerator DisableNextAction(float seconds = 1.0f)
    {
        yield return new WaitForSeconds(seconds);
        nextActionPossible = true;
    }

    private IEnumerator SkipBetweenChoices()
    {
        yield return new WaitForSeconds(0.2f);
        canGoToNextChoice = true;
    }

    void Update()
    {
        if (nextActionPossible && !waitingForAnswer && player.InputHandler.DoAffirmativeAction)
        {
            if (!dialogEnded)
                RefreshDialogView();
            else
            {
                player.EndDialog();
                return;
            }
        }

        if (nextActionPossible && waitingForAnswer)
        {
            var madeChoice = false;

            if (Input.GetKey(KeyCode.Alpha1) && story.currentChoices.Count >= 1)
            {
                story.ChooseChoiceIndex(0);
                madeChoice = true;
            }
            else if (Input.GetKey(KeyCode.Alpha2) && story.currentChoices.Count >= 2)
            {
                story.ChooseChoiceIndex(1);
                madeChoice = true;
            }
            else if (Input.GetKey(KeyCode.Alpha3) && story.currentChoices.Count >= 3)
            {
                story.ChooseChoiceIndex(2);
                madeChoice = true;
            }
            else if (Input.GetKey(KeyCode.Alpha4) && story.currentChoices.Count >= 4)
            {
                story.ChooseChoiceIndex(3);
                madeChoice = true;
            }
            else if (Input.GetKey(KeyCode.Alpha5) && story.currentChoices.Count >= 5)
            {
                story.ChooseChoiceIndex(5);
                madeChoice = true;
            }
            else if (Input.GetKey(KeyCode.Alpha6) && story.currentChoices.Count >= 6)
            {
                story.ChooseChoiceIndex(6);
                madeChoice = true;
            }
            else if (player.InputHandler.DoAffirmativeAction && selectedOption >= 0 &&
                     selectedOption < story.currentChoices.Count)
            {
                story.ChooseChoiceIndex(selectedOption);
                madeChoice = true;
            }

            if (madeChoice)
            {
                waitingForAnswer = false;
                RemoveChoices();
                RefreshDialogView();

                nextActionPossible = false;
                StartCoroutine("DisableNextAction", 0.5f);
            }
        }

        if (canGoToNextChoice)
        {
            var madeSkip = false;


            if (Input.GetAxis("Vertical") > 0.0f)
            {
                selectedOption -= 1;
                madeSkip = true;
            }
            else if (Input.GetAxis("Vertical") < 0.0f)
            {
                selectedOption += 1;
                madeSkip = true;
            }

            if (madeSkip)
            {
                selectedOption = Mathf.Clamp(selectedOption, 0, choiceCanvas.transform.childCount - 1);
                UpdateChoices();

                canGoToNextChoice = false;
                StartCoroutine("SkipBetweenChoices");
            }
        }

        /*if (Input.GetAxis("Joystick D Pad") > 0.0f)
        {
            selectedOption -= 1;
            selectedOption = Mathf.Clamp(selectedOption, 0, choiceCanvas.transform.childCount - 1);
            UpdateChoices();
        }
        else if (Input.GetAxis("Joystick D Pad") < 0.0f)
        {
            selectedOption += 1;
            selectedOption = Mathf.Clamp(selectedOption, 0, choiceCanvas.transform.childCount - 1);
            UpdateChoices();
        }*/
    }

    private void RemoveChoices()
    {
        var childCount = choiceCanvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
            GameObject.Destroy(choiceCanvas.transform.GetChild(i).gameObject);
    }

    void CreateChoice(string text)
    {
        var choice = Instantiate(choicePrefab);
        var textComponent = choice.GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError("No text component found in choice prefab");
            return;
        }

        textComponent.text = text;
        choice.transform.SetParent(choiceCanvas.transform, false);
    }

    private void UpdateChoices()
    {
        for (int i = 0; i < choiceCanvas.transform.childCount; i++)
            choiceCanvas.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);

        if (selectedOption >= 0 && selectedOption < choiceCanvas.transform.childCount)
            choiceCanvas.transform.GetChild(selectedOption).GetChild(0).gameObject.SetActive(true);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 30, 100, 200), "selected: " + selectedOption.ToString());
        GUI.Label(new Rect(0, 20, 300, 100), Input.GetAxis("Vertical").ToString());
    }
}
