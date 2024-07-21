using QuizMaker.Services;
using System.Xml;
using System.Xml.Linq;

namespace QuizMaker
{
    internal class Program
    {

        public static void foo1()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\yonat\\source\\repos\\QuizMaker\\QuizMaker\\Services\\Data.xml");
            Console.WriteLine("what's the question ?");
            string question = Console.ReadLine();
            Console.WriteLine("what's the answer ?");
            string answer = Console.ReadLine();

            XmlNode e = xmlDoc.CreateNode(XmlNodeType.Element, "item", "C:\\Users\\yonat\\source\\repos\\QuizMaker\\QuizMaker\\Services\\Data.xml");
            XmlElement questionnode = e.OwnerDocument.CreateElement("question");
            questionnode.InnerText = question;
            XmlElement answernode = e.OwnerDocument.CreateElement("answer");
            answernode.InnerText = answer;
        }
        public static void foo2()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\yonat\\source\\repos\\QuizMaker\\QuizMaker\\Services\\Data.xml");
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                XmlNode nodequestion = node.SelectSingleNode("question");
                Console.WriteLine(nodequestion.InnerText);
                string answer = Console.ReadLine();
                XmlNode nodeanswer = node.SelectSingleNode("answer");
                if (nodeanswer.InnerText.ToString().Trim() == answer)
                {
                    Console.WriteLine("great!!!");
                }
                else {
                    Console.WriteLine("you was wrong!!!");
                }
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                XmlDocument xmlDoc = new XmlDocument();
                Console.WriteLine("choose 1 to write questions, choose 2 to do quiz");
                string choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        foo1();
                        break;
                    case "2":
                        foo2();
                        break;
                    default:
                        Console.WriteLine("invalid choose");
                        break;
                }
            }
        }       
    }
}
