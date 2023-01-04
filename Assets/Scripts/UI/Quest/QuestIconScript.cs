using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIconScript : MonoBehaviour
{
    [SerializeField] Sprite roleIcon;
    [SerializeField] Sprite destinationIcon;
    [SerializeField] Sprite actionIcon;
    [SerializeField] Sprite NoneIcon;
    [SerializeField] ImageUIData uidata;
    public void ChangeIcon(QuestData quest)
    {
        switch (quest.QuestType)
        {
            case QuestType.CreateRole:
                uidata.ImageData.SetSprite(roleIcon);
                break;
            case QuestType.Destination:
                uidata.ImageData.SetSprite(destinationIcon);
                break;
            case QuestType.PlayAction:
                uidata.ImageData.SetSprite(actionIcon);
                break;
            default:
                uidata.ImageData.SetSprite(NoneIcon);
                break;
        }
    }
}
