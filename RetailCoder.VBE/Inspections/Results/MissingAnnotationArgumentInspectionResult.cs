using System;
using Rubberduck.Common;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Resources;
using Rubberduck.Parsing;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Inspections.Abstract;

namespace Rubberduck.Inspections.Results
{
    public class MissingAnnotationArgumentInspectionResult : InspectionResultBase
    {
        public MissingAnnotationArgumentInspectionResult(IInspection inspection, InspectionResultTarget target, string name)
            : base(inspection, target, name) { }

        [Obsolete]
        public MissingAnnotationArgumentInspectionResult(IInspection inspection, QualifiedContext<VBAParser.AnnotationContext> qualifiedContext)
            : base(inspection, qualifiedContext.ModuleName, qualifiedContext.Context)
        {
        }

        public override string Description
        {
            get { return string.Format(InspectionsUI.MissingAnnotationArgumentInspectionResultFormat, ((VBAParser.AnnotationContext)Context).annotationName().GetText()).Captialize(); }
        }
    }
}