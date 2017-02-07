﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Rubberduck.Inspections.Resources;
using Rubberduck.Parsing;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Symbols;
using Rubberduck.UI;
using Rubberduck.UI.Controls;
using Rubberduck.VBEditor;

namespace Rubberduck.Inspections.Abstract
{
    public abstract class InspectionResultBase : INavigateSource, IInspectionResult, ICopyFormatter
    {
        private readonly InspectionResultTarget _newTarget;

        protected InspectionResultBase(IInspection inspection, InspectionResultTarget target)
            : this(inspection, target, string.Empty) { }

        protected InspectionResultBase(IInspection inspection, InspectionResultTarget target, string identifierName)
        {
            _inspection = inspection;
            _newTarget = target;
            _identifierName = identifierName;
        }
        

        [Obsolete]
        protected InspectionResultBase(IInspection inspection, Declaration target)
            : this(inspection, target.QualifiedName.QualifiedModuleName, target.Context)
        {
            _target = target;
        }

        /// <summary>
        /// Creates an inspection result.
        /// </summary>
        [Obsolete]
        protected InspectionResultBase(IInspection inspection, QualifiedModuleName qualifiedName, ParserRuleContext context = null)
        {
            _inspection = inspection;
            _qualifiedName = qualifiedName;
            _context = context;
        }

        /// <summary>
        /// Creates an inspection result.
        /// </summary>
        [Obsolete]
        protected InspectionResultBase(IInspection inspection, QualifiedModuleName qualifiedName, ParserRuleContext context, Declaration declaration)
        {
            _inspection = inspection;
            _qualifiedName = qualifiedName;
            _context = context;
            _target = declaration;
        }

        private readonly IInspection _inspection;
        public IInspection Inspection { get { return _inspection; } }
        InspectionResultTarget IInspectionResult.Target { get { return _newTarget; } }

        public abstract string Description { get; }

        private readonly QualifiedModuleName _qualifiedName;
        protected QualifiedModuleName QualifiedName { get { return _qualifiedName; } }

        private readonly ParserRuleContext _context;
        public ParserRuleContext Context { get { return _context; } }

        private readonly Declaration _target;
        public Declaration Target { get { return _target; } }

        private readonly string _identifierName;
        public string IdentifierName { get { return _identifierName; } }

        /// <summary>
        /// Gets the information needed to select the target instruction in the VBE.
        /// </summary>
        public virtual QualifiedSelection QualifiedSelection
        {
            get
            {
                return _context == null
                    ? _target.QualifiedSelection
                    : new QualifiedSelection(_qualifiedName, _context.GetSelection());
            }
        }

        /// <summary>
        /// Gets all available "quick fixes" for a code inspection result.
        /// </summary>
        public virtual IEnumerable<IQuickFix> QuickFixes { get { return Enumerable.Empty<QuickFixBase>(); } }

        public bool HasQuickFixes { get { return QuickFixes.Any(); } }

        public virtual IQuickFix DefaultQuickFix { get { return QuickFixes.FirstOrDefault(); } }

        public virtual int CompareTo(IInspectionResult other)
        {
            var result = Inspection.CompareTo(other.Inspection);
            return result > 0 
                ? QualifiedSelection.Selection.CompareTo(other.Target.Selection) 
                : result;
        }

        /// <summary>
        /// WARNING: This property can have side effects. It can change the ActiveVBProject if the result has a null Declaration, 
        /// which causes a flicker in the VBE. This should only be called if it is *absolutely* necessary.
        /// </summary>
        public string ToClipboardString()
        {           
            var module = QualifiedSelection.QualifiedName;
            var documentName = _target != null ? _target.ProjectDisplayName : string.Empty;
            if (string.IsNullOrEmpty(documentName))
            {
                var component = module.Component;
                documentName = component != null ? component.ParentProject.ProjectDisplayName : string.Empty;
            }
            if (string.IsNullOrEmpty(documentName))
            {
                documentName = Path.GetFileName(module.ProjectPath);
            }

            return string.Format(
                InspectionsUI.QualifiedSelectionInspection,
                Inspection.Severity,
                Description,
                "(" + documentName + ")",
                module.ProjectName,
                module.ComponentName,
                QualifiedSelection.Selection.StartLine);
        }

        public virtual NavigateCodeEventArgs GetNavigationArgs()
        {
            return new NavigateCodeEventArgs(QualifiedSelection);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as IInspectionResult);
        }

        public object[] ToArray()
        {
            var module = QualifiedSelection.QualifiedName;
            return new object[] { Inspection.Severity.ToString(), module.ProjectName, module.ComponentName, Description, QualifiedSelection.Selection.StartLine, QualifiedSelection.Selection.StartColumn };
        }
    }
}
