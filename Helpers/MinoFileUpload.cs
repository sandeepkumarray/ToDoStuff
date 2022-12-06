using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Helpers
{
    class MinoFileUpload
    {
        string endpoint = "192.168.0.2:9000";
        string accessKey = "myworldadmin";
        string secretKey = "myworldadmin";
        string bucketName = "my-world-main";
        string location = "in-south-1";
        public void Main()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12;
            //var endpoint = "play.min.io";
            //var accessKey = "Q3AM3UQ867SPQQA43P2F";
            //var secretKey = "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG";

            try
            {
                var minio = new MinioClient()
                                    .WithEndpoint(endpoint)
                                    .WithCredentials(accessKey, secretKey)
                                    .Build();
                Run(minio);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Added for Windows folks. Without it, the window, tests
            // run in, dissappears as soon as the test code completes.
            Console.ReadLine();
        }

        /// <summary>
        /// Task that uploads a file to a bucket
        /// </summary>
        /// <param name="minio"></param>
        /// <returns></returns>
        private void Run(MinioClient minio)
        {
            // Make a new bucket called mymusic.
            //var bucketName = "mymusic-folder"; //<==== change this
            //var location = "us-east-1";
            // Upload the zip file
            var objectName = "exppl.png";
            // The following is a source file that needs to be created in
            // your local filesystem.
            var filePath = @"C:\Users\sande\Pictures\exppl.png";
            var contentType = "application/jpg";

            try
            {
                var bktExistArgs = new BucketExistsArgs()
                                                .WithBucket(bucketName);
                bool found = minio.BucketExistsAsync(bktExistArgs).Result;
                if (!found)
                {
                    var mkBktArgs = new MakeBucketArgs()
                                                .WithBucket(bucketName)
                                                .WithLocation(location);
                    minio.MakeBucketAsync(mkBktArgs);
                }
                PutObjectArgs putObjectArgs = new PutObjectArgs()
                                                        .WithBucket(bucketName)
                                                        .WithObject(objectName)
                                                        .WithFileName(filePath)
                                                        .WithContentType(contentType);
                minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                Console.WriteLine("Successfully uploaded " + objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            // Added for Windows folks. Without it, the window, tests
            // run in, dissappears as soon as the test code completes.
            Console.ReadLine();
        }
    }
}
