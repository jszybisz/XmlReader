using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using RxXmlReader;

namespace ConsoleApp1
{
    public class DataUpdaterService
    {
        public static int _iterator;

        public static Dictionary<string, PropertyInfo> busProperties = typeof(Bus)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
            .ToDictionary(x => x.Name);

        public async Task UpdateFromXml(string xmlPath, List<Bus> busses)
        {
            IList<Bus> updatedBusses = new List<Bus>();
            ICollection<string> names = new List<string>();
            var reader = XmlReader.Create(xmlPath);
            var sw = new Stopwatch();
            sw.Start();

            reader.ToObservable()
                .Where(node => node.IsElement("Bus"))
                .Select(node => new {BusId = node.GetAttribute("Id"), Reader = node})
                .Subscribe(busReader =>
                {
                    var id = int.Parse(busReader.BusId);
                    var bus = busses.FirstOrDefault(b => b.Id == id) ?? new Bus {Id = _iterator++};
                    updatedBusses.Add(bus);

                    busReader.Reader.ReadSubtree().ToObservable()
                        .Where(subNode => subNode.IsElement())
                        .ForEachAsync(prop => SetProperty(bus, prop));
                    busReader.Reader.Skip();
                }, () =>
                {
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    busses.Clear();
                    busses.AddRange(updatedBusses);
                    reader.Dispose();
                });
        }

        public void SetProperty(Bus bus, XmlReader propertyReader)
        {
            if (busProperties.ContainsKey(propertyReader.Name))
            {
                var prop = busProperties[propertyReader.Name];
                var conetent =
                    propertyReader.ReadElementContentAs(prop.PropertyType, new XmlNamespaceManager(new NameTable()));

                prop.SetValue(bus, conetent);
            }
        }
    }
}