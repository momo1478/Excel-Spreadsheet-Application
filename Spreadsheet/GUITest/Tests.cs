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
        public void NullContentsBoxCheck()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "6.5 * 3/2");
            view.FireUpdateContentsBoxEvent("A1000");

            Assert.AreEqual("", view.ContentsBox);
        }

        [TestMethod]
        public void ContentsBox_Circular()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 0, "=A2");
            view.FireSetContentsEvent(0, 1, "=A1");
            view.FireUpdateContentsBoxEvent("A1");

            Assert.AreEqual("Your input created a circular dependency, Stop it... now." , view.Message);
        }

        [TestMethod]
        public void ContentsBox_InvalidName()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 0, "=A1000");
            view.FireSetContentsEvent(0, 1, "=A1");
            view.FireUpdateContentsBoxEvent("A1");

            Assert.AreEqual("A Cell you input was off the board! I can't let you do that.", view.Message);
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

        [TestMethod]
        public void NullValueCheck()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "=6.5 * 4/2");
            view.FireUpdateValueBoxEvent(null);


            Assert.AreEqual("", view.ValueBox);
        }

        [TestMethod]
        public void SaveTest()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 9, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 8, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 7, "=6.5 * 4/2");
            view.FireUpdateValueBoxEvent("A8");


            Assert.AreEqual("13", view.ValueBox);

            view.FireSaveEvent("UntitledSpreadsheet.ss");
        }

        [TestMethod]
        public void SaveExceptionTest()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireSetContentsEvent(0, 10, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 9, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 8, "=6.5 * 4/2");
            view.FireSetContentsEvent(0, 7, "=6.5 * 4/2");
            view.FireUpdateValueBoxEvent("A8");


            Assert.AreEqual("13", view.ValueBox);

            view.FireSaveEvent("C:/badpath////cdrive/UntitledSpreadsheet.ss");

            Assert.IsTrue(view.Message.Contains("Unable to open file"));
        }

        [TestMethod]
        public void LoadTest()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireFileChosenEvent("UntitledSpreadsheetLoad.ss");

            view.FireUpdateValueBoxEvent("A8");

            Assert.AreEqual("Load success!", view.Message);

        }

        [TestMethod]
        public void LoadExceptionTest()
        {
            ViewStub view = new ViewStub();
            Controller controller = new Controller(view);

            view.FireFileChosenEvent("C:/badpalolololth////cdrive/UntitledSpreadsheet.ss");

            Assert.IsTrue(view.Message.Contains("Unable to open file"));
        }


    }
}