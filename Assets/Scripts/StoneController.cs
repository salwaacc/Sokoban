using UnityEngine;

public class StoneController : MonoBehaviour
{
    [Header("Floor Information")]
    public int currentFloor = 1;

    public void ChangeFloor(int newFloor)
    {
        currentFloor = newFloor;
       
    }
}