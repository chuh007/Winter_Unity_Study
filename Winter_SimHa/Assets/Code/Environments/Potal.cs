using Code.Core.EventSystems;
using Code.Entities;
using Code.Players;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Environments
{
    public enum PortalType
    {
        SceneChange, Teleport
    }

    public class Portal : MonoBehaviour
    {
        [SerializeField] private PortalType portalType;
        [SerializeField] private string nextSceneName;
        [SerializeField] private GameEventChannelSO uiChannel;

        private bool _isTriggerd = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggerd && portalType == PortalType.SceneChange) return;
            if (other.CompareTag("Player"))
            {
                _isTriggerd = true;
                Player player = other.GetComponent<Player>();

                player.GetCompo<EntityMover>().CanManualMove = false;

                FadeEvent fadeEvt = UIEvents.FadeEvent;
                fadeEvt.isFadeIn = false;
                fadeEvt.fadeTime = 0.5f;

                uiChannel.AddListener<FadeCompleteEvent>(HandleFadeComplete);
                uiChannel.RaiseEvent(fadeEvt);
            }
        }

        private void HandleFadeComplete(FadeCompleteEvent @event)
        {
            uiChannel.RemoveListener<FadeCompleteEvent>(HandleFadeComplete);
            SceneManager.LoadScene(nextSceneName);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}