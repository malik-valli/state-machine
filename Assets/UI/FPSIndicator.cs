using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class FPSIndicator : MonoBehaviour
{
    private Text _UItext;
    private int _FPS;
    [Range(0f, 1f), SerializeField] private float _drawInterval = 0.2f; // In seconds

    private void Start()
    {
        _UItext = GetComponent<Text>();
        StartCoroutine(DrawIntervalCoroutine());
    }

    public void Update() => _FPS = (int)(1f / Time.deltaTime);

    private IEnumerator DrawIntervalCoroutine()
    {
        yield return new WaitForSeconds(_drawInterval);

        DrawIndicator();

        StopCoroutine(DrawIntervalCoroutine());
        StartCoroutine(DrawIntervalCoroutine());
    }

    private void DrawIndicator()
    {
        _UItext.text = _FPS.ToString();
        _UItext.color = new Color(1f, _FPS / 60f, _FPS / 60f);
    }
}