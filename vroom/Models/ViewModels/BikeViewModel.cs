﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vroom.Models.ViewModels
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        private List<Currency> CList = new List<Currency>();
        private List<Currency> CreateList()
        {
            CList.Add(new Currency("MAD", "MAD"));
            CList.Add(new Currency("USD", "USD"));
            CList.Add(new Currency("EURO", "EURO"));

            return CList;
        }
        public BikeViewModel()
        {
            Currencies = CreateList();
        }
    }
    public class Currency
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public Currency(String id,String name)
        {
            Id = id;
            Name = name;
        }
    }
}
