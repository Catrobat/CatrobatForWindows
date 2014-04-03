﻿using Catrobat.IDE.Core.CatrobatObjects.Formulas.FormulaTree;
using Catrobat.IDE.Core.CatrobatObjects.Variables;
using Catrobat.IDE.Core.FormulaEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Catrobat.IDE.Tests.Tests.IDE.Formula
{
    [TestClass]
    public class FormulaParserTests
    {
        private static readonly IEnumerable<UserVariable> UserVariables = new[]
        {
            new UserVariable { Name = "UserVariable1" }, 
            new UserVariable { Name = "UserVariable2" }
        };
        private readonly FormulaParser _parser = new FormulaParser(UserVariables, null);
        private readonly Random _random = new Random();
        private readonly IFormulaTree _nodeZero = FormulaTreeFactory.CreateNumberNode(0);
        private readonly IFormulaTree _nodeOne = FormulaTreeFactory.CreateNumberNode(1);

        [TestMethod]
        public void TestNullOrWhitespace()
        {
            foreach (var input in new[] { null, string.Empty, " ", "  " })
            {
                ParsingError parsingError;
                IFormulaTree formula;
                Assert.IsTrue(_parser.Parse(input, out formula, out parsingError));
                Assert.IsNull(parsingError);
                Assert.IsNull(formula);
            }
        }

        #region numbers

        [TestMethod]
        public void TestNumber()
        {
            foreach (var value in new[] { 0, _random.Next(), -_random.Next(), _random.NextDouble() })
            {
                TestParser(
                    input: value.ToString(CultureInfo.CurrentCulture),
                    expectedFormula: FormulaTreeFactory.CreateNumberNode(value));
            }
        }

        [TestMethod]
        public void TestPi()
        {
            foreach (var input in new[] { "pi", "PI", "Pi" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreatePiNode());
            }
        }

        #endregion

        #region arithmetic

        [TestMethod]
        public void TestAdd()
        {
            foreach (var input in new[] { "0+1", "0 + 1" })
            {
                TestParser(
                    input: input, 
                    expectedFormula: FormulaTreeFactory.CreateAddNode(_nodeZero, _nodeOne));
            }
            Assert.Inconclusive("TODO: what to do in the case +5 ?");
        }

        [TestMethod]
        public void TestSubtract()
        {
            foreach (var input in new[] { "0-1", "0 - 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateSubtractNode(_nodeZero, _nodeOne));
            }
            Assert.Inconclusive("TODO: what to do in the case -5 ?");
        }

        [TestMethod]
        public void TestMultiply()
        {
            foreach (var input in new[] { "0*1", "0 * 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateMultiplyNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestDivide()
        {
            foreach (var input in new[] { "0/1", "0 / 1", "0:1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateDivideNode(_nodeZero, _nodeOne));
            }
        }

        #endregion

        #region relational operators

        [TestMethod]
        public void TestEquals()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestNotEquals()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestLess()
        {
            foreach (var input in new[] { "0<1", "0 < 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateLessNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestLessEqual()
        {
            foreach (var input in new[] { "0<=1", "0 <= 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateLessEqualNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestGreater()
        {
            foreach (var input in new[] { "0>1", "0 > 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateGreaterNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestGreaterEqual()
        {
            foreach (var input in new[] { "0>=1", "0 >= 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateGreaterEqualNode(_nodeZero, _nodeOne));
            }
        }

        #endregion

        #region logic

        [TestMethod]
        public void TestTrue()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestFalse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestAnd()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestOr()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestNot()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region min/max

        [TestMethod]
        public void TestMin()
        {
            foreach (var input in new[] { "min{0,1}", "Min{0,1}", "min{0, 1}", "min(0, 1)", "min 0, 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateMinNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestMax()
        {
            foreach (var input in new[] { "max{0,1}", "Max{0,1}", "max{0, 1}", "max(0, 1)", "max 0, 1" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateMaxNode(_nodeZero, _nodeOne));
            }
        }

        #endregion

        #region exponential function and logarithms

        [TestMethod]
        public void TestExp()
        {
            foreach (var input in new[] { "exp(0)", "Exp(0)", "exp 0", "e^0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateExpNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestLog()
        {
            foreach (var input in new[] { "log(0)", "Log(0)", "log 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateLogNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestLn()
        {
            foreach (var input in new[] { "ln(0)", "ln(0)", "ln 0" })
            {
                TestParser(
                    input: input,
                    expectedFormula: FormulaTreeFactory.CreateLnNode(_nodeZero));
            }
        }

        #endregion

        #region trigonometric functions

        [TestMethod]
        public void TestSin()
        {
            foreach (var input in new[] { "sin(0)", "Sin(0)", "sin 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateSinNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestCos()
        {
            foreach (var input in new[] { "cos(0)", "Cos(0)", "cos 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateCosNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestTan()
        {
            foreach (var input in new[] { "tan(0)", "Tan(0)", "tan 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateTanNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestArcsin()
        {
            foreach (var input in new[] { "arcsin(0)", "Arcsin(0)", "ArcSin(0)", "arcsin 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateArcsinNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestArccos()
        {
            foreach (var input in new[] { "arccos(0)", "Arccos(0)", "ArcCos(0)", "arccos 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateArccosNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestArcTan()
        {
            foreach (var input in new[] { "arctan(0)", "Arctan(0)", "ArcTan(0)", "arctan 0" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateArctanNode(_nodeZero));
            }
        }

        #endregion

        #region miscellaneous functions

        [TestMethod]
        public void TestSqrt()
        {
            foreach (var input in new[] { "sqrt(0)", "Sqrt(0)", "sqrt 0", "sqrt{0}" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateSqrtNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestAbs()
        {
            foreach (var input in new[] { "|0|", "abs(0)", "Abs(0)", "abs 0", "abs{0}" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateAbsNode(_nodeZero));
            }
        }

        [TestMethod]
        public void TestMod()
        {
            foreach (var input in new[] { "0 mod 1", "0 Mod 1" })
            {
                TestParser(
                   input: input,
                   expectedFormula: FormulaTreeFactory.CreateModuloNode(_nodeZero, _nodeOne));
            }
        }

        [TestMethod]
        public void TestRound()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestRandom()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region sensors

        [TestMethod]
        public void TestSensors()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region object variables

        [TestMethod]
        public void TestObjectVariables()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region user variables

        [TestMethod]
        public void TestUserVariable()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region brackets

        [TestMethod]
        public void TestParentheses()
        {
            foreach (var input in new[] { "(0)", "( 0 )", "{0}", "[0]", "((0))", "(((0)))" })
            {
                TestParser(
                  input: input,
                  expectedFormula: FormulaTreeFactory.CreateParenthesesNode(_nodeZero));
            }
        }

        #endregion

        #region Helpers

        private void TestParser(string input, IFormulaTree expectedFormula)
        {
            ParsingError parsingError;
            IFormulaTree formula;
            Assert.IsTrue(_parser.Parse(input, out formula, out parsingError));
            Assert.IsNull(parsingError);
            Assert.AreEqual(expectedFormula, formula);
        }

        #endregion

    }
}
