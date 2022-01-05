using Microsoft.Extensions.Options;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.SslResponses;
using RealtimeGpsTracker.Core.Interfaces.Services;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Application.Services
{
    public class SslService : ISslService
    {
        public class SslSettings
        {
            // Constructor needed for binding
            public SslSettings() { }

            /// <summary>
            /// Public key (WWW version) for ssl .well-known/acme-challenge/
            /// </summary>
            public string PublicKey_With_WWW { get; set; } = null;

            /// <summary>
            /// Public key (No WWW version) for ssl .well-known/acme-challenge/
            /// </summary>
            public string PublicKey_Without_WWW { get; set; } = null;
        }

        //EXAMPLE_WITHOUT_WWW = "RvH6GP4ZmrHQ_4HwCoKj1-r4MqdS2A1JkPSKumKzL-w.0XWv2rehou-pv0nZr4l4g9yi5XEALxznE5Pe7p5FOJE";
        //EXAMPLE_WITH_WWW = "XonfJQJyEVUVg8-H1Mh0NFN2N3QAjigmcA1JQmbUo8Q.0XWv2rehou-pv0nZr4l4g9yi5XEALxznE5Pe7p5FOJE";


        private readonly SslSettings _sslSettings;

        public SslService(IOptions<SslSettings> sslSettings)
        {
            // Getting strongly typed Jwt Issuer, Audience, Key and MinutesValid settings
            _sslSettings = sslSettings.Value;
        }
        public DetailSslResponse GetSsl(string fileName)
        {
            // Holds ssl details
            DetailSslResponse sslResponse = new DetailSslResponse { Success = true };

            // Validate request format


            if (string.IsNullOrEmpty(_sslSettings.PublicKey_Without_WWW) ||
                string.IsNullOrEmpty(_sslSettings.PublicKey_With_WWW))
            {
                sslResponse.Success = false;
                sslResponse.Errors.Add("SSL keys are not available");
                return sslResponse;
            }

            var publicKeyWithoutWwwFileName = _sslSettings.PublicKey_Without_WWW.Split('.')[0];
            var publicKeyWithWwwFileName = _sslSettings.PublicKey_With_WWW.Split('.')[0];

            if (string.IsNullOrEmpty(publicKeyWithoutWwwFileName) ||
                string.IsNullOrEmpty(publicKeyWithWwwFileName))
            {
                sslResponse.Success = false;
                sslResponse.Errors.Add("SSL keys are not available");
                return sslResponse;
            }

            if (fileName.Equals(publicKeyWithoutWwwFileName))
            {
                sslResponse.FileContent = _sslSettings.PublicKey_Without_WWW;
                sslResponse.FileDownloadName = publicKeyWithoutWwwFileName;
            }
            else if (fileName.Equals(publicKeyWithWwwFileName))
            {
                sslResponse.FileContent = _sslSettings.PublicKey_With_WWW;
                sslResponse.FileDownloadName = publicKeyWithWwwFileName;
            }
            else
            {
                sslResponse.Success = false;
                sslResponse.FileContent = "";
                sslResponse.FileDownloadName = "file";
                sslResponse.Errors.Add("Requested SSL file name doesn't match");
                return sslResponse;
            }

            return sslResponse;
        }
    }
}
