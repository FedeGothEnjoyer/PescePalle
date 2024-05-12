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
    [SerializeField] GameObject woodBreakEffect;

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
        yield return new WaitForSeconds(6);

        // GONFIAMENTO //
        InputSystem.playerInputEnabled = true;
        InputSystem.inflateEnabled = true;

        messageManager.ActiveMessage("Premi il pulsante spazio per gonfiarti e allontanare i nemici.");
        yield return new WaitForSeconds(6);
        messageManager.ActiveMessage("Se rimani gonfiato i nemici perderanno interesse e smetteranno di inseguirti.");
        yield return new WaitForSeconds(6);

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
        CurrentData.day = 0;

        Destroy(point3);
        Instantiate(woodBreakEffect, point3.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
    }

	private void OnDestroy()
	{
        messageManager.DeActiveMessage();
    }
}
