using fitshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitshop.App_Start.Generators
{
    interface IGenerate
    {

        byte[] Generate(List<food> foods);
    }
}
