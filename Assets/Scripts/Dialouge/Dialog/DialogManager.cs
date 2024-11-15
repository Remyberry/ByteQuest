using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;

    public void ShowDialog(Dialog dialog)
    {
        dialogBox.SetActive(true);
        dialogText.text = dialog.Lines[0];
    }
}
