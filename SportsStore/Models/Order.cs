﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessage = "Proszę podać imię i nazwisko.")]
        public string Name { set; get; }

        [Required(ErrorMessage ="Proszę podać ulicę, numer budynku i mieszkania.")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage ="Proszę podać nazwę miasta.")]
        public string City { get; set; }

        [Required(ErrorMessage ="Proszę podać nazwę województwa.")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage ="Proszę podać nazwę kraju.")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}