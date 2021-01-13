using SMP.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {        
            List<Alert> deserialize = new List<Alert>();
            if (TempData.ContainsKey(Alert.TempDataKey))
            {
                //     var alerts = TempData.ContainsKey(Alert.TempDataKey)
                //? (List<Alert>)TempData[Alert.TempDataKey]
                //: new List<Alert>();
                var alerts = TempData[Alert.TempDataKey];

                deserialize = JsonConvert.DeserializeObject<List<Alert>>(alerts.ToString());
            }

            return View(deserialize);
        }
    }
}
