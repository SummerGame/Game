using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace StateMachine
{
    public static class MachineSerializer
    {

        public static void Serialize(Machine machine, string name)
        {
            XmlTextWriter textWritter = new XmlTextWriter(name+".xml", Encoding.UTF8);
            textWritter.WriteStartDocument(true);
            textWritter.WriteStartElement("machine");

            serializeStates(machine.States.ToList(), textWritter);
            serializeAlphabet(machine.Alphabet.Letters.ToList(), textWritter);
            serializeFunctions(machine.Functions.ToList(), textWritter);
            serializeFirstState(machine.FirstState, textWritter);

            textWritter.WriteEndElement();
            textWritter.Close();
        }

        public static void serializeFirstState(State state,XmlTextWriter writer)
        {
            writer.WriteStartElement("FirstState");
            writer.WriteAttributeString("Name", state.Name);
            writer.WriteEndElement();
        }

        private static void serializeStates(List<State> states, XmlTextWriter writer)
        {
            writer.WriteStartElement("States");
            foreach (State state in states)
            {
                writer.WriteStartElement("State");
                writer.WriteAttributeString("Name", state.Name);
                writer.WriteAttributeString("IsLast", state.IsLast.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void serializeAlphabet(List<Letter> letters, XmlTextWriter writer)
        {
            writer.WriteStartElement("Alphabet");
            foreach (Letter letter in letters)
            {
                writer.WriteStartElement("Letter");
                writer.WriteAttributeString("Name", letter.Name);
                writer.WriteAttributeString("Description", letter.Description);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void serializeFunctions(List<Function> functions, XmlTextWriter writer)
        {
            writer.WriteStartElement("Functions");
            foreach (Function function in functions)
            {
                writer.WriteStartElement("Function");
                writer.WriteAttributeString("Name", function.Name);
                writer.WriteAttributeString("From", function.From.Name);
                writer.WriteAttributeString("To", function.To.Name);
                writer.WriteAttributeString("Letter", function.Letter.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public static Machine Deserialize(string fileName)
        {
            Machine machine = new Machine();
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlElement xmlMachine = xml["machine"];

            var states = xmlMachine["States"];
            foreach (XmlElement xmlState in states)
            {
                State state = new State();
                var attributes = xmlState.Attributes;
                state.Name = attributes[0].Value;
                state.IsLast = Convert.ToBoolean(attributes[1].Value);
                machine.States.Add(state);
            }

            var letters = xmlMachine["Alphabet"];
            foreach (XmlElement xmlLetter in letters)
            {
                Letter letter = new Letter();
                var attributes = xmlLetter.Attributes;
                letter.Name = attributes[0].Value;
                letter.Description = attributes[1].Value;
                machine.Alphabet.Letters.Add(letter);
            }

            var functions = xmlMachine["Functions"];
            foreach (XmlElement xmlFunction in functions)
            {
                Function function = new Function();
                var attributes = xmlFunction.Attributes;
                function.Name = attributes[0].Value;
                function.From = machine.States.Where(n => n.Name == attributes[1].Value).ToList()[0];
                function.To = machine.States.Where(n => n.Name == attributes[2].Value).ToList()[0];
                function.Letter = machine.Alphabet.Letters.Where(n => n.Name == attributes[3].Value).ToList()[0];
                machine.Functions.Add(function);
            }

            try
            {
                var firstState = xmlMachine.LastChild;
                var stateName = firstState.Attributes[0].Value;
                machine.FirstState = machine.States.Where(n => n.Name == stateName).ToList()[0];
            }
            catch
            {
                machine.FirstState = null;
            }

            return machine;
        }

    }
}
