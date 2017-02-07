﻿using System;
using System.Collections.Generic;
using Rubberduck.Common;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.QuickFixes;
using Rubberduck.Inspections.Resources;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Symbols;
using Rubberduck.Parsing.VBA;
using Rubberduck.SmartIndenter;

namespace Rubberduck.Inspections.Results
{
    public class EncapsulatePublicFieldInspectionResult : InspectionResultBase
    {
        private IEnumerable<QuickFixBase> _quickFixes;
        private readonly RubberduckParserState _state;
        private readonly IIndenter _indenter;

        public EncapsulatePublicFieldInspectionResult(IInspection inspection, InspectionResultTarget target, string name)
            : base(inspection, target, name) { }

        [Obsolete]
        public EncapsulatePublicFieldInspectionResult(IInspection inspection, Declaration target, RubberduckParserState state, IIndenter indenter)
            : base(inspection, target)
        {
            _state = state;
            _indenter = indenter;
        }

        public override IEnumerable<IQuickFix> QuickFixes
        {
            get
            {
                return _quickFixes ?? (_quickFixes = new QuickFixBase[]
                {
                    new EncapsulateFieldQuickFix(Target.Context, Target.QualifiedSelection, Target, _state, _indenter),
                    new IgnoreOnceQuickFix(Context, QualifiedSelection, Inspection.AnnotationName)
                });
            }
        }

        public override string Description
        {
            get { return string.Format(InspectionsUI.EncapsulatePublicFieldInspectionResultFormat, Target.IdentifierName).Captialize(); }
        }
    }
}
