using UnityEngine;

[ExecuteAlways] 
[RequireComponent(typeof(RectTransform), typeof(BoxCollider2D))]
public class AutoSizeCollider : MonoBehaviour
{
    private RectTransform _rectTransform;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
    }
    
    private void OnRectTransformDimensionsChange()
    {
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {

        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        
        if (_rectTransform != null && _collider != null)
        {

            _collider.size = _rectTransform.rect.size;
            
            _collider.offset = Vector2.zero; 
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateColliderSize();
    }
#endif
}