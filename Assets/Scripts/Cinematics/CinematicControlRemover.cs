using RPG.Core;
using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        
        GameObject player;
        void Awake()
        {
           player = GameObject.FindWithTag("Player");
        }

        private void OnEnable(){
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            Debug.Log("DisableControl");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<Mover>().Cancel();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<Mover>().Cancel();
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

