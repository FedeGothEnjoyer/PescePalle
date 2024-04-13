using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe del dialogo, in generale le caratterestiche del popup. Per settarlo bisogna
//farlo direttamente dal Trigger sull'inspector di Unity che hanno come variabile
//public un Instanza "Dialogo"
[System.Serializable]
public class Dialouge
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
}
