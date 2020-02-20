using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Models
{
    public class Response<T>
    {
        public String Message { get; set; }
        public T Item { get; set; }
        public List<T> Items { get; set; }
        public int TotalRows { get; set; }
    }
}