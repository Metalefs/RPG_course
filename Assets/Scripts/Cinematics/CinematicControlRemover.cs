using RPG.Core;
using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Debug.Log("DisableControl");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<Mover>().Cancel();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<Mover>().Cancel();
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

