using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    MessageManager messageManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;
    [SerializeField] GameObject woodBreakEffect;
    [SerializeField] GameObject buttonUI;

    private bool button; //PORCATA PIU' ATOMICA MAI VISTA D'ORA!!! PEGGIO DI ASSEMBLY (ho usato le region per nascondere l'orrore)

    // Start is called before the first frame update
    void Start()
    {
        button = false;
        messageManager = MessageManager.instance;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return null; //per evitare un bug
        // START //
        InputSystem.playerInputEnabled = false;
        buttonUI.SetActive(false);

        yield return new WaitForSeconds(2);

        messageManager.ActiveMessage("Benvenuto nel tutorial!");
        #region input bottone
        buttonUI.SetActive(true);
        while (!button) yield return null;
        button = false;
        buttonUI.SetActive(false);
        #endregion

        // PRIMO MOVIMENTO //

        InputSystem.playerInputEnabled = true;
        InputSystem.dashEnabled = false;
        InputSystem.inflateEnabled = false;

        messageManager.ActiveMessage("Clicca sullo schermo con il tasto sinistro del mouse per muoverti verso un punto.");

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // MOVIMENTO //
        yield return new WaitForSeconds(1);
        messageManager.ActiveMessage("Ottimo! Ora segui il percorso e prendi il gamberetto.");

        while (FoodManager.foodTaken < 1)
        {
            yield return null;
        }

        // SCATTO //
        yield return new WaitForSeconds(1);

        InputSystem.dashEnabled = true;
        InputSystem.playerMovementEnabled = false;
        Destroy(point1);
        Instantiate(woodBreakEffect, point1.transform.position, Quaternion.Euler(new Vector3(90,0,0)));

        messageManager.ActiveMessage("Bravo! Clicca il tasto destro del mouse per fare uno scatto e segui il percorso utilizzando solo questo tipo di movimento.");

        while (FoodManager.foodTaken < 2)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        InputSystem.playerInputEnabled = false;
        Destroy(point2);
        Instantiate(woodBreakEffect, point2.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
        messageManager.ActiveMessage("Puoi usare lo scatto per schivare i nemici e muoverti più velocemente!");
        #region input bottone
        buttonUI.SetActive(true);
        while(!button) yield return null;
        button = false;
        buttonUI.SetActive(false);
        #endregion

        // GONFIAMENTO //
        InputSystem.playerInputEnabled = true;
        InputSystem.inflateEnabled = true;

        messageManager.ActiveMessage("Premi il pulsante spazio per gonfiarti e allontanare i nemici.");
        #region input bottone
        buttonUI.SetActive(true);
        while (!button) yield return null;
        button = false;
        #endregion
        messageManager.ActiveMessage("Se rimani gonfiato i nemici perderanno interesse e smetteranno di inseguirti.");
        #region input bottone
        buttonUI.SetActive(true);
        while (!button) yield return null;
        button = false;
        buttonUI.SetActive(false);
        #endregion

        InputSystem.playerMovementEnabled = true;
        InputSystem.dashEnabled = true;

        messageManager.ActiveMessage("Ora prova ad allontanare il pesce nemico mentre si avvicina. Premi spazio per gonfiarti!");

        while (!enemyManager.isPushedBack)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        messageManager.ActiveMessage("Bravo! Ancora una volta! Premi spazio per gonfiarti.");

        while (!enemyManager.isPushedBack)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        messageManager.ActiveMessage("Complimenti, Hai superato il Tutorial! Prosegui verso destra per iniziare il gioco.");
        #region input bottone
        buttonUI.SetActive(true);
        while (!button) yield return null;

        messageManager.DeActiveMessage(); //per evitare un bug

        button = false;
        buttonUI.SetActive(false);
        #endregion

        CurrentData.day = 0;

        Destroy(point3);
        Instantiate(woodBreakEffect, point3.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
    }

    public void ButtonPressed()
    {
        button = true;
    }
}
