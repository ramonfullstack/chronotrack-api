using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChronotrackService.Application.Utils
{
    public class AppSettings
    {
        public AppSettings()
        { }

        public AppSettings(IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            ConnChronotrack = Environment.GetEnvironmentVariable("ConnChronotrack");
            if (string.IsNullOrEmpty(ConnChronotrack))
                ConnChronotrack = appSettingsSection["ConnChronotrack"];
        }

        public string? ConnChronotrack { get; set; } = "";
 
    }
}
