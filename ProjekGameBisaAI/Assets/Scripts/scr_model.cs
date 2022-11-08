using System;
using System.Collections.Generic;
using UnityEngine;

public class scr_model : MonoBehaviour
{
    #region -Weapons-
    [Serializable]
    public class WeaponSettingsModel
    {
        [Header("Weapon Sway")]
        public float swayAmount;
        public bool swayYInverted;
        public bool swayXInverted;
        public float swaySmoothing;
        public float swayResetSmoothing;
        public float swayClampX;
        public float swayClampY;

        [Header("Weapon Movement Sway")]
        public float movementSwayX;
        public float movementSwayY;
        public bool movementSwayYInverted;
        public bool movementSwayXInverted;
        public float movementSwaySmoothing;
    }
    #endregion
}
