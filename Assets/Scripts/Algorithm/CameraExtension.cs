using UnityEngine;

namespace Algorithm
{
    public static class CameraExtension
    {

        public static Vector2 GetCanvasPosition(this Camera camera, Transform transform, Canvas canvas)
        {
            var viewportPos = camera.WorldToViewportPoint(transform.position);
            var canvasRtm = canvas.GetComponent<RectTransform>();
            var uguiPos = Vector2.zero;
            var sizeDelta = canvasRtm.sizeDelta;
            uguiPos.x = (viewportPos.x - 0.5f) * sizeDelta.x;
            uguiPos.y = (viewportPos.y - 0.5f) * sizeDelta.y;
            return uguiPos;
        }
        
    }
}