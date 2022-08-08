using UnityEngine;
namespace RPG.Interfaces
{
    public abstract class IAction: MonoBehaviour
    {
        abstract public void Cancel();
    }
}