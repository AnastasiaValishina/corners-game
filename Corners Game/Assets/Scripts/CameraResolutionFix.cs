using UnityEngine;

[ExecuteInEditMode]
public class CameraAspectFix : MonoBehaviour
{
	[Header("Настройки масштаба")]
	[SerializeField] float referenceWidth = 1080f;
	[SerializeField] float targetAspectRatio = 1.15f;
	[SerializeField] float pixelsPerUnit = 100f;

	[Header("Настройки позиции (X и Y)")]
	[SerializeField] float xPosLandscape = 0f;
	[SerializeField] float xPosPortrait = 0f;
	[SerializeField] float yPosLandscape = 0f;
	[SerializeField] float yPosPortrait = 4f;

	[SerializeField] float portraitThreshold = 0.56f;

	private Camera cam;

	void Awake()
	{
		cam = GetComponent<Camera>();
	}

	void LateUpdate()
	{
		if (cam == null) return;

		float currentAspectRatio = (float)Screen.width / Screen.height;

		// Корректировка масштаба камеры
		if (currentAspectRatio < targetAspectRatio)
		{
			float constantWidthSize = targetAspectRatio / currentAspectRatio;
			cam.orthographicSize = constantWidthSize * (referenceWidth / (2f * pixelsPerUnit));
		}
		else
		{
			cam.orthographicSize = referenceWidth / (2f * pixelsPerUnit);
		}

		// Вычисляем коэффициент интерполяции t (от 0 до 1)
		float t = Mathf.InverseLerp(targetAspectRatio, portraitThreshold, currentAspectRatio);

		// Плавно вычисляем новые значения для осей X и Y
		float newX = Mathf.Lerp(xPosLandscape, xPosPortrait, t);
		float newY = Mathf.Lerp(yPosLandscape, yPosPortrait, t);

		// Применяем новые координаты к позиции камеры
		Vector3 pos = transform.position;
		pos.x = newX;
		pos.y = newY;
		transform.position = pos;
	}
}