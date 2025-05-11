using UnityEngine;

public class OrbPlacementManager : MonoBehaviour
{
    public static bool orbPlaced = false;  // Static flag to track orb placement

    public static void SetOrbPlaced()
    {
        orbPlaced = true;
    }
}
