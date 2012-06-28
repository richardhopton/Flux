using System;
using System.Collections.Generic;
using System.IO;
using System.Xaml;
using System.Xml;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    public static class XamlWorkflow
    {
        internal class CustomXamlSchemaContext : XamlSchemaContext, IAncestorProvider
        {
            public Stack<Object> Stack { get; private set; }

            public CustomXamlSchemaContext()
            {
                Stack = new Stack<Object>();
            }

            //public override XamlType GetXamlType(Type type)
            //{
            //    var retval = base.GetXamlType(type);
            //    if (retval == null)
            //        return null;
            //    // insert resolver code here
            //    return retval;
            //}

            //protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
            //{
            //    var retval = base.GetXamlType(xamlNamespace, name, typeArguments);
            //    if (retval == null)
            //        return null;
            //    // insert resolver code here
            //    return retval;
            //}

            IEnumerable<Object> IAncestorProvider.Ancestors
            {
                get { return Stack; }
            }
        }

        internal class CustomXamlObjectWriter : XamlObjectWriter
        {
            public CustomXamlObjectWriter(CustomXamlSchemaContext schemaContext)
                : base(schemaContext)
            {
                _stack = schemaContext.Stack;
            }

            protected override void Dispose(bool disposing)
            {
                _stack = null;
                base.Dispose(disposing);
            }

            private Stack<Object> _stack;

            protected override void OnAfterBeginInit(object value)
            {
                _stack.Push(value);
                base.OnAfterBeginInit(value);
            }

            protected override void OnAfterEndInit(object value)
            {
                base.OnAfterEndInit(value);
                _stack.Pop();
            }
        }

        public static IWorkflow Parse(String xaml)
        {
            var input = new StringReader(xaml);
            using (XmlReader reader = XmlReader.Create(input))
            {
                var xamlSchemaContext = new CustomXamlSchemaContext();
                using (var xamlReader = new XamlXmlReader(reader, xamlSchemaContext))
                {
                    var xamlWriter = new CustomXamlObjectWriter(xamlSchemaContext);
                    XamlServices.Transform(xamlReader, xamlWriter);
                    return xamlWriter.Result as IWorkflow;
                }
            }
        }

        public static IWorkflow Load(String fileName)
        {
            var xaml = File.ReadAllText(fileName);
            return Parse(xaml);
        }

        public static void Save(IWorkflow workflow, String fileName)
        {
            var xaml = Save(workflow);
            File.WriteAllText(fileName, xaml);
        }

        public static String Save(IWorkflow workflow)
        {
            var output = new StringWriter();
            using (var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
            {
                var xamlSchemaContext = new CustomXamlSchemaContext();
                using (var xamlWriter = new XamlXmlWriter(writer, xamlSchemaContext))
                {
                    XamlServices.Save(xamlWriter, workflow);
                }
            }
            return output.ToString();
        }
    }
}
