using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace RxXmlReader
{
    public static class XmlReaderExtensions
    {
        public static  IObservable<XmlReader> ToObservable(this XmlReader reader)
        {
            return  Observable.Create<XmlReader>(observer =>
                {
                    try
                    {
                        while (reader.Read())
                            observer.OnNext(reader);
                        observer.OnCompleted();
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    return reader;
                }
            );
        }

        public static bool Is(this XmlReader reader, string name, XmlNodeType nodeType  )
        {
            return reader.NodeType == nodeType && (name == null || reader.Name == name);
        }

        public static bool IsElement(this XmlReader reader, string name = null)
        {
            return reader.NodeType == XmlNodeType.Element && (name == null || reader.Name == name);
        }


    }
}