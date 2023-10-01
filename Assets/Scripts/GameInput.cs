using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public  PlayerInputActions playerInputActions;
    public event EventHandler OnGas;
    public event EventHandler OnGasCancel;
    public event EventHandler OnGearChange;
    float breaking;

 private void Awake() {
    playerInputActions = new PlayerInputActions();
    playerInputActions.Car.Enable();
    playerInputActions.Car.Gas.performed += PlayerInputActions_Gas;
    playerInputActions.Car.Gas.canceled += PlayerInputActions_CancelGas;
    playerInputActions.Car.ChangeGear.performed += PlayerInputActions_ChangeGear;

 }

    private void PlayerInputActions_ChangeGear(InputAction.CallbackContext context)
    {
        OnGearChange?.Invoke(this, EventArgs.Empty);
    }

    
  

    private void PlayerInputActions_CancelGas(InputAction.CallbackContext context)
    {
        
        OnGasCancel?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInputActions_Gas(InputAction.CallbackContext context)
    {
        OnGas?.Invoke(this, EventArgs.Empty);
    }

  public float GetInputSteer() {
    Vector2 inputVector = playerInputActions.Car.Steer.ReadValue<Vector2>();
    float steerInput = inputVector.x;
    
    return steerInput; 

  }
  public float GetBreaking(){
    return playerInputActions.Car.Breake.ReadValue<float>();
  }
}
