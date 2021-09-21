using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace WebAPIS3Bucket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IAmazonS3 S3Client { get; set; }

        public HomeController(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

    


        public string filePath = "D:\\sample2.json";

        

        [HttpGet("ReadAll")]
        public async Task<string> ReadAll()
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = "exbucketsjson";
            request.Key = "Catalog1.json";

            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();

            return content;
        }


        [HttpGet("ReadbyID")]
        public async Task<catalog> ReadbyID(int id)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = "exbucketsjson";
            request.Key = "Catalog1.json";

            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<catalog> dese = JsonConvert.DeserializeObject<List<catalog>>(content);
            catalog result = dese.Single(a => a.Id == id);


            return result;
        }

        [HttpPost("CreateData")]
        public async Task<string> CreateData(int id, string name)
        {
            catalog catalogObject = new catalog();
            catalogObject.Id = id;
            catalogObject.Name = name;

            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = "exbucketsjson";
            request.Key = "Catalog1.json";


            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<catalog> dese = JsonConvert.DeserializeObject<List<catalog>>(content);
            dese.Add(catalogObject);

            string serializedval = JsonConvert.SerializeObject(dese);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = "exbucketsjson",

                Key = "Catalog1.json",
                FilePath = filePath,
                ContentType = "text/plain"
            };

            var response2 = S3Client.PutObjectAsync(putRequest);

            string returnContent;

            returnContent = "Catalog has been Inserted Successfully";
            return returnContent;
        }

        [HttpPost("UpdateData")]
        public async Task<string> UpdateData(int id, string name)
        {
            catalog catalogObject = new catalog();

            catalogObject.Id = id;
            catalogObject.Name = name;
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = "exbucketsjson";
            request.Key = "Catalog1.json";


            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<catalog> dese = JsonConvert.DeserializeObject<List<catalog>>(content);
            dese.RemoveAll(x => x.Id == id);
            dese.Add(catalogObject);
        
            string serializedval = JsonConvert.SerializeObject(dese);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = "exbucketsjson",

                Key = "Catalog1.json",
                FilePath = filePath,
                ContentType = "text/plain"
            };

            var response2 = S3Client.PutObjectAsync(putRequest);



            string returnContent;

            returnContent = "Catalog has been updated Successfully";
            return returnContent;
        }

        [HttpPost("DeleteData")]
        public async Task<string> DeleteData(int id)
        {
            catalog catalogObject = new catalog();

            catalogObject.Id = id;
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = "exbucketsjson";
            request.Key = "Catalog1.json";


            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<catalog> dese = JsonConvert.DeserializeObject<List<catalog>>(content);
            dese.RemoveAll(x => x.Id == id);
       
            string serializedval = JsonConvert.SerializeObject(dese);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = "exbucketsjson",

                Key = "Catalog1.json",
                FilePath = filePath,
                ContentType = "text/plain"
            };

            var response2 = S3Client.PutObjectAsync(putRequest);


            string returnContent;

            returnContent = "Catalog has been deleted Successfully";

            return returnContent;
        }
       

    }
}

