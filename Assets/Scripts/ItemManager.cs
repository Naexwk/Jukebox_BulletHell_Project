using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void gameItem();
public class ItemManager : MonoBehaviour
{  
    gameItem[] itemInventory = new gameItem[10];
    gameItem item1;
    private int obtainedItemsNumber = -1;
    void Start () {
        //addItem(2);
        //applyItems();
    }

    void applyItems () {
        /*foreach (gameItem item in itemInventory) {
            item();
        }*/
        for (int i = 0; i <= obtainedItemsNumber; i++) {
            itemInventory[i]();
        }
    }

    public void addItem(int itemID) {
        obtainedItemsNumber++;
        if (itemID == 1) {
            itemInventory[obtainedItemsNumber] = new gameItem(SausageHeart);
            return;
        }

        if (itemID == 2) {
            itemInventory[obtainedItemsNumber] = new gameItem(Fosfofosfo);
            return;
        }

        if (itemID == 3) {
            itemInventory[obtainedItemsNumber] = new gameItem(Cheese);
            return;
        }
        
        
    }

    void DoNothing(){
    }

    // Funciones de items
    void SausageHeart () {
        gameObject.GetComponent<PlayerController>().maxHealth += 2;
        gameObject.GetComponent<PlayerController>().currentHealth += 2;
    }

    void Fosfofosfo () {
        gameObject.GetComponent<PlayerController>().playerSpeed += 1;
    }

    void Cheese () {
        gameObject.GetComponent<PlayerController>().bulletDamage += 1;
    }
}
