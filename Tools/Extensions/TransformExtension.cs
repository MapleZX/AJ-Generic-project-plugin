using UnityEngine;
namespace AJ.Generic.Extension
{
    public static class TransformExtension
    {
        public static void LookAt2D(this Transform transform, float opposite = -1)
        {
            var mousePos = Input.mousePosition;
            var target = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x -= target.x;
            mousePos.y -= target.y;

            var angel = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, opposite * angel));
        }
        public static void LookAt2D(this Transform transform, float duration, float opposite = -1)
        {
            var mousePos = Input.mousePosition;
            var target = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x -= target.x;
            mousePos.y -= target.y;

            var angel = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, opposite * angel)), duration);
        }
        public static void LookAt2D(this Transform transform, Vector3 direction, float opposite = -1)
        {
            var mousePos = direction;
            var angel = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, opposite * angel));
        }
        public static void LookAt2D(this Transform transform, Vector3 direction, float duration,  float opposite = -1)
        {
            var mousePos = direction;
            var angel = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, opposite * angel)), duration);
        }
        public static void Revolution(this Transform transform, Transform center, ref float angled, float radius, float speed = (2 * Mathf.PI / 5), float opposite = -1)
        {         
            var posX = radius * Mathf.Sin(angled /* * Mathf.Deg2Rad */);
            var posY = radius * Mathf.Cos(angled /* * Mathf.Deg2Rad */);
            transform.position = new Vector3(posX, posY, transform.position.z) + center.position;
            angled += speed * Time.deltaTime * opposite;
        }
    }
}
