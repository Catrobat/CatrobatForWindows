﻿using Catrobat.Core.Resources.Localization;
using Catrobat.IDE.Core.Formulas;
using Catrobat.IDE.Core.Models.Formulas.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Formulas.Tree
{
    abstract partial class FormulaNodeInfixOperator
    {
        #region Implements IFormulaTokenizer

        public override IEnumerable<IFormulaToken> Tokenize()
        {
            return (LeftChild == null ? FormulaTokenizer.EmptyChild : LeftChild.Tokenize())
                .Concat(Enumerable.Repeat(CreateToken(), 1))
                .Concat(RightChild == null ? FormulaTokenizer.EmptyChild : RightChild.Tokenize());
        }

        #endregion

        #region Implements IStringBuilderSerializable

        public override void Append(StringBuilder sb)
        {
            if (LeftChild == null)
            {
                sb.Append(FormulaSerializer.EmptyChild);
            }
            else
            {
                LeftChild.Append(sb);
            }
            AppendToken(sb);
            if (RightChild == null)
            {
                sb.Append(FormulaSerializer.EmptyChild);
            }
            else
            {
                RightChild.Append(sb);
            }
        }

        protected virtual void AppendToken(StringBuilder sb)
        {
            sb.Append(Serialize());
        }

        #endregion
    }

    #region Implementations

    partial class FormulaNodeAdd
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreatePlusToken();
        }

        public override double EvaluateNumber()
        {
            return LeftChild.EvaluateNumber() + RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "+";
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    partial class FormulaNodeSubtract
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateMinusToken();
        }

        public override double EvaluateNumber()
        {
            return LeftChild.EvaluateNumber() - RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "-";
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    partial class FormulaNodeMultiply
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateMultiplyToken();
        }

        public override double EvaluateNumber()
        {
            return LeftChild.EvaluateNumber() * RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "*";
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    partial class FormulaNodeDivide
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateDivideToken();
        }

        public override double EvaluateNumber()
        {
            return LeftChild.EvaluateNumber() / RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "/";
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    partial class FormulaNodePower
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateCaretToken();
        }

        public override double EvaluateNumber()
        {
            return Math.Pow(LeftChild.EvaluateNumber(), RightChild.EvaluateNumber());
        }

        public override string Serialize()
        {
            return "^";
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    partial class FormulaNodeEquals
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateEqualsToken();
        }

        public override double EvaluateNumber()
        {
            if (EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            try
            {
                return Math.Abs(LeftChild.EvaluateNumber() - RightChild.EvaluateNumber()) <= double.Epsilon;
            }
            catch (NotSupportedException)
            {
                return LeftChild.EvaluateLogic() == RightChild.EvaluateLogic();
            }
        }

        public override string Serialize()
        {
            return "=";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeNotEquals
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateNotEqualsToken();
        }

        public override double EvaluateNumber()
        {
            if (EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            try
            {
                return Math.Abs(LeftChild.EvaluateNumber() - RightChild.EvaluateNumber()) > double.Epsilon;
            }
            catch (NotSupportedException)
            {
                return LeftChild.EvaluateLogic() != RightChild.EvaluateLogic();
            }
        }

        public override string Serialize()
        {
            return "≠";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeGreater
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateGreaterToken();
        }

        public override double EvaluateNumber()
        {
            if (EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateNumber() > RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return ">";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeGreaterEqual
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateGreaterEqualToken();
        }

        public override double EvaluateNumber()
        {
            if(EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateNumber() >= RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "≥";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeLess
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateLessToken();
        }

        public override double EvaluateNumber()
        {
            if (EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateNumber() < RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "<";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeLessEqual
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateLessEqualToken();
        }

        public override double EvaluateNumber()
        {
            if (EvaluateLogic())
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateNumber() <= RightChild.EvaluateNumber();
        }

        public override string Serialize()
        {
            return "≤";
        }

        public override bool IsNumber()
        {
            return true;
        }
    }

    partial class FormulaNodeAnd
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateAndToken();
        }

        public override double EvaluateNumber()
        {
            if (!LeftChild.EvaluateNumber().Equals(0) && !RightChild.EvaluateNumber().Equals(0))
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateLogic() && RightChild.EvaluateLogic();
        }

        protected override void AppendToken(StringBuilder sb)
        {
            sb.Append(" ");
            base.AppendToken(sb);
            sb.Append(" ");
        }

        public override string Serialize()
        {
            return AppResourcesHelper.Get("Formula_Operator_And");
        }

        public override bool IsNumber()
        {
            return IsNumberT2T();
        }
    }

    partial class FormulaNodeOr
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateOrToken();
        }

        public override double EvaluateNumber()
        {
            if (!LeftChild.EvaluateNumber().Equals(0) || !RightChild.EvaluateNumber().Equals(0))
            {
                return 1;
            }

            return 0;
        }

        public override bool EvaluateLogic()
        {
            return LeftChild.EvaluateLogic() || RightChild.EvaluateLogic();
        }

        protected override void AppendToken(StringBuilder sb)
        {
            sb.Append(" ");
            base.AppendToken(sb);
            sb.Append(" ");
        }

        public override string Serialize()
        {
            return AppResourcesHelper.Get("Formula_Operator_Or");
        }

        public override bool IsNumber()
        {
            return IsNumberT2T();
        }
    }

    partial class FormulaNodeModulo
    {
        protected override IFormulaToken CreateToken()
        {
            return FormulaTokenFactory.CreateModToken();
        }

        public override double EvaluateNumber()
        {
            return LeftChild.EvaluateNumber() % RightChild.EvaluateNumber();
        }

        protected override void AppendToken(StringBuilder sb)
        {
            sb.Append(" ");
            base.AppendToken(sb);
            sb.Append(" ");
        }

        public override string Serialize()
        {
            return AppResourcesHelper.Get("Formula_Operator_Mod");
        }

        public override bool IsNumber()
        {
            return IsNumberN2N();
        }
    }

    #endregion

}
