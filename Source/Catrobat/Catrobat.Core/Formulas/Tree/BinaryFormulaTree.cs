﻿using Catrobat.IDE.Core.Formulas;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Formulas.Tree
{
    abstract partial class BinaryFormulaTree
    {
        #region Implements IFormulaInterpreter

        protected bool IsNumberN2N()
        {
            // TODO: meaningful (translated?) error message
            if (!FirstChild.IsNumber()) throw new SemanticsErrorException(this, "First child must be number");
            if (!SecondChild.IsNumber()) throw new SemanticsErrorException(this, "Second child must be number");
            return true;
        }

        protected bool IsNumberN2L()
        {
            // TODO: meaningful (translated?) error message
            if (!FirstChild.IsNumber()) throw new SemanticsErrorException(this, "First child must be number");
            if (!SecondChild.IsNumber()) throw new SemanticsErrorException(this, "Second child must be number");
            return false;
        }

        protected bool IsNumberT2T()
        {
            // TODO: meaningful (translated?) error message
            var t = FirstChild.IsNumber();
            if (!SecondChild.IsNumber() == t) throw new SemanticsErrorException(this, "Children are different");
            return t;
        }

        protected bool IsNumberT2L()
        {
            // TODO: meaningful (translated?) error message
            var t = FirstChild.IsNumber();
            if (!SecondChild.IsNumber() == t) throw new SemanticsErrorException(this, "Children are different");
            return false;
        }

        #endregion
    }
}
