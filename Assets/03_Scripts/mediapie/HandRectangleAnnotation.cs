// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;
using UnityEngine.UI;

using mplt = Mediapipe.LocationData.Types;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class HandRectangleAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private Image _targetImage;
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _color = Color.black;
    [SerializeField] private Color _leftRectColor = Color.green;
    [SerializeField] private Color _rightRectColor = Color.red;  
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

    private static readonly Vector3[] _EmptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    private Vector3 prevPos;
    private Vector2 prevSize;
    private Vector2 nomerizedPos;
    private Hand currentHandedness;

    private void OnEnable()
    {
      ApplyColor(_color);
      ApplyLineWidth(_lineWidth);
    }

    private void OnDisable()
    {
      ApplyLineWidth(0.0f);
      _lineRenderer.SetPositions(_EmptyPositions);
      
      if (currentHandedness == Hand.Left)
      {
        TrackingDataSender.LeftHandStatus.IsActive = false;
      } 
      else if (currentHandedness == Hand.Right)
      {
        TrackingDataSender.RightHandStatus.IsActive = false;
      }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyColor(_color);
        ApplyLineWidth(_lineWidth);
      }
    }
#endif

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetLeftColor(Color color)
    {
      _leftRectColor = color;
    }

    public void SetRightColor(Color color)
    {
      _rightRectColor = color;
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(_lineWidth);
    }

    public void Draw(Vector3[] positions)
    {
      _lineRenderer.SetPositions(positions ?? _EmptyPositions);
      Vector3 containerV3 = Vector3.zero;
      for (int i = 0; i < positions.Length; i++)
      {
        containerV3.x += positions[i].x;
        containerV3.y += positions[i].y;
        containerV3.z += positions[i].z;
      }
      containerV3.x = containerV3.x / positions.Length;
      containerV3.y = containerV3.y / positions.Length;
      containerV3.z = containerV3.z / positions.Length;
      prevPos = containerV3;

      if (positions.Length >= 4)
      {
        float width = Mathf.Sqrt(Mathf.Pow(positions[1].x - positions[0].x, 2) + Mathf.Pow(positions[1].y - positions[0].y, 2));
        
        prevSize = new Vector2(width, width);
      }
    }

    public void Draw(Rect target, Vector2Int imageSize)
    {
      if (ActivateFor(target))
      {
        Draw(GetScreenRect().GetRectVertices(target, imageSize, rotationAngle, isMirrored));
        
      }
    }

    public void Draw(NormalizedRect target)
    {
      if (ActivateFor(target))
      {
        Draw(GetScreenRect().GetRectVertices(target, rotationAngle, isMirrored));
        nomerizedPos = new Vector2(target.XCenter, target.YCenter);
      }
    }

    public void Draw(LocationData target, Vector2Int imageSize)
    {
      if (ActivateFor(target))
      {
        switch (target.Format)
        {
          case mplt.Format.BoundingBox:
            {
              Draw(GetScreenRect().GetRectVertices(target.BoundingBox, imageSize, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.RelativeBoundingBox:
            {
              Draw(GetScreenRect().GetRectVertices(target.RelativeBoundingBox, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.Global:
          case mplt.Format.Mask:
          default:
            {
              throw new System.ArgumentException($"The format of the LocationData must be BoundingBox or RelativeBoundingBox, but {target.Format}");
            }
        }
      }
    }

    public void Draw(LocationData target)
    {
      if (ActivateFor(target))
      {
        switch (target.Format)
        {
          case mplt.Format.RelativeBoundingBox:
            {
              Draw(GetScreenRect().GetRectVertices(target.RelativeBoundingBox, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.BoundingBox:
          case mplt.Format.Global:
          case mplt.Format.Mask:
          default:
            {
              throw new System.ArgumentException($"The format of the LocationData must be RelativeBoundingBox, but {target.Format}");
            }
        }
      }
    }

    private void ApplyColor(Color color)
    {
      if (_lineRenderer != null)
      {
        // _lineRenderer.startColor = color;
        // _lineRenderer.endColor = color;
        _targetImage.color = color;
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      if (_lineRenderer != null)
      {
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
      }
    }

    public enum Hand
    {
      Left,
      Right,
    }

    public void Tracking(Hand handedness)
    {
      _targetRect.localPosition = prevPos;
      _targetRect.sizeDelta = prevSize;

      if (handedness == Hand.Left)
      {
        TrackingDataSender.LeftHandStatus.Position = prevPos;
        TrackingDataSender.LeftHandStatus.NomerizedPos = nomerizedPos;
        TrackingDataSender.LeftHandStatus.IsActive = true;
      }
      else
      {
        TrackingDataSender.RightHandStatus.Position = prevPos;
        TrackingDataSender.RightHandStatus.NomerizedPos = nomerizedPos;
        TrackingDataSender.RightHandStatus.IsActive = true;
      }
    }

    public void SetHandedness(Hand handedness)
    {
      currentHandedness = handedness;
      if (handedness == Hand.Left)
      {
        SetColor(_leftRectColor);
        Tracking(handedness);
      }
      else if (handedness == Hand.Right)
      {
        SetColor(_rightRectColor);
        Tracking(handedness);
      }
    }

    public void SetHandedness(IList<Classification> handedness)
    {
      if (handedness == null || handedness.Count == 0 || handedness[0].Label == "Left")
      {
        SetHandedness(Hand.Left);
      }
      else if (handedness[0].Label == "Right")
      {
        SetHandedness(Hand.Right);
      }
      // ignore unknown label
    }

    public void SetHandedness(ClassificationList handedness)
    {
      SetHandedness(handedness.Classification);
    }
  }
}
