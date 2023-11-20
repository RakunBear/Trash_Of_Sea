// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Google.Protobuf.Reflection;
using UnityEngine;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class MultiHandRectangleListAnnotation : ListAnnotation<HandRectangleAnnotation>
  {
    [SerializeField] private Color _leftRectColor = Color.green;
    [SerializeField] private Color _rightRectColor = Color.red;  
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

    

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLeftColor(_leftRectColor);
        ApplyRightColor(_rightRectColor);
        ApplyLineWidth(_lineWidth);
      }
    }
#endif

    public void SetLeftColor(Color color)
    {
      _leftRectColor = color;
      ApplyLeftColor(_leftRectColor);
    }

    public void SetRightColor(Color color)
    {
      _rightRectColor = color;
      ApplyRightColor(_leftRectColor);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(_lineWidth);
    }

    public void Draw(IList<Rect> targets, Vector2Int imageSize)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, imageSize); }
        });
      }
    }

    public void Draw(IList<NormalizedRect> targets)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target); }
        });
      }
    }
    protected override HandRectangleAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLineWidth(_lineWidth);
      annotation.SetLeftColor(_leftRectColor);
      annotation.SetRightColor(_rightRectColor);
      return annotation;
    }
    
    private void ApplyLeftColor(Color color)
    {
      foreach (var rect in children)
      {
        if (rect != null) { rect.SetLeftColor(color); }
      }
    }

    private void ApplyRightColor(Color color)
    {
      foreach (var rect in children)
      {
        if (rect != null) { rect.SetRightColor(color); }
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      foreach (var rect in children)
      {
        if (rect != null) { rect.SetLineWidth(lineWidth); }
      }
    }

    

    public void SetHandedness(IList<ClassificationList> handedness)
    {
      var count = handedness == null ? 0 : handedness.Count;
      for (var i = 0; i < Mathf.Min(count, children.Count); i++)
      {
        children[i].SetHandedness(handedness[i]);
      }
      for (var i = count; i < children.Count; i++)
      {
        children[i].SetHandedness((IList<Classification>)null);
      }
    }
  }
}
