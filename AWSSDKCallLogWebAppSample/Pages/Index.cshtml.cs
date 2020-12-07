using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSSDKCallLogWebAppSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            BucketNameList = new List<string>();
        }

        public List<string> BucketNameList
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public async Task OnGet()
        {
            //Get AWSOptions object through the IConfiguration extension
            AWSOptions options = _configuration.GetAWSOptions();

            ViewData["AWSOptions.DefaultClientConfig.LogReponse"] = options.DefaultClientConfig.LogResponse;
            ViewData["AWSOptions.DefaultClientConfig.LogMetrics"] = options.DefaultClientConfig.LogMetrics;
            ViewData["AWSOptions.DefaultClientConfig.DisableLogging"] = options.DefaultClientConfig.DisableLogging;

            //Get logging parameters from the Legacy AWSConfig object and non-deprecated property LoggingConfigtring
            ViewData["Amazon.AWSConfigs.LoggingConfig.LogMetrics"] = Amazon.AWSConfigs.LoggingConfig.LogMetrics.ToString();
            ViewData["Amazon.AWSConfigs.LoggingConfig.LogResponses"] = Amazon.AWSConfigs.LoggingConfig.LogResponses.ToString();
            ViewData["Amazon.AWSConfigs.LoggingConfig.LogTo"] = Amazon.AWSConfigs.LoggingConfig.LogTo.ToString();

            //Get logging parameters from the Legacy AWSConfig object and deprecated properties
            ViewData["Amazon.AWSConfigs.Logging"] = Amazon.AWSConfigs.Logging.ToString();
            ViewData["Amazon.AWSConfigs.LogMetrics"] = Amazon.AWSConfigs.LogMetrics.ToString();
            ViewData["Amazon.AWSConfigs.ResponseLogging"] = Amazon.AWSConfigs.ResponseLogging.ToString();


            IAmazonS3 s3Client = options.CreateServiceClient<IAmazonS3>();
            ListBucketsResponse listBucketsReponse = await s3Client.ListBucketsAsync();
            if (listBucketsReponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (S3Bucket s3Bucket in listBucketsReponse.Buckets)
                {
                    BucketNameList.Add(s3Bucket.BucketName);
                }
            }
            else
            {
                ErrorMessage = listBucketsReponse.HttpStatusCode.ToString();
            }
            
        }
    }
}
