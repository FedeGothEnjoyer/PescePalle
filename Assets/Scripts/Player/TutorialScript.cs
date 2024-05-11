using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    MessageManager messageManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;

    // Start is called before the first frame update
    void Start()
    {
        messageManager = MessageManager.instance;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        // START //
        InputSystem.playerInputEnabled = false;
        yield return new WaitForSeconds(2);
        messageManager.ActiveMessage("Benvenuto nel tutorial!");

        // PRIMO MOVIMENTO //
        yield return new WaitForSeconds(5);

        InputSystem.playerInputEnabled = true;
        InputSystem.dashEnabled = false;
        InputSystem.inflateEnabled = false;

        messageManager.ActiveMessage("Clicca sullo schermo col tasto sinistro del muose per muoverti verso un punto.");

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

        messageManager.ActiveMessage("Bravo! Clicca il tasto destro del mouse per fare uno scatto e segui il percorso utilizzando solo questo tipo di movimento.");

        while (FoodManager.foodTaken < 2)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        InputSystem.playerInputEnabled = false;
        Destroy(point2);

        messageManager.ActiveMessage("Non dimenticarti che lo scatto lo puoi fare anche mentre sei in movimento.");
        yield return new WaitForSeconds(6);
        messageManager.ActiveMessage("Inoltre ti rende invincibile dai pericoli per tutto il periodo dello scatto.");
        yield return new WaitForSeconds(6);

        // GONFIAMENTO //
        InputSystem.playerInputEnabled = true;
        InputSystem.inflateEnabled = true;

        messageManager.ActiveMessage("Cliccando spazio il pesce è in grado di gonfiarsi, questo fa allontanare i pesci non amichevoli.");
        yield return new WaitForSeconds(6);

        InputSystem.playerMovementEnabled = true;
        InputSystem.dashEnabled = true;

        messageManager.ActiveMessage("Ora prova ad allontanare il pesce nemico nel momento in cui ti colpisce. Clicca spazio per gonfiarti");

        while (!enemyManager.isPushedBack)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
        messageManager.ActiveMessage("Bravo! Provaci un'altra volta! Clicca spazio per gonfiarti.");

        while (!enemyManager.isPushedBack)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        messageManager.ActiveMessage("Complimenti! Hai superato il Tutorial! Prosegui per iniziare il gioco.");

        Destroy(point3);
    }
}
