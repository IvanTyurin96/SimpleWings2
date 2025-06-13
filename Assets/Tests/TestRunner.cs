using Assets.Tests.Drag;
using Assets.Tests.Lift;
using UnityEngine;

namespace Assets.Tests
{
    /// <summary>
    /// This class should be a component of any object in Unity scene. All tests will running after starting scene. If some test fail, you will see the error in debug console.
    /// </summary>
    public class TestRunner : MonoBehaviour
    {
        private void Start()
        {
            RunTests();
        }

        private void RunTests()
        {
            LiftCurveCalculatorTests liftCurveCalculatorTests = new LiftCurveCalculatorTests();
            liftCurveCalculatorTests.RunTests();

            DragCurveCalculatorTests dragCurveCalculatorTests = new DragCurveCalculatorTests();
            dragCurveCalculatorTests.RunTests();
        }
    }
}
