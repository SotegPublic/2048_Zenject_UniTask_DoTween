using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraScaler : MonoBehaviour
{
    [SerializeField] private float _targetWidth = 5f; // Желаемая ширина игровой области
    [SerializeField] private float _targetHeight = 10f; // Желаемая высота игровой области
    [SerializeField] private bool _isLetterbox = false; // Добавлять ли чёрные полосы (если нужно строгое соотношение)

    private Camera orthoCamera;

    private void Awake()
    {
        orthoCamera = Camera.main;
        UpdateOrthoSize();
    }

    private void UpdateOrthoSize()
    {
        if (orthoCamera == null)
            return;

        float targetAspect = _targetWidth / _targetHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        if (_isLetterbox)
        {
            float scaleRatio = currentAspect > targetAspect
                ? _targetHeight / Screen.height * Screen.width / _targetWidth
                : 1f;

            orthoCamera.orthographicSize = _targetHeight / 2f / scaleRatio;
        }
        else
        {
            if (currentAspect > targetAspect)
            {
                orthoCamera.orthographicSize = _targetHeight / 2f;
            }
            else
            {
                orthoCamera.orthographicSize = _targetWidth / (2f * currentAspect);
            }
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateOrthoSize();
    }
}
