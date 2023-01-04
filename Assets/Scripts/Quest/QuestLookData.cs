using Cysharp.Threading.Tasks;
using UnityEngine;

public class QuestLookData : MonoBehaviour
{
    bool _isWaiting;

    private async void OnWillRenderObject()
    {
        if (_isWaiting) return;
        _isWaiting = true;

        if (Physics.Raycast(transform.position, (Camera.current.transform.position - transform.position).normalized, out RaycastHit hit, (Camera.current.transform.position - transform.position).magnitude, 1 << 8))
        {
            _isWaiting = false;
            return;
        }

        Logger.Log(Camera.current.name);

        await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

        _isWaiting = false;
        QuestManager.Instance.SucceedChecker.CheckQuestPlayActionSucceed(Dango.Quest.QuestPlayAction.PlayerAction.Look);
    }
}
