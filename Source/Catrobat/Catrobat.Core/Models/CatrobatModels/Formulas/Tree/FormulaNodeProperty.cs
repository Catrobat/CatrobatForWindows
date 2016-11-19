﻿using System.Diagnostics;

namespace Catrobat.IDE.Core.Models.Formulas.Tree
{
    /// <remarks>See /catroid/src/org/catrobat/catroid/formulaeditor/Sensors.java</remarks>
    public abstract partial class FormulaNodeProperty : ConstantFormulaTree
    {
    }

    #region Implementations

    [DebuggerDisplay("Property = Sprite.Brightness")]
    public partial class FormulaNodeBrightness : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.Layer")]
    public partial class FormulaNodeLayer : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.Transparency")]
    public partial class FormulaNodeTransparency : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.PositionX")]
    public partial class FormulaNodePositionX : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.PositionY")]
    public partial class FormulaNodePositionY : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.Rotation")]
    public partial class FormulaNodeRotation : FormulaNodeProperty
    {
    }

    [DebuggerDisplay("Property = Sprite.Size")]
    public partial class FormulaNodeSize : FormulaNodeProperty
    {
    }

    #endregion
}