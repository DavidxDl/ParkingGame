using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    
    public event EventHandler<OnGearChangeEventArgs> OnGearChange;
    public class OnGearChangeEventArgs : EventArgs{
        public string gearString;
    }
    public enum Gears{
        Neutral,
        Drive,
        Reverse,
        Park
    }


    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform[] wheels;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField]private float torque = 20;
    [SerializeField]private float reverseTorque = 13;
    [SerializeField]private float wAngle = 45;
    [SerializeField]private int breakStrg = 1000;
    private GameManager gameManager;
    private float gas;
    private float breaking;
    float timer;
    



   



    private Rigidbody rb;
    private Gears gear;
    
private void Awake() {
    
     rb = GetComponent<Rigidbody>();
     boxCollider = GetComponent<BoxCollider>();

}

    void Start()
    {
     gameManager = GameManager.Instance; 
     gear = Gears.Neutral; 
     Cursor.visible = false;
     Cursor.lockState = CursorLockMode.Locked;
     gear = Gears.Neutral;
     OnGearChange?.Invoke(this, new OnGearChangeEventArgs
                {
                    gearString = "Neutral"
                });
     

     gameInput.OnGas += GameInput_OnGas;
     gameInput.OnGasCancel += GameInput_OnGasCancel;
     gameInput.OnGearChange += GameInput_OnGearChange;


     
    }

    private void GameInput_OnGearChange(object sender, EventArgs e)
    {
        ChangeGears();
    }

    private void GameInput_OnGasCancel(object sender, EventArgs e)
    {
        gas = 0;
    }

    private void GameInput_OnGas(object sender, EventArgs e)
    {
        if(gear == Gears.Drive) gas = 1;
        if(gear == Gears.Reverse) gas = -1;
        
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckPark();
        HandleGears();
        Debug.Log(transform.rotation.eulerAngles.y);

    }




    private void HandleGears()
    {
        switch (gear)
        {
            case Gears.Neutral:
            HandleVehicle();
                break;
            case Gears.Drive:
                Drive();
                break;
            case Gears.Reverse:
                Reverse();
                break;
            case Gears.Park:
            HandleVehicle();
            BreakTorque(breakStrg);
                break;
        }

       
    }

    private void ChangeGears()
    {
        switch (gear)
        {

            case Gears.Neutral:
                gear = Gears.Drive;
                OnGearChange?.Invoke(this, new OnGearChangeEventArgs
                {
                    gearString = "Drive"
                });
                break;
            case Gears.Drive:
                gear = Gears.Reverse;
                OnGearChange?.Invoke(this, new OnGearChangeEventArgs
                {
                    gearString = "Reverse"
                });
                break;
            case Gears.Reverse:
                gear = Gears.Park;
                OnGearChange?.Invoke(this, new OnGearChangeEventArgs
                {
                    gearString = "Park"
                });
                break;
            case Gears.Park:
                gear = Gears.Neutral;

                OnGearChange?.Invoke(this, new OnGearChangeEventArgs
                {
                    gearString = "Neutral"
                });
                break;
        }
    }

    private void CheckPark()
    {
        float offset = 2f;
        if (Physics.Raycast(boxCollider.bounds.center, Vector3.down, out RaycastHit rayInfo, boxCollider.bounds.extents.y + offset, layerMask))
        {

            rayInfo.transform.GetComponent<Goal>().SetGreenColor();
            gameManager.EnableParkText();
            if(gear == Gears.Park)
            {
                timer += Time.deltaTime;
                 Debug.Log("PARK!");
                gameManager.DisableParkText();
                if(timer > 2f)gameManager.RestartGame();

            
            };
            
        }
    }

    void Drive()
    {
        HandleVehicle();
         for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = torque * gas;
            }

    }
    

    private void Reverse(){
        HandleVehicle();
         for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = reverseTorque * gas;
            }


    }
    private void HandleVehicle(){
         breaking = gameInput.GetBreaking();


        
        wheelColliders[0].steerAngle = gameInput.GetInputSteer() * wAngle;
        wheelColliders[1].steerAngle = gameInput.GetInputSteer() * wAngle;

        SetWheel(wheelColliders[0], wheels[0]);
        SetWheel(wheelColliders[1], wheels[1]);
        SetWheel(wheelColliders[2], wheels[2]);
        SetWheel(wheelColliders[3], wheels[3]);
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (breaking > 0)
            {
                BreakTorque(breakStrg);
            }
            else
            {
                foreach (WheelCollider wheel in wheelColliders)
                {
                    BreakTorque(0);
                }
            }
        }

    }

    private void BreakTorque(int BreakTorqueAmmount)
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.brakeTorque = BreakTorqueAmmount;
        }
    }

    private void OnTriggerExit(Collider other) {
        other.GetComponent<Goal>().SetDefaultColor();
        gameManager.DisableParkText();
    }
    private void OnCollisionEnter(Collision other) {
        Debug.Log("GameOver");
        gameManager.RestartGame();
    }
     void SetWheel(WheelCollider wheelCol, Transform wheelMesh)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCol.GetWorldPose(out pos, out rot);

        wheelMesh.position = pos;
        wheelMesh.rotation = rot;
    }
}
