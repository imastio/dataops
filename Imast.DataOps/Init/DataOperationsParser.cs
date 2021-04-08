using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Init
{
    /// <summary>
    /// The data operations definition parser
    /// </summary>
    public class DataOperationsParser
    {
        /// <summary>
        /// If parser should validate the schema
        /// </summary>
        private readonly bool validate;

        /// <summary>
        /// The reference to validation schema
        /// </summary>
        private XmlSchema validationSchema;

        /// <summary>
        /// Creates new instance of data operations parser
        /// </summary>
        /// <param name="validate">Indicate if validation is required</param>
        public DataOperationsParser(bool validate = false)
        {
            this.validate = validate;
        }
        
        /// <summary>
        /// Parse the given xml-based file into Data Operation Definition instance
        /// </summary>
        /// <param name="xml">The xml file to parse</param>
        /// <returns></returns>
        public DataOperationsDefinition Parse(string xml)
        {
            // open a stream to the file
            using var stream = new FileStream(xml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // parse the stream
            return this.Parse(stream);
        }

        /// <summary>
        /// Parse the given xml-based stream into Data Operation Definition instance
        /// </summary>
        /// <param name="xml">The stream to process</param>
        /// <returns></returns>
        public DataOperationsDefinition Parse(Stream xml)
        {
            // get validation schema
            var schema = this.GetValidationSchema();

            // create settings instance
            var settings = new XmlReaderSettings();
            
            // if schema is given for validation use it
            if (this.validate && schema != null)
            {
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
            }

            // create a reader for target xml
            using var reader = XmlReader.Create(xml, settings);
            
            // a document to load xml file in
            var document = new XmlDocument();
            
            // load reader content into document
            document.Load(reader);

            // keep errors if any
            var errors = new List<string>();
            
            // do validate if needed 
            if (this.validate)
            {
                // validate with proper callback of any event
                document.Validate((_, args) =>
                {
                    // skip any warnings
                    if (args.Severity != XmlSeverityType.Error)
                    {
                        return;
                    }

                    // on error add message to errors
                    errors.Add(args.Message);
                });
            }

            // in case of any error
            if (errors.Count != 0)
            {
                // raise an error
                throw new OperationParsingException(string.Join(Environment.NewLine, errors));
            }

            return this.ParseDocument(document);
        }

        /// <summary>
        /// Parse a valid Xml Document into Data Operations Definition instance
        /// </summary>
        /// <param name="document">The document to parse</param>
        /// <returns></returns>
        private DataOperationsDefinition ParseDocument(XmlDocument document)
        {
            // final result
            var result = new DataOperationsDefinition();

            // get configuration node if exists
            var config = document.GetElementsByTagName("DataConfiguration").Cast<XmlElement>().FirstOrDefault();

            // if configuration node found map it 
            if (config != null)
            {
                result.Configuration = MapConfiguration(config);
            }
            
            // get operation groups
            var groups = document.GetElementsByTagName("OperationGroup").Cast<XmlElement>();

            // map all the groups if any
            result.Groups = groups.Select(MapGroup).ToList();

            return result;
        }


        /// <summary>
        /// Gets the validation schema
        /// </summary>
        /// <returns></returns>
        private XmlSchema GetValidationSchema()
        {
            // use existing if any
            if (this.validationSchema != null)
            {
                return this.validationSchema;
            }

            // load validation xsd into the stream
            using var xsdStream = this.GetType().Assembly.GetManifestResourceStream("DataOps.xsd");

            // nothing was loaded
            if (xsdStream == null)
            {
                throw new FileLoadException("Could not load validation XSD schema");
            }

            this.validationSchema = XmlSchema.Read(xsdStream, null);

            // error while loading
            if (xsdStream == null)
            {
                throw new FileLoadException("Could not build validation schema from DataOps.xsd");
            }

            return this.validationSchema;
        }

        /// <summary>
        /// Maps the element into a group of operations
        /// </summary>
        /// <param name="element">The source element to map</param>
        /// <returns></returns>
        private static OperationGroup MapGroup(XmlElement element)
        {
            // create new instance of group 
            var group = new OperationGroup
            {
                Name = element.Attributes["Name"]?.Value
            };

            // map operations based on child elements
            group.Operations = element.GetElementsByTagName("SqlOperationDefinition")
                .Cast<XmlElement>()
                .Select(opElement => MapOperation(group, opElement))
                .ToList();

            // map transaction-specific options
            MapTransactionOptionsTo(element, group);

            // map provider specific options
            MapProviderOptionsTo(element, group);

            return group;
        }

        /// <summary>
        /// Maps the given element into an Sql Operation instance
        /// </summary>
        /// <param name="group">The group of operation</param>
        /// <param name="element">The element to map</param>
        /// <returns></returns>
        private static SqlOperationDefinition MapOperation(OperationGroup group, XmlElement element)
        {
            // create new operation
            var operation = new SqlOperationDefinition
            {
                Name = element.Attributes["Name"]?.Value,
                Group = group
            };

            // get command element of an operation
            var command = element.GetElementsByTagName("Command").Cast<XmlElement>().FirstOrDefault();
            
            // map operation command
            operation.Command = MapCommand(command);

            // map transaction-specific options
            MapTransactionOptionsTo(element, operation);

            // map provider specific options
            MapProviderOptionsTo(element, operation);
            
            return operation;
        }

        /// <summary>
        /// Maps the given element into a command definition 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static CommandDefinition MapCommand(XmlNode element)
        {
            // get command source
            var source = element.InnerText;

            // try get and parse expected result
            if (!Enum.TryParse<ExpectedResult>(element.Attributes?["ExpectedResult"]?.Value, out var expectedResult))
            {
                expectedResult = ExpectedResult.Unknown;
            }

            // try get and parse command type
            if (!Enum.TryParse<CommandTypeOption>(element.Attributes?["Type"]?.Value, out var commandType))
            {
                commandType = CommandTypeOption.Unknown;
            }
            
            // try get and parse NotSupported value
            if(!bool.TryParse(element.Attributes?["NotSupported"]?.Value ?? "false", out var notSupported))
            {
                notSupported = false;
            }

            // create an instance of command definition based on the element values
            return new CommandDefinition
            {
                Source = source,
                Type = commandType,
                NotSupported = notSupported,
                ExpectedResult = expectedResult
            };
        }

        /// <summary>
        /// Maps the element into a configuration instance
        /// </summary>
        /// <param name="element">The source element</param>
        /// <returns></returns>
        private static DataConfiguration MapConfiguration(XmlNode element)
        {
            // no configuration should be set
            if (element == null)
            {
                return null;
            }

            // create a configuration object
            var config = new DataConfiguration();
            
            // map transaction options
            MapTransactionOptionsTo(element, config);

            // map provider options
            MapProviderOptionsTo(element, config);

            return config;
        }
        
        /// <summary>
        /// Maps the element into transaction options
        /// </summary>
        /// <param name="element">The source element</param>
        /// <param name="options">The options to map to</param>
        private static void MapTransactionOptionsTo(XmlNode element, ITransactionOptions options)
        {
            // try get and parse transactional mode
            if (Enum.TryParse<AutoTransactionMode>(element.Attributes?["AutoTransactionMode"]?.Value, out var transactional))
            {
                options.AutoTransaction = transactional;
            }
        }
        
        /// <summary>
        /// Maps the element into provider options
        /// </summary>
        /// <param name="element">The source element</param>
        /// <param name="options">The options to map to</param>
        private static void MapProviderOptionsTo(XmlNode element, IProviderOptions options)
        {
            // try get and parse compatibility providers
            if (Enum.TryParse<CompatibilityProviders>(element.Attributes?["Compatibility"]?.Value, out var providers))
            {
                options.Providers = providers;
            }

            // try get and parse operation timeout
            if (TimeSpan.TryParse(element.Attributes?["Timeout"]?.Value, out var timeout))
            {
                options.Timeout = timeout;
            }
        }
    }
}