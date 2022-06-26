using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DangoRoleUI
{
    private static string _currentRoleName;

    public static void OnGUIRoleName(string role_name,float score)
    {
        _currentRoleName = "�u" + role_name + "�v�I" + score + "�_�I";
    }

    public static void OnGUIReset()
    {
        _currentRoleName = "";
    }

    public static string CurrentRoleName => _currentRoleName;
}
