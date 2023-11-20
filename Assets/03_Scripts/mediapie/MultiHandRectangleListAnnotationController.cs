// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Unity
{
  public class MultiHandRectangleListAnnotationController : AnnotationController<MultiHandRectangleListAnnotation>
  {
    private IList<NormalizedRect> _currentTarget;
    private IList<ClassificationList> _currentHandedness;

    public void DrawNow(IList<NormalizedRect> target, IList<ClassificationList> handedness = null)
    {
      _currentTarget = target;
      _currentHandedness = handedness;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedRect> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(IList<ClassificationList> handedness)
    {
      UpdateCurrentTarget(handedness, ref _currentHandedness);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget);

      if (_currentHandedness != null)
      {
        annotation.SetHandedness(_currentHandedness);
      }
      _currentHandedness = null;
    }
  }
}
