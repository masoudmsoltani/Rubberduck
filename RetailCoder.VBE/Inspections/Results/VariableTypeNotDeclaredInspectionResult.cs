using System;
using System.Collections.Generic;
using Rubberduck.Common;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.QuickFixes;
using Rubberduck.Inspections.Resources;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Symbols;

namespace Rubberduck.Inspections.Results
{
    public class VariableTypeNotDeclaredInspectionResult : InspectionResultBase
    {
        private IEnumerable<QuickFixBase> _quickFixes;

        public VariableTypeNotDeclaredInspectionResult(IInspection inspection, InspectionResultTarget target, string name)
            : base(inspection, target, name) { }

        [Obsolete]
        public VariableTypeNotDeclaredInspectionResult(IInspection inspection, Declaration target)
            : base(inspection, target) { }

        public override string Description
        {
            get
            {
                return string.Format(InspectionsUI.ImplicitVariantDeclarationInspectionResultFormat, 
                    Target.DeclarationType,
                    Target.IdentifierName).Captialize();
            }
        }

        public override IEnumerable<IQuickFix> QuickFixes
        {
            get
            {
                return _quickFixes ?? (_quickFixes = new QuickFixBase[]
                {
                    new DeclareAsExplicitVariantQuickFix(Context, QualifiedSelection), 
                    new IgnoreOnceQuickFix(Context, QualifiedSelection, Inspection.AnnotationName)
                });
            }
        }
    }
}
