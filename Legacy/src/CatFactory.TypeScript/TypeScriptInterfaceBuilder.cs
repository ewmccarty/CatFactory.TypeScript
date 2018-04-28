﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;

namespace CatFactory.TypeScript
{
    public class TypeScriptInterfaceBuilder : TypeScriptCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params TypeScriptInterfaceDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                var codeBuilder = new TypeScriptInterfaceBuilder
                {
                    OutputDirectory = outputDirectory,
                    ForceOverwrite = forceOverwrite,
                    ObjectDefinition = definition
                };

                codeBuilder.CreateFile(subdirectory);
            }
        }

        public TypeScriptInterfaceBuilder()
        {
        }

        public new ITypeScriptInterfaceDefinition ObjectDefinition { get; set; }

        public override string FileName
            => ObjectDefinition.Name;

        public override void Translating()
        {
            var output = new StringBuilder();

            var start = 0;
            
            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
            {
                //output.AppendFormat("namespace {0} {1}", ObjectDefinition.Namespace, "{");
                //output.AppendLine();

                Lines.Add(new CodeLine("namespace {0} {1}", ObjectDefinition.Namespace, "{"));

                start = 1;
            }

            foreach (var attribute in ObjectDefinition.Attributes)
            {
                var dec = new List<string>();

                //output.AppendFormat("{0}@{1}(", Indent(start), attribute.Name);
                dec.Add(string.Format("{0}@{1}(", Indent(start), attribute.Name));

                if (attribute.Sets.Count > 0)
                {
                    //output.Append("{");
                    //output.AppendLine();

                    dec.Add("{");
                    dec.Add("\r\n");

                    for (var i = 0; i < attribute.Sets.Count; i++)
                    {
                        //output.AppendFormat("{0}{1}", Indent(start + 1), attribute.Sets[i]);
                        dec.Add(string.Format("{0}{1}", Indent(start + 1), attribute.Sets[i]));

                        if (i < attribute.Sets.Count - 1)
                        {
                            output.Append(",");
                            dec.Add(";");
                        }

                        //output.AppendLine();
                        dec.Add("\r\n");
                    }

                    output.Append("}");
                    dec.Add("}");
                }

                //output.AppendFormat(")");
                //output.AppendLine();

                //Lines.Add(new CodeLine());
            }
            
            var declaration = new List<string>();

            declaration.Add(string.Format("{0}", ObjectDefinition.Export ? "export" : string.Empty));
            declaration.Add("interface");
            declaration.Add(ObjectDefinition.Name);

            //output.AppendFormat("{0}{1}interface {2}", Indent(start), ObjectDefinition.Export ? "export " : string.Empty, ObjectDefinition.Name);
            //Lines.Add(new CodeLine());

            if (ObjectDefinition.HasInheritance && ObjectDefinition.Implements.Count > 0)
            {
                //output.AppendFormat(" implements {0}", string.Join(", ", ObjectDefinition.Implements));
                //Lines.Add(new CodeLine(" implements {0}", string.Join(", ", ObjectDefinition.Implements));

                declaration.Add("implements");
                declaration.Add(string.Join(", ", ObjectDefinition.Implements));
            }

            declaration.Add("{");

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            //output.AppendFormat(" {0}", "{");
            //output.AppendLine();

            //Lines.Add(new CodeLine("{"));

            if (ObjectDefinition.Properties.Count > 0)
            {
                foreach (var property in ObjectDefinition.Properties)
                {
                    //output.AppendFormat("{0}{1}: {2};", Indent(start + 1), property.Name, property.Type);
                    //output.AppendLine();

                    Lines.Add(new CodeLine("{0}{1}: {2};", Indent(start + 1), property.Name, property.Type));
                }
            }

            if (ObjectDefinition.Methods.Count > 0)
            {
                foreach (var method in ObjectDefinition.Methods)
                {
                    var parameters = string.Join(", ", method.Parameters.Select(item => string.Format("{0}: {1}", item.Name, item.Type)));

                    //output.AppendFormat("{0}{1}({2}): {3};", Indent(start + 1), method.Name, method.Parameters.Count == 0 ? string.Empty : parameters, method.Type);
                    //output.AppendLine();

                    Lines.Add(new CodeLine("{0}{1}({2}): {3};", Indent(start + 1), method.Name, method.Parameters.Count == 0 ? string.Empty : parameters, method.Type));
                }
            }

            //output.AppendFormat("{0}{1}", Indent(start), "}");
            //output.AppendLine();

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
            {
                //output.AppendFormat("{0}", "}");
                //output.AppendLine();

                Lines.Add(new CodeLine("}"));
            }
        }

        //public override string Code
        //{
        //    get
        //    {
        //        var output = new StringBuilder();

        //        var start = 0;

        //        if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
        //        {
        //            output.AppendFormat("namespace {0} {1}", ObjectDefinition.Namespace, "{");
        //            output.AppendLine();

        //            start = 1;
        //        }

        //        foreach (var attribute in ObjectDefinition.Attributes)
        //        {
        //            output.AppendFormat("{0}@{1}(", Indent(start), attribute.Name);

        //            if (attribute.Sets.Count > 0)
        //            {
        //                output.Append("{");

        //                output.AppendLine();

        //                for (var i = 0; i < attribute.Sets.Count; i++)
        //                {
        //                    output.AppendFormat("{0}{1}", Indent(start + 1), attribute.Sets[i]);

        //                    if (i < attribute.Sets.Count - 1)
        //                        output.Append(",");

        //                    output.AppendLine();
        //                }

        //                output.Append("}");
        //            }

        //            output.AppendFormat(")");
        //            output.AppendLine();
        //        }

        //        output.AppendFormat("{0}{1}interface {2}", Indent(start), ObjectDefinition.Export ? "export " : string.Empty, ObjectDefinition.Name);

        //        if (ObjectDefinition.HasInheritance)
        //        {
        //            if (ObjectDefinition.Implements.Count > 0)
        //                output.AppendFormat(" implements {0}", string.Join(", ", ObjectDefinition.Implements));
        //        }

        //        output.AppendFormat(" {0}", "{");
        //        output.AppendLine();

        //        if (ObjectDefinition.Properties.Count > 0)
        //        {
        //            foreach (var property in ObjectDefinition.Properties)
        //            {
        //                output.AppendFormat("{0}{1}: {2};", Indent(start + 1), property.Name, property.Type);
        //                output.AppendLine();
        //            }
        //        }

        //        if (ObjectDefinition.Methods.Count > 0)
        //        {
        //            foreach (var method in ObjectDefinition.Methods)
        //            {
        //                var parameters = string.Join(", ", method.Parameters.Select(item => string.Format("{0}: {1}", item.Name, item.Type)));

        //                output.AppendFormat("{0}{1}({2}): {3};", Indent(start + 1), method.Name, method.Parameters.Count == 0 ? string.Empty : parameters, method.Type);
        //                output.AppendLine();
        //            }
        //        }

        //        output.AppendFormat("{0}{1}", Indent(start), "}");
        //        output.AppendLine();

        //        if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
        //        {
        //            output.AppendFormat("{0}", "}");
        //            output.AppendLine();
        //        }

        //        return output.ToString();
        //    }
        //}
    }
}
