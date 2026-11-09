
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class StairsLayerTrigger : MonoBehaviour
    {
        public Direction direction;

        [Space]
        public string layerUpper;
        public string sortingLayerUpper;

        [Space]
        public string layerLower;
        public string sortingLayerLower;

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject target = GetTarget(other);

            if (ShouldGoUpper(target.transform))
            {
                SetLayerAndSortingLayer(
                    target,
                    layerUpper,
                    sortingLayerUpper
                );
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GameObject target = GetTarget(other);

            if (ShouldGoUpper(target.transform))
            {
                SetLayerAndSortingLayer(
                    target,
                    layerLower,
                    sortingLayerLower
                );
            }
        }

        private GameObject GetTarget(Collider2D other)
        {
            if (other.attachedRigidbody != null)
                return other.attachedRigidbody.gameObject;

            return other.transform.root.gameObject;
        }

        private bool ShouldGoUpper(Transform target)
        {
            switch (direction)
            {
                case Direction.South:
                    return target.position.y < transform.position.y;

                case Direction.West:
                    return target.position.x < transform.position.x;

                case Direction.East:
                    return target.position.x > transform.position.x;
            }

            return false;
        }

        private void SetLayerAndSortingLayer(
            GameObject target,
            string layer,
            string sortingLayer)
        {
            int layerIndex = LayerMask.NameToLayer(layer);

            if (layerIndex == -1)
            {
                Debug.LogWarning("Layer tidak ditemukan: " + layer);
                return;
            }

            SetLayerRecursive(target.transform, layerIndex);

            SpriteRenderer[] renderers =
                target.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (SpriteRenderer sr in renderers)
            {
                sr.sortingLayerName = sortingLayer;
            }
        }

        private void SetLayerRecursive(Transform target, int layer)
        {
            target.gameObject.layer = layer;

            foreach (Transform child in target)
            {
                SetLayerRecursive(child, layer);
            }
        }

        public enum Direction
        {
            North,
            South,
            West,
            East
        }
    }
}
