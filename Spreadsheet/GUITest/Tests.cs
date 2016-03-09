using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileAnalyzer;

namespace GUITest
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void OpenClose()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireCloseEvent();
            Assert.IsTrue(view.CalledDoClose);

            view.FireOpenEvent();
            Assert.IsTrue(view.CalledOpenNew);
        }

        [TestMethod]
        public void SetGetDouble()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "7.5");

            Assert.AreEqual("7.5", view.GetCellValue(0, 10));
        }

        [TestMethod]
        public void SetGetFormula_OneCell()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "=7.5 * 4");

            Assert.AreEqual("30", view.GetCellValue(0, 10));
        }

        [TestMethod]
        public void SetGetString()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "6.5 * 3/2");

            Assert.AreEqual("6.5 * 3/2", view.GetCellValue(0, 10));
        }

        [TestMethod]
        public void ContentsBoxCheck()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "6.5 * 3/2");
            view.FireUpdateContentsBoxEvent("A11");
            view.FireUpdateContentsBoxEvent("A11");

            Assert.AreEqual("6.5 * 3/2", view.ContentsBox);
        }

        [TestMethod]
        public void ContentsPlusValueCheck()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "=6.5 * 4/2");
            view.FireUpdateContentsBoxEvent("A11");
            view.FireUpdateValueBoxEvent("A11");


            Assert.AreEqual("=6.5 * 4 / 2", view.ContentsBox);
            Assert.AreEqual("13", view.ValueBox);
        }
    }
}