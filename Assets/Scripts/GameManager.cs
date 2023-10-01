using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gearText;
    [SerializeField] private TextMeshProUGUI parkText;
    [SerializeField] private PlayerController player;
    public static GameManager Instance {get; private set;}

    private void Start() {
        player.OnGearChange += Player_OnGearChange;
    }

    private void Player_OnGearChange(object sender, PlayerController.OnGearChangeEventArgs e)
    {
        gearText.text = "Gear: " + e.gearString;
    }

    private void Awake() {
        Instance = this;
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnableParkText(){
        parkText.gameObject.SetActive(true);
    }
    public void DisableParkText(){
        parkText.gameObject.SetActive(false);
    }

}
