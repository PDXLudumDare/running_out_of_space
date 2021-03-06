﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarrokhGames.Inventory;


public class DesireSystem : MonoBehaviour {
    [SerializeField] Color hoverColor;
 	[SerializeField] Emote emoteBubbleDefault;
    [SerializeField] Emote emoteBubbleMad;
    [SerializeField] Emote emoteBubbleHappy;
	[SerializeField] Sprite happyIcon;
	[SerializeField] Sprite angryIcon;
	[SerializeField] float destroyEmoteTime = 3f;
    [SerializeField] Vector3 emoteOffset;

    [SerializeField] AudioClip happySFX;
    [SerializeField] AudioClip madSFX;
 
	public IInventoryItem currentDesire;

	Emote currentEmote;
    PlayerStatus status;
    AudioSource audioSource;

    void Start(){
        status = FindObjectOfType<PlayerStatus>();
        audioSource = GetComponent<AudioSource>();
    }

	public void CreateDesire()
    {
        ItemDefinition[] items = FindObjectOfType<ConveyerBelt>().itemsToSpawn;
        currentDesire = ScriptableObject.Instantiate(items[UnityEngine.Random.Range(0, items.Length)]);
        CreateEmote(currentDesire.Sprite, emoteBubbleDefault);
    }

	public void LoseDesire(int penalty = 1){
        audioSource.PlayOneShot(madSFX);
        status.ChangeSecurityPoints(-penalty);
		currentDesire = null;
		DestroyCurrentEmote();
		StartCoroutine(ActivateTempBubble(angryIcon, emoteBubbleMad));
	}
            
    public void FulfillDesire(int reward = 1){
        audioSource.PlayOneShot(happySFX);
        status.ChangeSecurityPoints(reward);
        currentDesire = null;
        DestroyCurrentEmote();
		StartCoroutine(ActivateTempBubble(happyIcon, emoteBubbleHappy));
	}

    private Emote CreateEmote(Sprite emoteSprite, Emote emoteBubble)
    {
        GameObject emoteObj = Instantiate(emoteBubble.gameObject, transform.position + emoteOffset, Quaternion.identity, transform);
        currentEmote = emoteObj.GetComponent<Emote>();
        currentEmote.SetEmoteIcon(emoteSprite);
        return currentEmote;
    }

    private IEnumerator ActivateTempBubble(Sprite icon, Emote emoteBubble)
    {
        Emote tempEmote = CreateEmote(icon, emoteBubble);
		yield return new WaitForSecondsRealtime(destroyEmoteTime);
		Destroy(tempEmote.gameObject, .1f);
		
    }

    private void DestroyCurrentEmote()
    {
		if (currentEmote){
			Destroy(currentEmote.gameObject, .1f);
		}
        
    }

    public void GetItem(IInventoryItem item)
    {
        if (item == null) { return; }
        if (currentDesire != null && currentDesire.Name == item.Name){
            FulfillDesire(item.Points);
        }else{
            LoseDesire(item.Points);
        }
    }

    void OnMouseOver()
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()){
            renderer.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()){
            renderer.color = Color.white;
        }
    }
}
