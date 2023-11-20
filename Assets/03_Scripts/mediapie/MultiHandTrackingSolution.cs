// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.HandTracking
{
  public class MultiHandTrackingSolution : ImageSourceSolution<HandTrackingGraph>
  {
    [SerializeField] private MultiHandRectangleListAnnotationController _multiHandListAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;

    public HandTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => graphRunner.maxNumHands;
      set => graphRunner.maxNumHands = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnPalmDetectectionsOutput += OnPalmDetectionsOutput;
        graphRunner.OnHandRectsFromPalmDetectionsOutput += OnHandRectsFromPalmDetectionsOutput;
        graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
        // TODO: render HandWorldLandmarks annotations
        graphRunner.OnHandRectsFromLandmarksOutput += OnHandRectsFromLandmarksOutput;
        graphRunner.OnHandednessOutput += OnHandednessOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_multiHandListAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Detection> palmDetections = null;
      List<NormalizedRect> handRectsFromPalmDetections = null;
      List<NormalizedLandmarkList> handLandmarks = null;
      List<LandmarkList> handWorldLandmarks = null;
      List<NormalizedRect> handRectsFromLandmarks = null;
      List<ClassificationList> handedness = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, false));
      }

      _handLandmarksAnnotationController.DrawNow(handLandmarks, handedness);
      // TODO: render HandWorldLandmarks annotations
      _multiHandListAnnotationController.DrawNow(handRectsFromLandmarks, handedness);
    }

    private void OnPalmDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
    }

    private void OnHandRectsFromPalmDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
    }

    private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _multiHandListAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
      _multiHandListAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
