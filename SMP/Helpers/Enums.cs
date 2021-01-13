using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public class Enums
    {
    }

    public enum Raportet
    {
        [Description("Raporti për pagat - tabelare")]
        PagatTabelare = 1,
        [Description("Payslip")]
        Payslip = 3,
        [Description("Raporti i punëtorëve")]
        Punetoret = 2,
    }
}
