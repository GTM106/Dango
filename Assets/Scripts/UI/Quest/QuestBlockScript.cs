using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBlockScript : MonoBehaviour
{
    [SerializeField] Sprite role;
    [SerializeField] Sprite destination;
    [SerializeField] Sprite action;
    [SerializeField] Sprite none;
    [SerializeField] ImageUIData uIData;

    public void changeSprite(QuestData quest)
    {
        switch (quest.QuestType) {
            case QuestType.CreateRole:
                uIData.ImageData.SetSprite(role);
                break;
            case QuestType.Destination:
                uIData.ImageData.SetSprite(destination);
                break;
            case QuestType.PlayAction:
                uIData.ImageData.SetSprite(action);
                break;
            default:
                uIData.ImageData.SetSprite(none);
                break;
        } }
}
