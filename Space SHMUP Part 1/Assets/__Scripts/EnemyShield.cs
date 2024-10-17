using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BlinkColorOnHit))]

public class EnemyShield : MonoBehaviour {
    [Header("Inscribed")]
    public float health = 10;

    private List<EnemyShield> protectors = new List<EnemyShield>();
    private BlinkColorOnHit blinker;

    void Start() {
        blinker = GetComponent<BlinkColorOnHit>();
        blinker.ignoreOnCollisionEnter = true; //This will not yet compile

        if(transform.parent == null) return;
        EnemyShield shieldParent = transform.parent.GetComponent<EnemyShield>();
        if(shieldParent != null) {
            shieldParent.AddProtector(this);
        }
    }

    //Called by another EnemyShield to join the protectore of this EnemyShield
    public void AddProtector(EnemyShield shieldChild) {
        protectors.Add(shieldChild);
    }

    //Shortcut for gameObject.activeInHierarchy and gameObject.SetActive()
    public bool isActive {
        get {return gameObject.activeInHierarchy;}
        private set {gameObject.SetActive(value);}
    }

    //Called by Enemy_4.OnCollisionEnter() & parent's EnemyShields.TakeDamage() to distribute damage to EnemyShield protectors
    public float TakeDamage(float dmg) {
        //Can we pass damage to a protector EnemtShield
        foreach(EnemyShield es in protectors) {
            if(es.isActive) {
                dmg = es.TakeDamage(dmg);
                //If all damage was handled, return 0 damage
                if(dmg == 0 ) return 0;
            }
        }

        //EnemyShield will blink & take damage. Make the blinker blink
        blinker.SetColors(); //This will appear underlined in red for now
        health -= dmg;
        if(health <=0) {
            //Deactivate this EnemyShield GameObject
            isActive = false;
            //Return any damage that was not absorbed by this EnemyShield
            return -health;
        }

        return 0;
    }

}
