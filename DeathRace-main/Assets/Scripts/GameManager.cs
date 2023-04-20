using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameVariable buyTurret;
    public GameVariable addSlot;
    public GameVariable addHealth;

    public IntVariable gold;
    public TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        textMeshProUGUI.text = gold.count.ToString();
    }
}
