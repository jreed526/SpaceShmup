using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Inscribed")]
    public float speed = 10f; //The movement speed is 10m/s
    public float fireRate = 0.3f; //Seconds/shot (Unused)
    public float health = 10; //Damage needed to destroy this enemt
    public int score = 100; //Points earned for destroying this
    public float powerUpDropChance = 1f; //Chance to drop a PowerUp

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;
    public AudioClip explosionSound; // NEW
    private AudioSource audioSource; // NEW


    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
        // Initialize the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>(); // NEW
        audioSource.volume = 0.2f; //NEW
    }

    //This is a Property: A methos that acts like a field
    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    void Update() {
        Move();

       //Check whether this Enemy has gone off the bottom of the screen
        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        
        //Check for collisions with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null) {
            //Only damage this Enemy if it's on screen
            if(bndCheck.isOnScreen) {
                //Get the damage amount from the Main WEAP_DICT.
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if(health <= 0) {
                    // Play explosion sound before destroying the enemy
                    if (explosionSound != null) {
                    PlayExplosionSound(); // Play sound here
                    }
                    //Tell Main that thus ship was destroyed
                    if (!calledShipDestroyed) {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    // Immediately destroy this Enemy
                    Destroy(this.gameObject);
                }
            }
            //Destroy the ProjectileHero regardless
            Destroy(otherGO); 
        } else {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
    // Method to play the explosion sound using a temporary object
    private void PlayExplosionSound() {
        GameObject tempGO = new GameObject("TempAudio");
        AudioSource tempAudioSource = tempGO.AddComponent<AudioSource>();
        tempAudioSource.clip = explosionSound;
        tempAudioSource.volume = 0.2f; // Set volume to half
        tempAudioSource.Play();
        Destroy(tempGO, explosionSound.length); // Destroy after the sound plays
    }
}
