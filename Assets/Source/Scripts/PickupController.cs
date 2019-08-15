using System;
using UnityEngine;

public class PickupController : MonoBehaviour{
    [SerializeField] private PickupTypeEnum _type;
    [SerializeField] private int value;
    [SerializeField] private AudioClip _pickupSfx;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            PlayerController player = other.GetComponent<PlayerController>();
            HandlePickup(ref player);
            other.GetComponent<AudioSource>().PlayOneShot(_pickupSfx);
            Destroy(gameObject);
        }
    }

    private void HandlePickup(ref PlayerController player){
        switch (_type){
            case PickupTypeEnum.Health:
                player.GiveHealth(value);
                break;
            case PickupTypeEnum.Ammo:
                player.GiveAmmo(value);
                break;
            default:
                return;
        }
    }
}
