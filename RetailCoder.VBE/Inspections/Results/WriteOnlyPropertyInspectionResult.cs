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
    public class WriteOnlyPropertyInspectionResult : InspectionResultBase
    {
        private IEnumerable<QuickFixBase> _quickFixes;

        public WriteOnlyPropertyInspectionResult(IInspection inspection, InspectionResultTarget target, string name)
            : base(inspection, target, name) { }

        [Obsolete]
        public WriteOnlyPropertyInspectionResult(IInspection inspection, Declaration target) 
            : base(inspection, target) { }

        public override string Description
        {
            get { return string.Format(InspectionsUI.WriteOnlyPropertyInspectionResultFormat, Target.IdentifierName).Captialize(); }
        }

        public override IEnumerable<IQuickFix> QuickFixes
        {
            get
            {
                return _quickFixes ?? (_quickFixes = new QuickFixBase[]
                {
                    new WriteOnlyPropertyQuickFix(Context, Target),
                    new IgnoreOnceQuickFix(Context, QualifiedSelection, Inspection.AnnotationName)
                });
            }
        }
    }
}